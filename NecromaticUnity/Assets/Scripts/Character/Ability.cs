using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Necromatic.Utility;

namespace Necromatic.Character
{
    public interface Ability
    {
        bool PlayerFire();
		// todo: npc fire		
    }
    // 100000000
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
				Debug.Log(corpse);
                if (corpse != null && corpse.Dead.Value)
                {
					Debug.Log("Raise");
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
            Object.Destroy(corpse);
        }
    }
}