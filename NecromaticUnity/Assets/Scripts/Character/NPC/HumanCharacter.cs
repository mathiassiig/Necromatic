using System.Collections;
using System.Collections.Generic;
using Necromatic.Utility;
using UnityEngine;
namespace Necromatic.Character.NPC
{
    public class HumanCharacter : CharacterInstance, IRaisable
    {
        protected CharacterType _undeadToRaise;
        
        public void Raise()
        {
            if(!Death.Dead.Value)
            {
                return;
            }
            var motherpool = FindObjectOfType<MotherPool>();
            var undeadPrefab = motherpool.GetCharacterPrefab(_undeadToRaise);
            var undead = Object.Instantiate(undeadPrefab, transform.position, transform.rotation);
            var ai = undead.GetComponent<ArtificialIntelligence>();
            ai.SetBrainState(false);
            var player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterInstance>();
            undead.Representation.ReviveAnimation(() =>
            {
                ai.SetBrainState(true);
                undead.Representation.LookDirectionAnim(MathUtils.PlaneDirection(undead.transform, player.transform), 0.3f);
            });
            Destroy(gameObject);
        }

        protected override void Init()
        {
            var combat = new Combat(this, 10, 0.3f, 0.7f, 1.5f);
            Combat = combat;
            _death = new HumanDeath();
            _undeadToRaise = CharacterType.Skeleton;
            base.Init();
        }
    }
}