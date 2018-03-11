using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Necromatic.Character;
using Necromatic.Utility;
using Necromatic.Character.Abilities;
using System.Linq;

namespace Necromatic.Player
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private CharacterInstance _character;

        void Start()
        {
            _character.CurrentAbility = new RaiseCorpse();
        }

        void Update()
        {
            // attacking
            if (Input.GetButton("Attack"))
            {
                _character.AttackNearest();
            }

            // moving
            var direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            if (direction.magnitude != 0)
            {
                _character.Movement.Move(direction);
            }

            // spells
            if (Input.GetButtonDown("Fire1"))
            {
                _character.DoAbility();
            }
        }
    }
}