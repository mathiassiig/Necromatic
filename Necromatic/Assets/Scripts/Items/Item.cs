using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Necromatic.Items
{
    public class Item
    {
        public string Name { get; set; }
        public ItemId Id { get; set; }
        public int Quantity { get; set; }
        public static Item Copy(Item i)
        {
            return new Item() { Name = i.Name, Id = i.Id, Quantity = i.Quantity };
        }
    }
}