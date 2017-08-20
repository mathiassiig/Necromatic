using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UniRx;
namespace Necromatic.Items
{
    [Serializable]
    public class Inventory
    {
        private List<Item> Items = new List<Item>();
        public readonly ReactiveProperty<bool> ItemAdded = new ReactiveProperty<bool>();
        private int _maxItems = 100;


        public bool AddItem(ItemId id, int quantity = 1)
        {
            var item = Catalogue.GetItem(id);
            item.Quantity = quantity;
            return AddItem(item);
        }

        public int AmountOf(ItemId id)
        {
            var existingItem = Items.FirstOrDefault(x => x.Id == id);
            return existingItem == null ? 0 : existingItem.Quantity;
        }

        public Item Pop(ItemId id)
        {
            var existingItem = Items.FirstOrDefault(x => x.Id == id);
            if(existingItem == null)
            {
                return null;
            }
            Items.Remove(existingItem);
            ItemAdded.Value = !ItemAdded.Value;
            return existingItem;
        }

        public bool AddItem(Item i)
        {
            if (Items.Count >= _maxItems)
            {
                return false;
            }
            var existing = Items.FirstOrDefault(x => x.Id == i.Id);
            if (existing != null)
            {
                existing.Quantity += i.Quantity;
            }
            else
            {
                Items.Add(i);
            }
            ItemAdded.Value = !ItemAdded.Value;
            return true;
        }

        public bool Contains(ItemId id)
        {
            return Items.FirstOrDefault(x => x.Id == id) != null;
        }
    }
}