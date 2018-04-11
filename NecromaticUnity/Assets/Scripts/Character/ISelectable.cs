using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Necromatic.Character.NPC;
namespace Necromatic.Character
{
    public interface ISelectable
    {
        void Select();
        void Deselect();
        ArtificialIntelligence AI { get; }
    }
}