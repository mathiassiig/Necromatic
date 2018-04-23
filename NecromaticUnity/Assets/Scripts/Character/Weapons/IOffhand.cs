using Necromatic.Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Necromatic.Character.Weapons
{
    public interface IOffhand
    {
        void Use(GameObject target, CharacterInstance sender);
    }
}