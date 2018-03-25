using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Necromatic.World;
using UniRx;

namespace Necromatic.Character
{
    public class Movement : MonoBehaviour
    {
        [SerializeField] private Representation _representation;
        [SerializeField] private float _baseSpeed;

        private CharacterInstance _character;
        private bool _canMove = true;

        private Node _currentNavTile;
        private bool _initialized = false;

        public void Init(CharacterInstance c)
        {
            _character = c;
            _character.Death.Dead.Subscribe(dead =>
            {
                if (dead && _currentNavTile != null)
                {
                    _currentNavTile.Taken = false;
                    GameManager.Instance.NavMesh.SetStatus(transform.position, _currentNavTile);
                }
            });
            _initialized = true;
        }

        void Update()
        {
            if (_initialized)
            {
                if (_character.Combat.CurrentState.Value != CombatState.Idle)
                {
                    _canMove = false;
                    if (_character.Combat.LastTarget != null)
                    {
                        _representation.LookDirection((_character.Combat.LastTarget.gameObject.transform.position - transform.position).normalized);
                    }
                }
                else
                {
                    _canMove = true;
                }
            }
        }

        public void Move(Vector3 direction)
        {
            Move(new Vector2(direction.x, direction.z));
        }

        public void Move(Vector2 direction)
        {
            if (_canMove)
            {
                direction.Normalize();
                _representation.LookDirection(direction);
                // check if can move
                var desiredPosition = new Vector3(transform.position.x + direction.x * _baseSpeed * Time.fixedDeltaTime,
                                                transform.position.y,
                                                transform.position.z + direction.y * _baseSpeed * Time.fixedDeltaTime);
                var tilePos = GameManager.Instance.NavMesh.GetNode(desiredPosition);
                if (!tilePos.Taken)
                {
                    if (tilePos != _currentNavTile)
                    {
                        if (_currentNavTile != null)
                        {
                            _currentNavTile.Taken = false;
                            GameManager.Instance.NavMesh.SetStatus(transform.position, _currentNavTile);
                        }
                        _currentNavTile = tilePos;
                    }
                    transform.position = desiredPosition;
                }
                else if (tilePos == _currentNavTile)
                {
                    transform.position = desiredPosition;
                }
                if (_currentNavTile != null) // shouldn't be null here though
                {
                    _currentNavTile.Taken = true;
                }
            }
        }
    }
}