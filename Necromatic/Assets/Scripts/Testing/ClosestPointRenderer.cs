using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Necromatic.Utility;
namespace Necromatic.Testing
{
    public class ClosestPointRenderer : MonoBehaviour
    {
        public CircleRenderer _target;

        void Start()
        {
            var newPosition = _target.Subscribe(gameObject, transform.position);
            transform.position = newPosition;
        }

        void OnDrawGizmos()
        {

        }
    }
}