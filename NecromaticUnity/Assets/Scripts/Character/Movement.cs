using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Necromatic.Character
{
    public class Movement : MonoBehaviour
    {
		[SerializeField] private Representation _representation;
        [SerializeField] private float _baseSpeed;
        // Use this for initialization
        void Start()
        {
			
        }

		void FixedUpdate()
		{
			//var x = Mathf.Sin(Time.time);
			//var z = Mathf.Cos(Time.time);
			//var dir = new Vector2(x, z);
			//Move(dir);
		}

        public void Move(Vector2 direction)
        {
            direction.Normalize();
			_representation.LookDirection(direction);
            // check if can move
            transform.position = new Vector3(transform.position.x + direction.x * _baseSpeed * Time.fixedDeltaTime,
                                            transform.position.y,
                                            transform.position.z + direction.y * _baseSpeed * Time.fixedDeltaTime);
        }
    }
}