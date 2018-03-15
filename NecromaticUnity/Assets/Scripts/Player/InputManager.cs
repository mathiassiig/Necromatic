using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Necromatic.Character;
using Necromatic.Utility;
using Necromatic.Character.Abilities;
using System.Linq;
using UniRx;
using Necromatic.UI;

namespace Necromatic.Player
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private CharacterInstance _character;
        private GameManager _gameManager;
        private HotBar _hotBar;

        void Awake()
        {
            _gameManager = FindObjectOfType<GameManager>();
            _gameManager.ResearchBank.BankLoaded.Subscribe(loaded =>
            {
                if(loaded)
                {
                    _character.CurrentAbility = _gameManager.ResearchBank.Abilities[0];
                }
            });
            _hotBar = FindObjectOfType<HotBar>();
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

            CheckAbilities();
        }

        void CheckAbilities()
        {
            int maxAllowedOnHotbar = System.Math.Max(_gameManager.ResearchBank.Abilities.Count, 10);
            for (int i = 0; i < maxAllowedOnHotbar; i++)
            {
                if (Input.GetKeyDown((KeyCode)(i+(int)KeyCode.Alpha0)))
                {
                    var index = (int)Mathf.Repeat(i-1, 10);
                    var ability = _gameManager.ResearchBank.Abilities[index];
                    if (ability != null)
                    {
                        _character.CurrentAbility = ability;
                        _hotBar.SwitchTo(index);
                    }
                }
            }
        }
    }
}