using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Necromatic.Utility;
using System.Linq;
using UniRx;
using Necromatic.Items;
using System;
using Necromatic.Char.NPC.TaskHandlers;

namespace Necromatic.Char.NPC
{
    public class UndeadWorkerNPC : UndeadNPC
    {
        [SerializeField]
        private GameObject _resourceWood;
        [SerializeField]
        private GameObject _resourceBag;
        [SerializeField]
        private WoodCuttingHandler _woodCuttingHandler;

        private IDisposable _turningSubscription;

        private Vector3 _resourceNavigationPosition;
        private bool _cutTrees = true;

        void Awake()
        {
            Init();
            _woodCuttingHandler.Init(this);
            Inventory.ItemAdded.Subscribe(value =>
            {
                var hasWood = Inventory.Contains(ItemId.Wood);
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
            if(_cutTrees)
            {
                _woodCuttingHandler.TaskUpdate();
            }
            else if(!_woodCuttingHandler.IsCuttingTree.Value)
            {
                base.NPCUpdate();
            }
        }
    }
}