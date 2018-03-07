using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Necromatic.Character;
using Necromatic.Utility;
using System.Linq;

namespace Necromatic.Player
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private CharacterInstance _character;

        void Update()
        {
            if (Input.GetButton("Attack"))
            {
                _character.AttackNearest();
            }
        }

        void FixedUpdate()
        {
            var direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            if (direction.magnitude != 0)
            {
                _character.Movement.Move(direction);
            }
        }
    }
}