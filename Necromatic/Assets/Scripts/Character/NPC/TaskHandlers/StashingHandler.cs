using UnityEngine;
using Necromatic.Items;
using Necromatic.Utility;
using UniRx;
using System.Linq;
using System;

namespace Necromatic.Char.NPC.TaskHandlers
{
    public class StashingHandler : ITaskHandler
    {
        public Stash CurrentStash { get; private set; }
        private Inventory _inventory;
        private CharacterNPCMovement _movement;
        public ReactiveProperty<bool> StashingDone = new ReactiveProperty<bool>(false);

        public void Init(CharacterNPC npc)
        {
            _inventory = npc.Inventory;
            _movement = npc.NPCMovement;
        }

        public void TaskUpdate()
        {
            if (CurrentStash != null)
            {
                _movement.NavigateTo(CurrentStash.transform.position);
            }
        }

        public void Think()
        {
        }

        public Transform FindStash()
        {
            StashingDone.Value = false;
            var stash = GameObject.FindGameObjectWithTag("Stash");
            if (stash != null)
            {
                var stashScript = stash.GetComponent<Stash>();
                CurrentStash = stashScript;
                Observable.EveryUpdate().TakeUntilDestroy(_movement).TakeWhile((x) => CurrentStash != null).Subscribe(_ =>
                {
                    if (!Vector3Utils.XZDistanceGreater(_movement.transform.position, CurrentStash.transform.position, 2f))
                    {
                        StashWood();
                        CurrentStash = null;
                    }
                });
                return stash.transform;
            }
            return null;
        }

        private void StashWood()
        {
            StashingDone.Value = true;
            var wood = _inventory.Pop(ItemId.Wood);
            CurrentStash.Inventory.AddItem(wood);
        }
    }
}
