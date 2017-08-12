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

        private bool _canTryAttack = true;

        void Update()
        {
            if(Input.GetMouseButton(0) && _canTryAttack)
            {
                _combatModule.TryAttack();
                _canTryAttack = false;
                Observable.Timer(TimeSpan.FromSeconds(0.1f)).First().Subscribe(_ => _canTryAttack = true);
            }
        }
    }
}