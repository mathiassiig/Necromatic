using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Necromatic.Character.NPC;
using Necromatic.Character.NPC.Strategies;
using Necromatic.Utility;

namespace Necromatic.Character.Abilities
{
    public class RaiseCorpse : ClickAbility
    {
        protected override void HandleHitObject(Transform objectHit)
        {
            var raisable = objectHit.GetComponent<IRaisable>();
            if (raisable != null)
            {
                raisable.Raise();
            }
        }

        protected override string GetLayer()
        {
            return "Corpse";
        }

        public override string GetIconPath()
        {
            return $"{base.GetIconPath()}icon_raise";
        }

    }
}