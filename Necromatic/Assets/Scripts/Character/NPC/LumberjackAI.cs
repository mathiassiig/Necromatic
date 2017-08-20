using UnityEngine;
using Necromatic.Items;
using Necromatic.Utility;
using UniRx;
using System.Linq;

namespace Necromatic.Char.NPC
{
    public class LumberjackAI : MonoBehaviour
    {
        [SerializeField]
        private LayerMask _trees;

        private Inventory _inventory;
        private bool _hasTree => CurrentTree != null;
        private float _treeSearchRadius = 10;
        private int _maxWood = 2;
        private int _currentWood => _inventory.AmountOf(ItemId.Wood);
        //private CharacterNPCMovement _npcMovement;

        public ResourceTree CurrentTree { get; private set; }
        private Stash _currentStash;
        public bool ShouldFindNewTree =>  !_hasTree && _currentWood < _maxWood;
        public bool MaxWoodReached => _currentWood >= _maxWood;
        public bool ShouldNavigateToTree => _hasTree && Vector3Utils.XZDistanceGreater(transform.position, CurrentTree.transform.position, 2f) && !CurrentTree.Cut;
        public bool ShouldTurnTowardsTree => _hasTree && !CurrentTree.Cut;

        public void Init(Inventory inventory, CharacterAnimationEvents animEvents)
        {
            _inventory = inventory;
            //_npcMovement = npcMovement;
            animEvents.Attacking.Subscribe(value =>
            {
                if (value && CurrentTree != null && _currentWood < _maxWood)
                {
                    CutTree();
                }
                else
                {
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
            if(_currentStash != null)
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
                    if(!Vector3Utils.XZDistanceGreater(transform.position, _currentStash.transform.position, 3f))
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
            return CurrentTree.transform;
        }

        //private bool _pullingLog = false;

        /*
        private void HandleLog()
        {
            _pullingLog = false;
            Observable.Timer(TimeSpan.FromSeconds(3.4f)).TakeUntilDestroy(this).Subscribe(_ =>
            {
                var obs = FollowTarget(_currentTree.WorkerPosition);
                obs.TakeWhile((x) => gameObject.activeInHierarchy && _currentTree.gameObject != null && _pullingLog == false).Subscribe(x =>
                {
                    Debug.Log(Vector3Utils.XZDistance(_currentTree.WorkerPosition.position, transform.position));
                    if (!Vector3Utils.XZDistanceGreater(_currentTree.WorkerPosition.position, transform.position, 3f))
                    {
                        _pullingLog = true;
                        Destroy(_currentTree.gameObject);
                        _currentTree = null;
                        StopFollowTarget();
                        _inventory.AddItem(ItemId.Wood);
                    }
                });
            });
        }*/
    }
}