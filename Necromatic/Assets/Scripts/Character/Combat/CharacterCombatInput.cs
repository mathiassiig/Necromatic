using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using System;
namespace Necromatic.Character.Combat
{
    public class CharacterCombatInput : MonoBehaviour
    {
        [SerializeField] private CharacterCombat _combatModule;

        public bool CanTryAttack = true;

        void Update()
        {
            if(Input.GetMouseButton(0) && CanTryAttack)
            {
                _combatModule.TryAttack();
                CanTryAttack = false;
                Observable.Timer(TimeSpan.FromSeconds(0.1f)).First().Subscribe(_ => CanTryAttack = true);
            }
        }
    }
}