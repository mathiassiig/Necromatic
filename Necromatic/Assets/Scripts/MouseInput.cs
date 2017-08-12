using Necromatic.Character.Combat;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
namespace Necromatic
{
    public class MouseInput : MonoBehaviour
    {
        [SerializeField] private CharacterCombat _combatModule;
        [SerializeField] private SquareSelect _selector;

        private bool _canTryAttack = true;

        // Update is called once per frame
        void FixedUpdate()
        {
            CheckLeftClick();
            CheckRightClick();
        }

        private void CheckLeftClick()
        {
            if (Input.GetMouseButton(0))
            {
                _selector.Select(Input.mousePosition);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                _selector.SelectionDone();
            }
        }

        void CheckRightClick()
        {
            if (Input.GetMouseButton(1))
            {
                // create a ray cast and set it to the mouses cursor position in game
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    // Debug.DrawLine(ray.origin, hit.point);
                    HandleRaycastResult(hit);
                }
            }
        }

        private void HandleRaycastResult(RaycastHit hit)
        {
            var go = hit.collider.gameObject;
            switch (hit.collider.tag)
            {
                case "Untagged": // if there is nothing, the user probably wants to attack the nearest enemy...?
                case "Character":
                    if (_canTryAttack)
                    {
                        _combatModule.TryAttack();
                        _canTryAttack = false;
                        Observable.Timer(TimeSpan.FromSeconds(0.1f)).First().Subscribe(_ => _canTryAttack = true);
                    }
                    break;
                //case "Character":
                //    var character = go.GetComponent<Necromatic.Character.Character>();
                //    if(character.Type != CharacterType.NPCUndeadInfantry)
                //    {
                //        // attack
                //    }
                //    break;
                case "Corpse":
                    var corpse = go.GetComponent<Necromatic.Character.Combat.Corpse>();
                    corpse.Resurrect();
                    break;
            }
        }
    }
}