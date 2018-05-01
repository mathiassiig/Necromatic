using System.Collections;
using System.Collections.Generic;
using Necromatic.Character.Inventory;
using Necromatic.Character.NPC.Strategies;
using UnityEngine;
namespace Necromatic.Character.NPC
{
    public class SkeletonCharacter : CharacterInstance, ISelectable
    {
        [SerializeField] private Canvas _selectionCanvas;

        CharacterInventory ISelectable.Inventory => Inventory;

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
            _death = new Death();
            var combat = new Combat(this);
            Combat = combat;
            base.Init();
        }
    }
}