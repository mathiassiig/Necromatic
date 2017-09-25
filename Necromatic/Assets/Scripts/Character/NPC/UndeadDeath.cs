using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DG.Tweening;
namespace Necromatic.Char
{
    public class UndeadDeath : MonoBehaviour, IDeath
    {
        private const float _timeUntilDisappear = 3f;
        private const float _amountToMoveUnderground = 3f;

        public void HandleDeath()
        {
            var npcScript = GetComponent<NPC.CharacterNPC>();
            npcScript.enabled = false;
            npcScript.Movement.M_Animator.SetTrigger("Death");
            npcScript.Movement.M_Animator.SetBool("Dead", true);
            npcScript.NPCMovement.StopMoving();
            gameObject.layer = LayerMask.NameToLayer("Corpse");
            //npcScript.EnableCircleBehaviour(false);
            Observable.Timer(TimeSpan.FromSeconds(_timeUntilDisappear * Time.timeScale))
                .Subscribe(_ =>
                {
                    transform.DOMoveY(transform.position.y - _amountToMoveUnderground, _timeUntilDisappear)
                    .SetEase(Ease.InExpo);
                });
        }
    }
}