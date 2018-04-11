using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Necromatic.Character;
using Necromatic.Utility;
using Necromatic.Character.Abilities;
using System.Linq;
using UniRx;
using Necromatic.UI;
using UnityEngine.UI;
using Necromatic.Character.NPC.Strategies.Results;

namespace Necromatic.Player
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private Necromancer _character;
        [Header("Selection")]
        [SerializeField]
        private Image _selectionImage;
        [SerializeField] private Canvas _selectionCanvasPrefab;
        private SquareSelect _squareSelect = new SquareSelect();
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
            _squareSelect.Init(_selectionImage, _selectionCanvasPrefab);
        }

        void Update()
        {
            FetchCommands();
            CheckAbilities();
            SendCommands();
        }

        void FetchCommands()
        {
            if (Input.GetButton("Fire1"))
            {
                _squareSelect.Select(Input.mousePosition);
            }
            if (Input.GetButtonUp("Fire1"))
            {
                _squareSelect.SelectionDone();
            }
            if(Input.GetButton("Fire2"))
            {
                RaycastCommands();
            }
            _doAttack = Input.GetButton("Attack");
            _moveDir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            //_doAbility = Input.GetButtonDown("Fire2");
        }

        void RaycastCommands()
        {
            // todo: check if we hit something interactable
            // check if we should move units
            var movementTarget = GameObjectUtils.GetGroundPosition(Input.mousePosition);
            if(movementTarget != null)
            {
                var sr = new MoveResult(movementTarget.Value, 0.25f);
                sr.Priority = 100;
                foreach(var character in  _squareSelect.SelectedUnits.Value)
                {
                    character.AI.AddTask(sr);
                }
            }

        }

        void SendCommands()
        {
            if (_moveDir.magnitude != 0)
            {
                _character.Movement.MoveDir(_moveDir);
            }
            if (_doAttack)
            {
                _doAttack = false;
                _character.AttackNearest();
            }
            if (_doAbility)
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