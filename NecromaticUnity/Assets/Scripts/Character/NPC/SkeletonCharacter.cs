using System.Collections;
using System.Collections.Generic;
using Necromatic.Character.NPC.Strategies;
using UnityEngine;
namespace Necromatic.Character.NPC
{
    public class SkeletonCharacter : CharacterInstance, ISelectable
    {
        [SerializeField] private Canvas _selectionCanvas;

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
            var combat = new Combat(this, 10, 0.3f, 0.2f, 1);
            Combat = combat;
            _death = new Death();
            var player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterInstance>();
            var ai = GetComponent<ArtificialIntelligence>();
            ai.AddPrimaryStrategy(new ProtectTargetStrategy(player.transform));
            base.Init();
        }
    }
}