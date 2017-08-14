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

        public void Cast(Action spell, string animBool, string animationName, Vector3 lookAt, float speed = 1, float castAfterT = 0)
        {
            Movement.M_Animator.SetBool(animBool, true);
            Movement.M_Animator.SetFloat($"{animBool}_speed", speed);
            Movement.ShouldMove = false;
            var animClip = Movement.M_Animator.runtimeAnimatorController.animationClips.FirstOrDefault(x => x.name.Equals(animationName));
            var length = (animClip.length / speed) * Time.timeScale;;
            Movement.TurnTowards(lookAt, true);
            Movement.StopMovement();
            Observable.Timer(TimeSpan.FromSeconds(castAfterT*length)).First().Subscribe(_ =>
            {
                spell();
            });
            Observable.Timer(TimeSpan.FromSeconds(length)).First().Subscribe(_ =>
            {
                Movement.ShouldMove = true;
            });
        }
    }
}