using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Necromatic.Char.Combat;
using UniRx;
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

		private void Awake()
		{
			if (_retaliate)
			{
                var character = GetComponent<Character>();
                character.Health.Current.Subscribe(_ =>
                {
                    //if (character.Health.LastSender != null && character.Health.LastSender)
                });
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