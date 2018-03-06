using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Necromatic.Character;

namespace Necromatic.Player
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private Movement _movement;

        void FixedUpdate()
        {
			var direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
			if(direction.magnitude != 0)
			{
				_movement.Move(direction);
			}
        }
    }
}