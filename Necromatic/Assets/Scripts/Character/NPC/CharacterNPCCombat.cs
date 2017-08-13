using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Necromatic.Char.Combat;
using UniRx;
using Necromatic.Utility;
namespace Necromatic.Char.NPC
{
    public class CharacterNPCCombat : MonoBehaviour
    {
        public Character CurrentTarget { get; private set; }
        public bool TargetOutOfRange { get; private set; }
        private CharacterCombat _combat;
        private bool _engageEnemy = true;
		private bool _retaliate = true; // when true, switches target to who attacked
		[SerializeField] private float _detectionRange = 10f;

        private Character _characterScript;

		private void Awake()
		{
            _characterScript = GetComponent<Character>();
            if (_retaliate)
            {
                _characterScript.Health.Current.Subscribe(_ => HandleSwitchTarget(_characterScript.Health.LastSender));
			}
		}

        /// <summary>
        /// Should this NPC switch its current target to the attacker?
        /// </summary>
        private void HandleSwitchTarget(Character attacker)
        {
            if(attacker == null)
            {
                return;
            }
            // last sender damaged health, current target is seemingly not engaged with this npc
            if (!_characterScript.Health.LastSenderAdded && 
                (CurrentTarget.Combat.CurrentTarget != _characterScript || 
                !CurrentTarget.tag.Equals("Player") || 
                !Vector3Utils.PointingTowards(CurrentTarget.transform, _characterScript.transform, 90f)))
            {
                CurrentTarget = attacker;
            }
        }

        public void Init(CharacterCombat combat)
        {
            _combat = combat;
        }

        public void ThinkCombat()
        {
            // look for enemies
            var enemy = CurrentTarget;
            if (enemy == null || enemy.IsDead.Value)
            {
                enemy = _combat.GetEnemy(_detectionRange);
            }

            if (enemy != null && _engageEnemy)
            {
                CurrentTarget = enemy;
                var dis = (enemy.transform.position - transform.position).magnitude;
                if (dis > _combat.Weapon.Range)
                {
                    TargetOutOfRange = true;
                }
                else
                {
                    TargetOutOfRange = false;
                    _combat.Attack(enemy);
                }
            }
        }
    }
}