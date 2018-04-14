using Necromatic.Character.Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Necromatic.UI
{
    public class ItemUI : MonoBehaviour
    {
        [SerializeField] private Image _itemIcon;
        private Item _item;

        public void ShowItem(Item item)
        {
            _item = item;
            if (_item == null)
            {
                _itemIcon.sprite = null;
            }
            else
            {
                _itemIcon.sprite = item.Icon;
            }
        }
    }
}