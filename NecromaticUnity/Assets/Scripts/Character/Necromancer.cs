using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Necromatic.Utility;
namespace Necromatic.Character
{
    public class Necromancer : CharacterInstance
    {
        protected override void Init()
        {
            _death = new Death();
            var combat = new Combat(this);
            Combat = combat;
            FindObjectOfType<MotherPool>().AddBarToCharacter(this);
            base.Init();
        }

        public void DoAbility()
        {
            CurrentAbility.PlayerFire(this);
        }

        public void DoHoverAbility()
        {
            CurrentAbility.PlayerHover(this);
        }
    }
}