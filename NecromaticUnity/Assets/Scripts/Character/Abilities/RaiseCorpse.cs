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
            var corpse = objectHit.parent.GetComponent<CharacterInstance>();
            if (corpse != null && corpse.Death.Dead.Value)
            {
                Raise(corpse.gameObject, Object.FindObjectOfType<MotherPool>().GetCharacterPrefab(CharacterType.Skeleton));
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

        private void Raise(GameObject corpse, CharacterInstance undeadToRaise)
        {
            var undead = Object.Instantiate(undeadToRaise, corpse.transform.position, corpse.transform.rotation);
            var ai = undead.GetComponent<ArtificialIntelligence>();
            ai.SetBrainState(false);
            var player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterInstance>();
            undead.Representation.ReviveAnimation(() =>
            {
                ai.SetBrainState(true);
                undead.Representation.LookDirectionAnim(GameObjectUtils.PlaneDirection(undead.transform, player.transform), 0.3f);
            });
            ai.AddPrimaryStrategy(new FollowStrategy(player.transform, 2.5f, 7.5f));
            Object.Destroy(corpse);
        }
    }
}