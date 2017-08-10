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

        public void TryAttack(Character c)
        {
            if(_weapon.CanAttack.Value)
            {
                _weapon.Attack(c);
            }
        }
    }
}