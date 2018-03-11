using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace Necromatic.Character.Abilities
{
    public interface Ability
    {
        bool PlayerFire(CharacterInstance sender);
        // todo: npc fire		
    }
}