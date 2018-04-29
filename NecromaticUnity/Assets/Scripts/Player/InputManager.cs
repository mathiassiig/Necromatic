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
using Necromatic.World;
using Necromatic.Character.NPC.Strategies;

namespace Necromatic.Player
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private Necromancer _character;
        [SerializeField] private InventoryUI _inventoryUI;

        [Header("Selection")]
        [SerializeField] private Image _selectionImage;
        [SerializeField] private Canvas _selectionCanvasPrefab;
        private SquareSelect _squareSelect = new SquareSelect();
        private GameManager _gameManager;
        private HotBar _hotBar;
        private Vector2 _moveDir;
        private bool _doAbility = false;
        private bool _doAttack = false;
        private bool _inventoryOpen => _inventoryUI.gameObject.activeInHierarchy;

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
            if (!_inventoryOpen)
            {
                if (Input.GetButton("Fire1"))
                {
                    _squareSelect.Select(Input.mousePosition);
                }
                if (Input.GetButtonUp("Fire1"))
                {
                    _squareSelect.SelectionDone();
                }
                if (Input.GetButtonDown("Fire2"))
                {
                    RaycastCommands();
                }
            }
            if (Input.GetKeyDown(KeyCode.I))
            {
                if (_inventoryOpen)
                {
                    _inventoryUI.gameObject.SetActive(false);
                }
                else
                {
                    _inventoryUI.gameObject.SetActive(true);
                    var selectedCharacter = _squareSelect.SelectedUnits.Value.FirstOrDefault();
                    if (selectedCharacter != null)
                    {
                        _inventoryUI.Populate(selectedCharacter.Inventory);
                    }
                }
            }
            _doAttack = Input.GetButton("Attack");
            _moveDir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        }

        void ToggleInventory()
        {

        }

        void RaycastCommands()
        {
            var mask = LayerMask.GetMask(NecromaticLayers.TREE, NecromaticLayers.CHARACTER, NecromaticLayers.CORPSE);
            var clickable = GameObjectUtils.RayGetComponent<IClickReceiver>(Input.mousePosition, mask);
            if (clickable != null)
            {
                clickable.Click(_squareSelect.SelectedUnits.Value);
            }
            else
            {
                var movementTarget = GameObjectUtils.GetGroundPosition(Input.mousePosition);
                if (movementTarget != null)
                {
                    var sr = new MoveResult(movementTarget.Value, 0.25f);
                    sr.Priority = 100;
                    if(_squareSelect.SelectedUnits.Value != null && _squareSelect.SelectedUnits.Value.Count > 0)
                    {
                        foreach (var character in _squareSelect.SelectedUnits.Value)
                        {
                            character.AI.SetPrimaryStrategy(new SearchForEnemies());
                            character.AI.AddTask(sr);
                        }
                    }
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
                _character.Combat.TryAttackNearest(_character);
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