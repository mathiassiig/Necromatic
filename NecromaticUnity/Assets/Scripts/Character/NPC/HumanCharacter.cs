using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Necromatic.Character.NPC
{
    public class HumanCharacter : CharacterInstance
    {
        protected override void Init()
        {
            var combat = new Combat(this, 10, 0.3f, 0.2f, 1);
			Combat = combat;
            _death = new HumanDeath();
            base.Init();
        }
    }
}