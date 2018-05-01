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
                    break; // todo: figure something here in inventory
                }
                _itemSlots[i].ShowItem(inventory.Items[i]);
            }
            var asCharacter = inventory as CharacterInventory; // todo: handle this another way
            if (asCharacter != null)
            {
                _shoulderSlot.ShowItem(asCharacter.ShoulderSlot.Value);
                _headSlot.ShowItem(asCharacter.HeadSlot.Value);
                _amuletSlot.ShowItem(asCharacter.AmuletSlot.Value);
                _handSlot.ShowItem(asCharacter.HandSlot.Value);
                _chestSlot.ShowItem(asCharacter.ChestSlot.Value);
                _backSlot.ShowItem(asCharacter.BackSlot.Value);
                _weaponSlot.ShowItem(asCharacter.WeaponSlot.Value);
                _bootSlot.ShowItem(asCharacter.BootSlot.Value);
                _offhandSlot.ShowItem(asCharacter.OffhandSlot.Value);
                _weaponSlot.ShowItem(asCharacter.WeaponSlot.Value);
            }
            _inventory = inventory;
        }
    }
}