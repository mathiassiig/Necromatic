using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Necromatic.Char.Combat;
using UniRx;
using Necromatic.Utility;
using System;

namespace Necromatic.Char.NPC
{
    public class CharacterNPCCombat : MonoBehaviour
    {
        public Character CurrentTarget { get; private set; }
        public bool TargetOutOfRange => (CurrentTarget.transform.position - transform.position).magnitude > _combat.Weapon.Range;
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

        public void InitAttack(Hurtable h)
        {
            _combat.InitAttack(h);
        }

        public void ThinkCombat()
        {
            // look for enemies
            var enemy = CurrentTarget;
            if (enemy == null || enemy.IsDead.Value)
            {
                enemy = _combat.GetEnemy(_detectionRange);
                _currentAttackTimer?.Dispose();
            }

            if (enemy != null && _engageEnemy && CurrentTarget != enemy)
            {
                CurrentTarget = enemy;
                StartAttackTimer(enemy);
            }
        }

        private IDisposable _currentAttackTimer;

        private void StartAttackTimer(Hurtable enemy)
        {
            _currentAttackTimer?.Dispose();
            _currentAttackTimer = Observable
                .Timer(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(_combat.Weapon.AttackTime))
                .TakeUntilDestroy(enemy.gameObject)
                .TakeUntilDestroy(gameObject)
                .Subscribe(_ =>
            {
                if(gameObject.name == "NPC_Human_Infantry")
                {
                    Debug.Log(_combat.Weapon.AttackTime);
                }
                if (!TargetOutOfRange)
                {
                    _combat.InitAttack(enemy);
                }
            });
        }
    }
}