using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Necromatic.Character
{
    public class WeaponAnimator
    {
        public void FireAnimation(CombatState state, Transform weapon, Combat c)
        {
            weapon.DOKill();
            switch (state)
            {
                case CombatState.Idle:
                    weapon.localPosition = new Vector3(0, 0, 1.8f);
					break;
                case CombatState.Forward:
                    weapon.DOLocalMove(new Vector3(0, 0, 3), c.ForwardTime).SetEase(Ease.OutExpo);
                    break;
                case CombatState.Retracting:
                    weapon.DOLocalMove(new Vector3(0, 0, 1.8f), c.RetractTime).SetEase(Ease.OutExpo);
                    break;
            }
        }
    }
}