using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
namespace Necromatic.Character.Combat
{
    public class CharacterCombat : MonoBehaviour
    {
        [SerializeField] private WeaponBase _weapon;
        [SerializeField] private Animator _animator;
        public bool Attacking { get; private set; }

        public void TryAttack(Character c)
        {
            if(_weapon.CanAttack.Value)
            {
                _weapon.Attack(c);
                _animator.SetTrigger("Attack");
                Attacking = true;
                Observable.Timer(TimeSpan.FromSeconds(_weapon.Cooldown)).First().Subscribe(_ =>
                {
                    Attacking = false;
                });
            }
        }
    }
}