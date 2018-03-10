using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Necromatic.Utility;
using Necromatic.Character.NPC;
using Necromatic.Character.NPC.Strategies;

namespace Necromatic.Character
{
    public interface Ability
    {
        bool PlayerFire();
        // todo: npc fire		
    }

    // todo: move to separate file
    public class RaiseCorpse : Ability
    {
        public bool PlayerFire()
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Corpse")))
            {
                Transform objectHit = hit.transform;
                var corpse = objectHit.parent.GetComponent<HumanDeath>();
                if (corpse != null && corpse.Dead.Value)
                {
                    Raise(corpse.gameObject, Object.FindObjectOfType<MotherPool>().GetCharacterPrefab(CharacterType.Skeleton));
                }
            }
            return false;
        }

        private void Raise(GameObject corpse, CharacterInstance undeadToRaise)
        {
            var undead = Object.Instantiate(undeadToRaise, corpse.transform.position, corpse.transform.rotation);
            var ai = undead.GetComponent<ArtificialIntelligence>();
            ai.SetBrainState(false);
            undead.Representation.ReviveAnimation(() => ai.SetBrainState(true));
            var player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterInstance>();
            ai.AddPrimaryStrategy(new FollowStrategy(player.transform, 2.5f, 7.5f));
            Object.Destroy(corpse);
        }
    }
}