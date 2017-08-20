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

        private Transform _resourceNavigationTarget;
        private Hurtable _resourceHurtable;

        void Awake()
        {
            Init();
            _lumberAI.Init(_inventory, _animEvents);
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
            if (_lumberAI.ShouldFindNewTree)
            {
                _resourceNavigationTarget = _lumberAI.FindTree();
                _resourceHurtable = _lumberAI.CurrentTree;
            }
            else if(_lumberAI.MaxWoodReached)
            {
                _resourceNavigationTarget = _lumberAI.FindLumberStash();
            }
            base.Think();
        }

        protected override void NPCUpdate()
        {
            if (_lumberAI.ShouldNavigateToTree || _lumberAI.MaxWoodReached)
            {
                _npcMovement.NavigateTo(_resourceNavigationTarget.position);
            }
            else if (_lumberAI.ShouldTurnTowardsTree)
            {
                _npcMovement.StopMoving();
                LookAndDo(_resourceNavigationTarget, () =>
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