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
        public int Capacity = 32;

        public bool Add(Item i)
        {
            if (Items.Count >= Capacity)
            {
                return false;
            }
            Items.Add(i);
            return true;
        }

        public bool Add(StackableItem i)
        {

            var existing = Items.Cast<StackableItem>().FirstOrDefault(x => x.UniqueIdentifier == i.UniqueIdentifier);
            if(existing == null)
            {
                return Add(i as Item);
            }
            else
            {
                var toAdd = i.Count;
                if(existing.Count + toAdd <= existing.MaxStack)
                {
                    existing.Count += toAdd;
                    return true;
                }
                return false;
            }
        }

        public bool Remove(StackableItem i, int amountToRemove = 1)
        {
            if (Items.Contains(i))
            {
                var currentCount = i.Count;
                if (currentCount < amountToRemove)
                {
                    return false;
                }
                i.Count -= amountToRemove;
                if (i.Count == 0)
                {
                    Items.Remove(i);
                }
                return true;
            }
            return false;
        }

        public bool Remove(Item i)
        {
            if (Items.Contains(i))
            {
                Items.Remove(i);
                return true;
            }
            return false;
        }
    }
}