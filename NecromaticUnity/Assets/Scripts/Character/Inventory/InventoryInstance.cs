using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Necromatic.Character.Inventory
{
    [System.Serializable]
    public class InventoryInstance
    {
        public List<Item> Items = new List<Item>();
        public Item ShoulderSlot;
        public Item HeadSlot;
        public Item AmuletSlot;
        public Item HandSlot;
        public Item ChestSlot;
        public Item BackSlot;
        public Item WeaponSlot;
        public Item BootSlot;
        public Item OffhandSlot;
    }
}