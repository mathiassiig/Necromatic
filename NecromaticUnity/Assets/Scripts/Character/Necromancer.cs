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
            var combat = new Combat(this);
            Combat = combat;
            _death = new Death();
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