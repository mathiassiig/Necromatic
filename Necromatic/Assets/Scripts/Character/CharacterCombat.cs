using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
namespace Necromatic.Character
{
    public class CharacterCombat : MonoBehaviour
    {
        [SerializeField] private float _damagePerAttack;
        [SerializeField] private float _coolDown; // in seconds
        private bool _canAttack = true;

        public void TryAttack(Character c)
        {
            if(_canAttack && c != null)
            {
                Attack(c);
                _canAttack = false;
                Observable.Timer(TimeSpan.FromSeconds(_coolDown)).First().Subscribe(_ => _canAttack = true);
            }
        }

        private void Attack(Character c)
        {
            c.Health.Add(-_damagePerAttack);
        }
    }
}