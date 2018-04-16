using System.Collections;
using System.Collections.Generic;
using Necromatic.Character;
using Necromatic.Character.NPC.Strategies;
using Necromatic.Character.NPC.Strategies.Results;
using UnityEngine;
namespace Necromatic.World
{
    public class Log : MonoBehaviour, IClickReceiver
    {
        [SerializeField] private Tree _root;
        public void Click(List<ISelectable> senders)
        {
            foreach (var sender in senders)
            {
                if (sender.Inventory.Has(Character.Inventory.SpecialType.Axe))
                {
                    sender.Inventory.EquipSpecial(Character.Inventory.SpecialType.Axe, ItemSlotLocation.Weapon);
                    sender.AI.SetPrimaryStrategy(new SearchForTrees());
                    var cutThis = new TreeSpottedResult(_root);
                    sender.AI.AddTask(cutThis);
                }
                else
                {
                    Debug.Log("You need an axe"); // todo
                }
            }
        }
    }
}