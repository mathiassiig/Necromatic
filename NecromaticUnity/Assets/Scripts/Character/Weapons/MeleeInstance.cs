using System;
using System.Collections;
using System.Collections.Generic;
using Necromatic.Character.Inventory;
using UnityEngine;
using UniRx;

namespace Necromatic.Character.Weapons
{
    public class MeleeInstance : MonoBehaviour, IWeaponInstance
    {
        public IDisposable Attack(Weapon weaponData, IDamagable target, CharacterInstance attacker, Action onFinished = null, Action onHit = null)
        {
            var forward = weaponData.ForwardRetractRatio / weaponData.Speed;
            var retract = (1 - weaponData.ForwardRetractRatio) / weaponData.Speed;
            IDisposable attackingDisposabe;
            attacker.Combat.CurrentState.Value = CombatState.Attacking;
            attacker.Representation.Animate(weaponData.UseAnimation);
            attacker.Representation.SetAttackSpeed(weaponData.Speed);
            attackingDisposabe = Observable
                .Timer(TimeSpan.FromSeconds(forward))
                .TakeWhile((x) => attacker.Combat.CurrentState.Value == CombatState.Attacking)
                .Subscribe(x =>
                {
                    onHit?.Invoke();
                    target.Combat.ReceiveAttack(weaponData.BaseDamage, attacker);
                    attackingDisposabe = Observable
                        .Timer(TimeSpan.FromSeconds(retract))
                        .Subscribe(y =>
                    {
                        onFinished?.Invoke();
                    });
                });
            return attackingDisposabe;

        }
    }
}