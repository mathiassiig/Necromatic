using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Necromatic.Character.Inventory
{
    [System.Serializable]
    public class InventoryInstance
    {
        public List<Item> Items = new List<Item>();
        public Item WeaponSlot;
        public Item OffhandSlot;
    }
}