using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Necromatic.Character.NPC
{
    public class HumanCharacter : CharacterInstance
    {
        protected override void Init()
        {
            _death = new HumanDeath();
            base.Init();
        }
    }
}