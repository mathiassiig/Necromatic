using Necromatic.Character.Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Necromatic.UI
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] private List<ItemUI> _itemSlots;
        [SerializeField] private ItemUI _shoulderSlot;
        [SerializeField] private ItemUI _headSlot;
        [SerializeField] private ItemUI _amuletSlot;
        [SerializeField] private ItemUI _handSlot;
        [SerializeField] private ItemUI _chestSlot;
        [SerializeField] private ItemUI _backSlot;
        [SerializeField] private ItemUI _weaponSlot;
        [SerializeField] private ItemUI _bootSlot;
        [SerializeField] private ItemUI _offhandSlot;
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
            _shoulderSlot.ShowItem(inventory.ShoulderSlot.Value);
            _headSlot.ShowItem(inventory.HeadSlot.Value);
            _amuletSlot.ShowItem(inventory.AmuletSlot.Value);
            _handSlot.ShowItem(inventory.HandSlot.Value);
            _chestSlot.ShowItem(inventory.ChestSlot.Value);
            _backSlot.ShowItem(inventory.BackSlot.Value);
            _weaponSlot.ShowItem(inventory.WeaponSlot.Value);
            _bootSlot.ShowItem(inventory.BootSlot.Value);
            _offhandSlot.ShowItem(inventory.OffhandSlot.Value);
            _weaponSlot.ShowItem(inventory.WeaponSlot.Value);
            _inventory = inventory;
        }
    }
}