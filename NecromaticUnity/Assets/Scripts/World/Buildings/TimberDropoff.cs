using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Necromatic.World.Buildings
{
    public class TimberDropoff : MonoBehaviour
    {
        private List<Transform> _logs = new List<Transform>();
        private float _xBase = -0.1f;
        private float _yBase = 5f;
        private float _zBase = 0.4f;
        private float _yOffset = 7f;
        private float _zOffset = -0.2f;

        private Vector3 _rotation = new Vector3(0, 90, 0);
        public void Dropoff(Transform log)
        {
            var rb = log.GetComponent<Rigidbody>();
            rb.isKinematic = true;

            log.localRotation = Quaternion.Euler(_rotation);
            log.SetParent(transform);
            var index = _logs.Count;
            var x = _xBase;
            var y = _yBase;
            var z = _zBase + index * _zOffset;
            log.localPosition = new Vector3(x, y, z);
            _logs.Add(log);
        }
    }
}