using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Necromatic.UI
{
    public class FollowCamera : MonoBehaviour
    {
		[SerializeField] private Transform _target;
		[SerializeField] private Vector3 _offset;

        void Update()
        {
			transform.LookAt(_target);
			transform.position = _target.position + _offset;
        }
    }
}