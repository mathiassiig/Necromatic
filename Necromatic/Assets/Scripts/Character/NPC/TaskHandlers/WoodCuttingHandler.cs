using UnityEngine;
using Necromatic.Items;
using Necromatic.Utility;
using UniRx;
using System.Linq;
using Necromatic.Char.Combat;
using System;

namespace Necromatic.Char.NPC.TaskHandlers
{
    public class WoodCuttingHandler : MonoBehaviour, ITaskHandler
    {
        [SerializeField] private LayerMask _trees;
        [SerializeField] private int _maxWood = 2;
        [SerializeField] private float _treeSearchRadius = 100;

        private Inventory _inventory;
        private CharacterNPCCombat _combat;
        private CharacterNPCMovement _movement;
        private Stash _currentStash;

        private int _currentWood => _inventory.AmountOf(ItemId.Wood);

        public ResourceTree CurrentTree { get; private set; }
        public Vector3 CurrentTreeCuttingPosition { get; private set; }

        // states
        private bool _hasTree => CurrentTree != null;
        public bool ShouldFindNewTree =>  !_hasTree && _currentWood < _maxWood;
        public bool MaxWoodReached => _currentWood >= _maxWood;
        public bool ShouldNavigateToTree => _hasTree 
            && Vector3Utils.XZDistanceGreater(transform.position, CurrentTreeCuttingPosition, 0.1f) 
            && !CurrentTree.Cut && !IsCuttingTree.Value;

        public bool ShouldTurnTowardsTree => _hasTree && !CurrentTree.Cut;
        public ReactiveProperty<bool> IsCuttingTree = new ReactiveProperty<bool>(false);


        public void Init(CharacterNPC npc)
        {
            _inventory = npc.Inventory;
            _movement = npc.NPCMovement;
            _combat = npc.NPCCombat;
            npc.AnimEvents.Attacking.Subscribe(value =>
            {
                if (value && CurrentTree != null && _currentWood < _maxWood)
                {
                    CutTree();
                }
                else
                {
                    if (CurrentTree != null)
                    {
                        CurrentTree.Disengange(gameObject);
                    }
                    CurrentTree = null;
                }
            });
        }

        private void CutTree()
        {
            _inventory.AddItem(ItemId.Wood);
            if (CurrentTree.Health.Current.Value <= 0 && !CurrentTree.Cut)
            {
                var force = (CurrentTree.transform.position - transform.position).normalized;
                CurrentTree.Timber(force);
                CurrentTree = null;
                //HandleLog();
            }
        }

        public Transform FindLumberStash()
        {
            CurrentTree = null;
            if (_currentStash != null)
            {
                return _currentStash.transform;
            }
            var stash = GameObject.FindGameObjectWithTag("Stash");
            if(stash != null)
            {
                var stashScript = stash.GetComponent<Stash>();
                _currentStash = stashScript;
                Observable.EveryUpdate().TakeUntilDestroy(this).TakeWhile((x) => _currentStash != null).Subscribe(_ =>
                {
                    if(!Vector3Utils.XZDistanceGreater(transform.position, _currentStash.transform.position, 2f))
                    {
                        StashWood();
                        _currentStash = null;
                    }
                });
                return stash.transform;
            }
            return null;
        }

        private void StashWood()
        {
            var wood = _inventory.Pop(ItemId.Wood);
            _currentStash.Inventory.AddItem(wood);
        }

        public Transform FindTree()
        {
            var transforms = Physics.OverlapSphere(transform.position, _treeSearchRadius, _trees).Select(x => x.transform).ToArray();
            if (transforms == null || transforms.Length == 0)
            {
                return null;
            }
            var closestTransform = Vector3Utils.GetClosestTransform(transforms, transform);
            var closestTree = transforms.FirstOrDefault(x => x == closestTransform).GetComponent<ResourceTree>();
            if (closestTree != null)
            {
                CurrentTree = closestTree;
            }
            CurrentTreeCuttingPosition = CurrentTree.Engage(gameObject, CurrentTree.DesiredPosition(transform));
            return CurrentTree.transform;
        }

        public void Think()
        {

        }

        public void TaskUpdate()
        {
            Vector3 treePos = Vector3.zero;
            if (ShouldFindNewTree)
            {
                FindTree();
            }
            else if (MaxWoodReached)
            {
                IsCuttingTree.Value = false;
            }

            if (ShouldNavigateToTree)
            {
                _movement.NavigateTo(CurrentTreeCuttingPosition);
            }
            else if (ShouldTurnTowardsTree)
            {
                _movement.StopMoving();
                _movement.LookAndDo(CurrentTree.transform, () =>
                {
                    _combat.InitAttack(CurrentTree);
                    IsCuttingTree.Value = true;
                });
            }
        }
    }
}