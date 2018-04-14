using Necromatic.Character.Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Necromatic.UI
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] private List<ItemUI> _itemSlots;
        [SerializeField] private ItemUI _weaponSlot;
        private InventoryInstance _inventory;

        public void Populate(InventoryInstance inventory)
        {
            // todo: clear the rest
            for (int i = 0; i < inventory.Items.Count; i++)
            {
                if (i > _itemSlots.Count - 1)
                {
                    break; // todo: figure something here in inventory, shouldn't have that many items
                }
                _itemSlots[i].ShowItem(inventory.Items[i]);
            }
            _weaponSlot.ShowItem(inventory.WeaponSlot);
            _inventory = inventory;
        }
    }
}