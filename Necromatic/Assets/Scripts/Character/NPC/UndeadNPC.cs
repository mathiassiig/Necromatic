using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Necromatic.Char.Combat;
using DG.Tweening;
using UniRx;
using System;

namespace Necromatic.Char.NPC
{
    public class UndeadNPC : CharacterNPC
    {
        private const float _timeUntilDisappear = 3f;
        private const float _amountToMoveUnderground = 3f;
        void Awake()
        {
            Init();
        }

        protected override void HandleDeath()
        {
            enabled = false;
            Movement.M_Animator.SetTrigger("Death");
            Movement.M_Animator.SetBool("Dead", true);
            Observable.Timer(TimeSpan.FromSeconds(_timeUntilDisappear * Time.timeScale))
                .Subscribe(_ =>
                {
                    transform.DOMoveY(transform.position.y - _amountToMoveUnderground, _timeUntilDisappear)
                    .SetEase(Ease.InExpo);
                });
        }
    }
}