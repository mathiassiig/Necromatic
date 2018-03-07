using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Necromatic.Character
{
    public class Movement : MonoBehaviour
    {
        [SerializeField] private Representation _representation;
        [SerializeField] private float _baseSpeed;

        private Combat _combat;
        private bool _canMove = true;

        public void Init(Combat c)
        {
            _combat = c;
        }

        void Update()
        {
            if (_combat.CurrentState.Value != CombatState.Idle)
            {
                _canMove = false;
                if(_combat.LastTarget != null)
                {
                    _representation.LookDirection((_combat.LastTarget.transform.position - transform.position).normalized);
                }
            }
            else
            {
                _canMove = true;
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
                transform.position = new Vector3(transform.position.x + direction.x * _baseSpeed * Time.deltaTime,
                                                transform.position.y,
                                                transform.position.z + direction.y * _baseSpeed * Time.deltaTime);
            }
        }
    }
}