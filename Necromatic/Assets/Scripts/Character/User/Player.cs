using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UniRx;
namespace Necromatic.Char.User
{
    public class Player : Character
    {
        protected override void Init()
        {
            base.Init();
            _healthBar.gameObject.SetActive(false);
        }

        public void Cast(Action spell, string animation)
        {
            Movement.M_Animator.SetBool(animation, true);
            var animClip = Movement.M_Animator.runtimeAnimatorController.animationClips.FirstOrDefault(x => x.Equals(animation)); // animation has to have the same name as the trigger
            var length = animClip.length * Movement.M_Animator.speed * Time.timeScale;
            spell();

        }
    }
}