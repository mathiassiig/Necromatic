using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UniRx;

namespace Necromatic.Character.Inventory
{
    [System.Serializable]
    public class InventoryInstance
    {
        public List<Item> Items = new List<Item>();
        public ReactiveProperty<Item> ShoulderSlot = new ReactiveProperty<Item>();
        public ReactiveProperty<Item> HeadSlot = new ReactiveProperty<Item>();
        public ReactiveProperty<Item> AmuletSlot = new ReactiveProperty<Item>();
        public ReactiveProperty<Item> HandSlot = new ReactiveProperty<Item>();
        public ReactiveProperty<Item> ChestSlot = new ReactiveProperty<Item>();
        public ReactiveProperty<Item> BackSlot = new ReactiveProperty<Item>();
        public ReactiveProperty<Item> WeaponSlot = new ReactiveProperty<Item>();
        public ReactiveProperty<Item> BootSlot = new ReactiveProperty<Item>();
        public ReactiveProperty<Item> OffhandSlot = new ReactiveProperty<Item>();
        private List<ReactiveProperty<Item>> _equippables;
        private Representation _representation;

        public void Init(Representation representation)
        {
            _representation = representation;
            /*WeaponSlot.Subscribe(x =>
            {
                if (x == null)
                {
                    _representation.SetItem(null, ItemSlotLocation.Weapon);
                }
            });
            OffhandSlot.Subscribe(x =>
            {

            });*/
        }

        public List<ReactiveProperty<Item>> Equippables
        {
            get
            {
                if (_equippables == null)
                {
                    _equippables = new List<ReactiveProperty<Item>>();
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

        public void EquipAny(ItemType type, ItemSlotLocation l)
        {
            var item = Items.Where(x => x != null).FirstOrDefault(x => x.Type == type);
            Equip(item, l);
        }

        public void EquipSpecial(SpecialType s, ItemSlotLocation l)
        {
            var slot = GetSlot(l);
            if(slot.Value != null && slot.Value.Special == s)
            {
                return;
            }
            var item = Items.Where(x => x != null).FirstOrDefault(x => x.Special == s);
            Equip(item, l);
        }

        private ReactiveProperty<Item> GetSlot(ItemSlotLocation l)
        {
            switch (l)
            {
                case ItemSlotLocation.Weapon:
                    return WeaponSlot;
                case ItemSlotLocation.Offhand:
                    return OffhandSlot;
            }
            return null;
        }

        public Item Equip(Item toEquip, ItemSlotLocation l)
        {
            var itemSlot = GetSlot(l);
            if (itemSlot.Value != null) // move old back
            {
                Items.Add(itemSlot.Value);
            }
            itemSlot.Value = toEquip;
            if (itemSlot != null)
            {
                Items.Remove(toEquip);
                itemSlot.Value.GameObjectInstance = _representation.SetItem(itemSlot.Value, l);
            }
            return itemSlot.Value;
        }


        public bool Has(SpecialType t)
        {
            bool inInventory = Items.Where(x => x != null).FirstOrDefault(x => x.Special == t) != null;
            bool inEquippables = Equippables.Where(x => x.Value != null).FirstOrDefault(x => x.Value.Special == t) != null;
            return inInventory || inEquippables;
        }


    }
}