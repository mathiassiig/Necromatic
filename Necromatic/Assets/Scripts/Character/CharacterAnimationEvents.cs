using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
namespace Necromatic.Char
{
    public class CharacterAnimationEvents : MonoBehaviour
    {
        public readonly ReactiveProperty<bool> Attacking = new ReactiveProperty<bool>();
        public void Attack()
        {
            // emit pulse
            Attacking.Value = true;
            Attacking.Value = false;
        }
    }
}