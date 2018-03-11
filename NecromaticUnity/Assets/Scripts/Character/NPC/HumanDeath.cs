using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Necromatic.Character.NPC
{
    public class HumanDeath : Death
    {
        public override void Die(CharacterInstance c)
        {
            Object.Destroy(c.GetComponent<Movement>());
            Object.Destroy(c.GetComponent<ArtificialIntelligence>());
            c.Representation.gameObject.AddComponent<BoxCollider>();
            c.Representation.DeathAnimation();
            Dead.Value = true;
        }
    }
}