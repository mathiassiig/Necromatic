using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Necromatic.Character
{
    public class WeaponAnimator
    {
        private Vector3 _retracted = new Vector3(0.25f, 0, 1.8f);
        private Vector3 _forward = new Vector3(0.25f, 0, 3f);
        public void FireAnimation(CombatState state, Transform weapon, Combat c)
        {
            weapon.DOKill();
            switch (state)
            {
                case CombatState.Idle:
                    weapon.localPosition = _retracted;
                    break;
                case CombatState.Forward:
                    weapon.DOLocalMove(_forward, c.ForwardTime).SetEase(Ease.OutExpo);
                    break;
                case CombatState.Retracting:
                    weapon.DOLocalMove(_retracted, c.RetractTime).SetEase(Ease.OutExpo);
                    break;
            }
        }
    }
}