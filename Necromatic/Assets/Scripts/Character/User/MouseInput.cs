using Necromatic.Char.Combat;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Necromatic.Managers;
using System.Linq;
namespace Necromatic.Char.User
{
    public class MouseInput : MonoBehaviour
    {
        [SerializeField]
        private CharacterCombat _combatModule;
        [SerializeField]
        private SquareSelect _selector;
        [SerializeField]
        private Material _highlightMaterial;

        private Player _player;

        private MicroManager _microManager = new MicroManager();

        private bool _canTryAttack = true;

        void Awake()
        {
            _selector.SelectedUnits.Subscribe(x =>
            {
                _microManager.UpdatedSelectedUnits(x);
            });
            _player = FindObjectOfType<Player>();
        }

        #region UpdateMethods
        void Update()
        {
            HighlightClickables();
            CheckLeftClick();
        }

        void FixedUpdate()
        {
            CheckRightClick();
        }
        #endregion

        #region InputMethods
        void HighlightClickables()
        {
            RaycastFromMouse(HighlightClickable);
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
            if (Input.GetMouseButtonDown(1))
            {
                RaycastFromMouse(HandleRaycastResult);
            }
        }
        #endregion


        private void RaycastFromMouse(Action<RaycastHit> methodToCall)
        {
            // create a ray cast and set it to the mouses cursor position in game
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                methodToCall(hit);
            }
        }

        #region ActionMethods
        private Renderer _highlightedRenderer;
        private void HighlightClickable(RaycastHit hit)
        {
            /*
            if (hit.collider.gameObject.layer != LayerMask.NameToLayer("Ground"))
            {
                var renderer = hit.collider.gameObject.GetComponentInChildren<Renderer>();
                if (renderer == _highlightedRenderer)
                {
                    return;
                }
                if (_highlightedRenderer != null)
                {
                    var mats = _highlightedRenderer.materials.ToList();
                    mats.Remove(mats.FirstOrDefault(m => m.name.Equals($"{_highlightMaterial.name} (Instance)")));
                    _highlightedRenderer.materials = mats.ToArray();
                }
                _highlightedRenderer = renderer;
                List<Material> materials = renderer.materials.ToList();
                materials.Add(_highlightMaterial);
                renderer.materials = materials.ToArray();
            }
            */
        }

        private void HandleRaycastResult(RaycastHit hit)
        {
            var go = hit.collider.gameObject;
            if (_microManager.SelectedUnits != null && _microManager.SelectedUnits.Count > 0)
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
                    var corpse = go.GetComponent<Corpse>();
                    _player.Cast(() =>
                    {
                        corpse.Resurrect();
                    }, 
                    "Cast B",
                    "UD_mage_11_cast_B",
                    corpse.transform.position,
                    1.5f);
                    break;
            }
        }
        #endregion
    }
}