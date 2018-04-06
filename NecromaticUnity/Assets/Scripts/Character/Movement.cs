using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Necromatic.World;
using UniRx;
using UnityEngine.AI;
using System.Linq;

namespace Necromatic.Character
{
    public class Movement : MonoBehaviour
    {
        [SerializeField] private Representation _representation;
        [SerializeField] private NavMeshAgent _agent;

        private CharacterInstance _character;
        private bool _canMove = true;

        private bool _initialized = false;

        public void Init(CharacterInstance c)
        {
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
            _agent.destination = transform.position + new Vector3(dir.x, 0, dir.y).normalized;
            _representation.LookDirection(dir);
        }

        public void Move(Vector3 to)
        {
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
                    _canMove = false;
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
                    _canMove = true;
                }
            }
        }
    }
}