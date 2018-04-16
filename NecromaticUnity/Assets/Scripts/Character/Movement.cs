using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Necromatic.World;
using UniRx;
using UnityEngine.AI;
using System.Linq;

namespace Necromatic.Character
{
    public class SpeedModifier
    {
        public float Modifier;
    }

    public class Movement : MonoBehaviour
    {
        [SerializeField] private Representation _representation;
        [SerializeField] private NavMeshAgent _agent;

        private CharacterInstance _character;
        public bool CanMove = true;

        private bool _initialized = false;

        private float _currentSpeed = 0;
        private float _baseSpeed = 3.5f;
        public List<SpeedModifier> SpeedBonuses = new List<SpeedModifier>();
        private Vector3? _lastPosition;

        public void ModifySpeed(SpeedModifier modifier)
        {
            SpeedBonuses.Add(modifier);
            CalculateNewSpeed();
        }

        public void RemoveModifier(SpeedModifier modifier)
        {
            SpeedBonuses.Remove(modifier);
            CalculateNewSpeed();
        }

        private void CalculateNewSpeed()
        {
            _agent.speed = _baseSpeed + SpeedBonuses.Select(x => x.Modifier).Sum();
        }

        public void Init(CharacterInstance c)
        {
            _baseSpeed = _agent.speed;
            _agent.updateRotation = false;
            _character = c;
            _character.Death.Dead.Subscribe(dead =>
            {
                if (dead)
                {
                    Destroy(_agent);
                }
            });
            _initialized = true;
        }

        public void MoveDir(Vector2 dir)
        {
            if (!_agent.enabled)
            {
                return;
            }
            _agent.destination = transform.position + new Vector3(dir.x, 0, dir.y).normalized;
            _representation.LookDirection(dir);
        }

        public void Move(Vector3 to)
        {
            if(!_agent.enabled)
            {
                return;
            }
            _agent.destination = to;
            try
            {
                var next = _agent.path.corners[1];
                var dir = (next - transform.position).normalized;
                _representation.LookDirection(new Vector2(dir.x, dir.z).normalized);
            }
            catch
            {

            }
        }

        void Update()
        {
            if (_character == null || _representation == null)
            {
                return;
            }
            if (_initialized)
            {
                if (_character.Combat.CurrentState.Value != CombatState.Idle)
                {
                    CanMove = false;
                    if (_character.Combat.LastTarget != null)
                    {
                        try
                        {
                            _representation.LookDirection((_character.Combat.LastTarget.gameObject.transform.position - transform.position).normalized);
                        }
                        catch
                        {
                            // so apparently I cannot null check for Character.Combat.LastTarget.gameObject
                            // so I just have to try catch
                        }
                    }
                }
                else
                {
                    CanMove = true;
                }
            }
            if(_lastPosition.HasValue)
            {
                _currentSpeed = (transform.position - _lastPosition.Value).magnitude / Time.deltaTime;
                _representation.Move(_currentSpeed);
            }
            _lastPosition = transform.position;
            _agent.enabled = CanMove;
        }
    }
}