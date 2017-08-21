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
        private CharacterAnimationEvents _animEvents;
        [SerializeField]
        private LumberjackAI _lumberAI;

        private IDisposable _turningSubscription;

        private Vector3 _resourceNavigationPosition;
        private Hurtable _resourceHurtable;

        void Awake()
        {
            Init();
            _lumberAI.Init(_inventory, _animEvents, Combat.Weapon);
            _inventory.ItemAdded.Subscribe(value =>
            {
                var hasWood = _inventory.Contains(ItemId.Wood);
                _animator.SetLayerWeight(0, hasWood ? 0 : 1);
                _animator.SetLayerWeight(1, hasWood ? 1 : 0);
                _resourceWood.SetActive(hasWood);
            });
        }

        protected override void Think()
        {
            base.Think();
        }

        protected override void NPCUpdate()
        {
            if (_lumberAI.ShouldFindNewTree)
            {
                _lumberAI.FindTree();
                _resourceNavigationPosition = _lumberAI.CurrentTreeCuttingPosition;
                _resourceHurtable = _lumberAI.CurrentTree;
            }
            else if (_lumberAI.MaxWoodReached)
            {
                _resourceNavigationPosition = _lumberAI.FindLumberStash().position;
            }
            if (_lumberAI.ShouldNavigateToTree || _lumberAI.MaxWoodReached)
            {
                _npcMovement.NavigateTo(_resourceNavigationPosition);
            }
            else if (_lumberAI.ShouldTurnTowardsTree)
            {
                _npcMovement.StopMoving();
                LookAndDo(_lumberAI.CurrentTree.transform, () =>
                {
                    Combat.InitAttack(_lumberAI.CurrentTree);
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
    }
}