using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Necromatic.Utility;
using System.Linq;
using UniRx;
using Necromatic.Items;
using System;

namespace Necromatic.Char.NPC
{
    public class UndeadWorkerNPC : UndeadNPC
    {
        [SerializeField]
        private GameObject _resourceWood;
        [SerializeField]
        private GameObject _resourceBag;
        [SerializeField]
        private LayerMask _trees;
        [SerializeField]
        private CharacterAnimationEvents _animEvents;

        #region Tree
        private float _treeSearchRadius = 10;
        private bool _hasTree => _currentTree != null;
        private ResourceTree _currentTree;
        private ReactiveProperty<bool> CanCutTree = new ReactiveProperty<bool>(true);
        private IDisposable _turningSubscription;
        #endregion

        void Awake()
        {
            Init();
            _animEvents.Attacking.Subscribe(value =>
            {
                if (value && _currentTree != null)
                {
                    CutTree();
                }
            });
            _inventory.ItemAdded.Subscribe(value =>
            {
                if (_inventory.Contains(ItemId.Wood))
                {
                    _animator.SetLayerWeight(0, 0);
                    _animator.SetLayerWeight(1, 1);
                    _resourceWood.SetActive(true);
                }
                else
                {
                    _animator.SetLayerWeight(1, 0);
                    _animator.SetLayerWeight(0, 1);
                    _resourceWood.SetActive(false);
                }
            });
        }

        protected override void Think()
        {
            if (!_hasTree)
            {
                FindTree(); // look for trees
                base.Think(); // and enemies
            }
            // if you have a tree, just focus on that until told otherwise
        }

        protected override void NPCUpdate()
        {
            if (_hasTree && Vector3Utils.XZDistanceGreater(transform.position, _currentTree.transform.position, 2f) && !_currentTree.Cut)
            {
                _npcMovement.NavigateTo(_currentTree.transform.position);
            }
            else if (_hasTree && !_currentTree.Cut)
            {
                _npcMovement.StopMoving();
                LookAndDo(_currentTree.transform, () =>
                {
                    Combat.InitAttack(_currentTree);
                    if (_turningSubscription != null)
                    {
                        _turningSubscription.Dispose();
                        _turningSubscription = null;
                    }
                });
            }
            else
            {
                base.NPCUpdate();
            }
        }

        private void CutTree()
        {
            _inventory.AddItem(ItemId.Wood);
            if (_currentTree.Health.Current.Value <= 0 && !_currentTree.Cut)
            {
                var force = (_currentTree.transform.position - transform.position).normalized;
                _currentTree.Timber(force);
                _currentTree = null;
                //HandleLog();
            }
        }

        private void FindTree()
        {
            var transforms = Physics.OverlapSphere(transform.position, _treeSearchRadius, _trees).Select(x => x.transform).ToArray();
            if (transforms == null || transforms.Length == 0)
            {
                return;
            }
            var closestTransform = Vector3Utils.GetClosestTransform(transforms, transform);
            var closestTree = transforms.FirstOrDefault(x => x == closestTransform).GetComponent<ResourceTree>();
            if (closestTree != null)
            {
                _currentTree = closestTree;
            }
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

        public void LookAndDo(Transform target, Action a)
        {
            if (_npcMovement.IsLookingTowards(target))
            {
                a();
            }
            else
            {
                Movement.TurnTowards(target);
            }
        }
    }
}