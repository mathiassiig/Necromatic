using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Necromatic.Character;
using Necromatic.Utility;
using Necromatic.Character.NPC;

namespace Necromatic.World
{
    public class Grave : MonoBehaviour, IRaisable
    {
		[SerializeField] private Transform _graveEarth;
		private bool _raised = false;
		private CharacterType _undeadToRaise = CharacterType.Skeleton;
        public void Raise()
        {
			if(_raised)
			{
				return;
			}
			_raised = true;
            var motherpool = FindObjectOfType<MotherPool>();
            var undeadPrefab = motherpool.GetCharacterPrefab(_undeadToRaise);
            var undead = Object.Instantiate(undeadPrefab, _graveEarth.transform.position, _graveEarth.transform.rotation);
            var ai = undead.GetComponent<ArtificialIntelligence>();
            ai.SetBrainState(false);
            var player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterInstance>();
            undead.Representation.ReviveAnimation(() =>
            {
                ai.SetBrainState(true);
                undead.Representation.LookDirectionAnim(MathUtils.PlaneDirection(undead.transform, player.transform), 0.3f);
            });
            Destroy(_graveEarth.gameObject);
        }

        void Start()
        {

        }
    }
}