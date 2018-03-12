using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;
using Necromatic.Character.NPC;
using Necromatic.Character.NPC.Strategies;
using Necromatic.Utility;

namespace Necromatic.Character.Abilities
{
    public class Sacrifice : ClickAbility
    {

        protected override void HandleHitObject(Transform objectHit)
        {
            var c = objectHit.GetComponent<CharacterInstance>();
            if (c != null && !c.Death.Dead.Value && c.Faction == _sender.Faction)
            {
                Kill(c);
            }
        }

        protected override string GetLayer()
        {
            return "Character";
        }

        public override string GetIconPath()
        {
            return $"{base.GetIconPath()}icon_sacrifice";
        }

        private void Kill(CharacterInstance toSacrifice)
        {
            var alreadyHas = toSacrifice.Buffs.FirstOrDefault(x => x.GetType() == typeof(SacrificialBuff)) != null;
            if (!alreadyHas)
            {
                toSacrifice.AddBuff(new SacrificialBuff());
                Observable
                    .EveryUpdate()
                    .TakeUntilDestroy(toSacrifice)
                    .TakeWhile((x) => !toSacrifice.Death.Dead.Value && !_sender.Death.Dead.Value)
                    .Subscribe(_ =>
                    {
                        var hpStolen = toSacrifice.Health.Add(-2);
                        _sender.Health.Add(hpStolen);
                    });
            }
        }
    }
}