using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
        private List<Item> _equippables;
        public List<Item> Equippables
        {
            get
            {
                if (_equippables == null)
                {
                    _equippables = new List<Item>();
                    _equippables.Add(ShoulderSlot);
                    _equippables.Add(HeadSlot);
                    _equippables.Add(AmuletSlot);
                    _equippables.Add(HandSlot);
                    _equippables.Add(ChestSlot);
                    _equippables.Add(BackSlot);
                    _equippables.Add(WeaponSlot);
                    _equippables.Add(BootSlot);
                    _equippables.Add(OffhandSlot);

                }
                return _equippables;
            }
        }


        public bool Has(SpecialType t)
        {
            bool inInventory = Items.Where(x => x != null).FirstOrDefault(x => x.Special == t) != null;
            bool inEquippables = Equippables.Where(x => x != null).FirstOrDefault(x => x.Special == t) != null;
            return inInventory || inEquippables;
        }


    }
}