﻿using System.Collections;
using System.Collections.Generic;
using Necromatic.Character.Inventory;
using Necromatic.Character.NPC.Strategies;
using UnityEngine;
namespace Necromatic.Character.NPC
{
    public class SkeletonCharacter : CharacterInstance, ISelectable
    {
        [SerializeField] private Canvas _selectionCanvas;

        InventoryInstance ISelectable.Inventory => Inventory;

        public void Deselect()
        {
            _selectionCanvas.gameObject.SetActive(false);
        }

        public void Select()
        {
            _selectionCanvas.gameObject.SetActive(true);
        }

        protected override void Init()
        {
            var combat = new Combat(this, 10, 0.30f, 0.7f, 1.5f);
            Combat = combat;
            _death = new Death();
            base.Init();
        }
    }
}