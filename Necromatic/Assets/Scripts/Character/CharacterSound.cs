using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Necromatic.Char
{
    /// <summary>
    /// This class serves as an interface between the animator and the classes holding the audio sources
    /// </summary>
    public class CharacterSound : MonoBehaviour
    {
        [SerializeField] private Combat.CharacterCombat _combat;
        [SerializeField] private CharacterMovement _movement;
        public void Attack()
        {
            _combat.Weapon.PlaySound();
        }

        public void StepA()
        {
            _movement.PlayStep(Managers.SoundEffect.Step_Grass_A);
        }

        public void StepB()
        {
            _movement.PlayStep(Managers.SoundEffect.Step_Grass_B);
        }
    }
}