using Necromatic.Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IOffhand 
{
    void Use(GameObject target, CharacterInstance sender);
}
