using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace Necromatic.Character.Abilities
{
    public class Ability
    {
        public virtual bool PlayerFire(CharacterInstance sender)
        {
            return false;
        }

        public virtual void PlayerHover(CharacterInstance sender)
        {
            
        }
        
        public virtual string GetIconPath()
        {
            return "Images/UI/Icons/";
        }

        public virtual string GetDescription => "";
        public virtual string GetName => "";
    }
}