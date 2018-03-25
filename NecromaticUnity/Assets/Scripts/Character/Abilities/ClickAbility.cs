using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Necromatic.Character.NPC;
using Necromatic.Character.NPC.Strategies;
using Necromatic.Utility;

namespace Necromatic.Character.Abilities
{
    public class ClickAbility : Ability
    {
        protected string _layer;
        protected CharacterInstance _sender;
        public override bool PlayerFire(CharacterInstance sender)
        {
            _sender = sender;
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer(GetLayer())))
            {
                HandleHitObject(hit);
            }
            return false;
        }

        public override void PlayerHover(CharacterInstance sender)
        {
            _sender = sender;
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer(GetLayer())))
            {
                HandleHoverObject(hit);
            }
        }

        protected virtual string GetLayer()
        {
            return _layer;
        }

        protected virtual void HandleHitObject(RaycastHit objectHit)
        {

        }

        protected virtual void HandleHoverObject(RaycastHit objectHit)
        {

        }

    }
}