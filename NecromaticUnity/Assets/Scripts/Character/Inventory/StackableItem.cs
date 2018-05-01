using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Necromatic.Character.Inventory
{
    [CreateAssetMenu(fileName = "NewItem", menuName = "Necromatic/Items/StackableItem")]
    public class StackableItem : Item
    {
        public int MaxStack;
        public int Count;
        public string UniqueIdentifier;
    }
}