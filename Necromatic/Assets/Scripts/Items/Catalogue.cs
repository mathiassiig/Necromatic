using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Necromatic.Items
{

    public enum ItemId
    {
        Wood
    }

    public static class Catalogue
    {
        private static Dictionary<ItemId, Item> _items = new Dictionary<ItemId, Item>
        {
            { ItemId.Wood, new Item { Name = nameof(ItemId.Wood), Type = ItemId.Wood } }
        };

        public static Item GetItem(ItemId iType)
        {
            return _items[iType];
        }
    }
}