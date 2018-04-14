using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Necromatic.Character.NPC;
using Necromatic.Character.Inventory;

namespace Necromatic.Character
{
    public interface ISelectable
    {
        void Select();
        void Deselect();
        ArtificialIntelligence AI { get; }
        InventoryInstance Inventory { get; }
    }
}