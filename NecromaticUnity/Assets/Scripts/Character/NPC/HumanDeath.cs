using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Necromatic.Character.NPC
{
    public class HumanDeath : Death
    {
        [SerializeField] private Representation _representation;
        public override void Die()
        {
            Destroy(GetComponent<CharacterInstance>());
            Destroy(GetComponent<Movement>());
            Destroy(GetComponent<ArtificialIntelligence>());
            _representation.gameObject.AddComponent<BoxCollider>();
            _representation.DeathAnimation();
            Dead.Value = true;
        }
    }
}