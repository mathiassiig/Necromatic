using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Necromatic.Character.Combat
{
    public class CharacterCombatInput : MonoBehaviour
    {
        [SerializeField] private CharacterCombat _combatModule;

        public Character TEST_CHARACTER_TO_ATTACK;

        void Update()
        {
            if(Input.GetMouseButton(0))
            {
                _combatModule.TryAttack(TEST_CHARACTER_TO_ATTACK);
            }
        }
    }
}