using Necromatic.Char.Combat;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Necromatic.Managers;
namespace Necromatic.Char.User
{
    public class MouseInput : MonoBehaviour
    {
        [SerializeField] private CharacterCombat _combatModule;
        [SerializeField] private SquareSelect _selector;

        private MicroManager _microManager = new MicroManager();

        private bool _canTryAttack = true;

        void Awake()
        {
            _selector.SelectedUnits.Subscribe(x =>
            {
                _microManager.UpdatedSelectedUnits(x);
            });
        }

        void Update()
        {
            CheckLeftClick();
        }

        void FixedUpdate()
        {
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
            if(_microManager.SelectedUnits != null && _microManager.SelectedUnits.Count > 0)
            {
                _microManager.HandleUserHit(hit);
                return;
            }
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
                    var corpse = go.GetComponent<Necromatic.Char.Combat.Corpse>();
                    corpse.Resurrect();
                    break;
            }
        }
    }
}