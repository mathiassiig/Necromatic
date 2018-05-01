using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Necromatic.Character.Inventory
{
    public class ItemInstance : MonoBehaviour
    {
        [SerializeField] private Item _itemData;
        public Item ItemData => _itemData;
    }
}