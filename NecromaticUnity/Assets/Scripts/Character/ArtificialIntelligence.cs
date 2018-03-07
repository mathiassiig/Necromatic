using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Necromatic.Utility;
using System.Linq;

namespace Necromatic.Character
{
    public class ArtificialIntelligence : MonoBehaviour
    {
        [SerializeField] private CharacterInstance _character;
		[SerializeField] private float _searchRange = 10;
        
		void Update()
        {
            SearchAndDestroy();
        }

		void SearchAndDestroy()
		{
			var enemies = GameObjectUtils.DetectEnemies(_searchRange, _character.transform.position, _character);
			if(enemies != null && enemies.Count != 0)
			{
				var inRange = enemies.FirstOrDefault(x => GameObjectUtils.Distance(x.transform.position, _character.transform.position) <= _character.AttackRange);
				if(inRange != null)
				{
					_character.AttackNearest();
				}
				else
				{
					var nearest = GameObjectUtils.Closest<CharacterInstance>(enemies, _character);
					MoveTo(nearest.transform);
				}
			}

		}

		void MoveTo(Transform t)
		{

			var dir = (t.position - _character.transform.position).normalized;
			//Debug.Log(dir + " - " + t.name);
			_character.Movement.Move(dir);
		}
    }
}