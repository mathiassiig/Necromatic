using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Necromatic.Utility;
using System.Linq;
using UniRx;
using System;

namespace Necromatic.Char.NPC
{
    public class UndeadWorkerNPC : UndeadNPC
    {
        [SerializeField] private GameObject _resourceWood;
        [SerializeField] private GameObject _resourceBag;
        [SerializeField] private LayerMask _trees;
        [SerializeField] private CharacterAnimationEvents _animEvents;

        #region Tree
        private float _treeSearchRadius = 10;
        private bool _hasTree => _currentTree != null;
        private ResourceTree _currentTree;
        private float _treeHitDamage = 50;
        private ReactiveProperty<bool> CanCutTree = new ReactiveProperty<bool>(true);
        private float _treeCutDelay = 0.5f;
        #endregion



        void Awake()
        {
            Init();
            _animEvents.Attacking.Subscribe(value =>
            {
                if (value)
                {
                    CutTree();
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
            if (_hasTree && Vector3Utils.XZDistanceGreater(transform.position, _currentTree.transform.position, 1.5f))
            {
                _npcMovement.NavigateTo(_currentTree.transform.position);
            }
            else if(_hasTree)
            {
                _npcMovement.StopMoving();
                if (_npcMovement.IsLookingTowards(_currentTree.transform))
                {
                    StartCutTree();
                }
                else
                {
                    _npcMovement.TurnTowardsObservable(_currentTree.transform);
                }
            }
            else
            {
                base.NPCUpdate();
            }
        }

        private void StartCutTree()
        {
            if (CanCutTree.Value)
            {
                Combat.AttackAnimation(_treeCutDelay);
                CanCutTree.Value = false;
                Observable.Timer(TimeSpan.FromSeconds(_treeCutDelay)).TakeUntilDisable(this).Subscribe(_ =>
                {
                    CanCutTree.Value = true;
                });
            }
        }

        private void CutTree()
        {
            _currentTree.Health.Add(-_treeHitDamage, this);
            if (_currentTree.Health.Current.Value <= 0)
            {
                var force = (_currentTree.transform.position - transform.position).normalized;
                _currentTree.Timber(force);
                _currentTree = null;
            }
        }

        private void FindTree()
        {
            var transforms = Physics.OverlapSphere(transform.position, _treeSearchRadius, _trees).Select(x => x.transform).ToArray();
            if(transforms == null || transforms.Length == 0)
            {
                return;
            }
            var closestTransform = Vector3Utils.GetClosestTransform(transforms, transform);
            var closestTree = transforms.FirstOrDefault(x => x == closestTransform).GetComponent<ResourceTree>();
            if(closestTree != null)
            {
                _currentTree = closestTree;
            }
        }
    }
}