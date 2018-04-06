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
        [SerializeField] private Necromancer _character;
        private GameManager _gameManager;
        private HotBar _hotBar;
        private Vector2 _moveDir;
        private bool _doAbility = false;
        private bool _doAttack = false;

        void Awake()
        {
            _gameManager = FindObjectOfType<GameManager>();
            _gameManager.ResearchBank.BankLoaded.Subscribe(loaded =>
            {
                if (loaded)
                {
                    _character.CurrentAbility = _gameManager.ResearchBank.Abilities[0];
                }
            });
            _hotBar = FindObjectOfType<HotBar>();
        }

        void Update()
        {
            FetchCommands();
            CheckAbilities();
            SendCommands();
        }

        void FixedUpdate()
        {
            
        }

        void FetchCommands()
        {
            _doAttack = Input.GetButton("Attack");
            _moveDir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            _doAbility = Input.GetButtonDown("Fire1");

        }

        void SendCommands()
        {
            if (_moveDir.magnitude != 0)
            {
                _character.Movement.MoveDir(_moveDir);
            }
            if(_doAttack)
            {
                _doAttack = false;
                _character.AttackNearest();
            }
            if(_doAbility)
            {
                _doAbility = false;
                _character.DoAbility();
            }
            _character.DoHoverAbility();
        }

        void CheckAbilities()
        {
            int maxAllowedOnHotbar = System.Math.Max(_gameManager.ResearchBank.Abilities.Count, 10);
            for (int i = 0; i < maxAllowedOnHotbar; i++)
            {
                if (Input.GetKeyDown((KeyCode)(i + (int)KeyCode.Alpha0)))
                {
                    var index = (int)Mathf.Repeat(i - 1, 10);
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