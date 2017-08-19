using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Necromatic.Char;
using UniRx;
namespace Necromatic
{
    public class ResourceTree : Hurtable, IClickable
    {
        public bool Cut { get; private set; }
        [SerializeField] private GameObject _root;
        [SerializeField] private ConfigurableJoint _joint;
        [SerializeField] private Transform _workerPosition;

        public Transform WorkerPosition => _workerPosition;

        private void Awake()
        {
            Health.Init();
        }

        public void Timber(Vector3 forceDirection)
        {
            var log = gameObject.GetComponent<Rigidbody>();
            var parent = transform.parent;
            _root.transform.SetParent(parent);
            _joint.connectedBody = null;
            log.transform.SetParent(null);
            Cut = true;
            gameObject.layer = LayerMask.NameToLayer("Default");
            // find another way of doing this
            Observable.Timer(TimeSpan.FromSeconds(3f)).TakeUntilDestroy(this).Subscribe(_ =>
            {
                gameObject.layer = LayerMask.NameToLayer("NoCharCollision");
            });
            log.velocity = Vector3.zero;
            forceDirection *= log.mass * 3f;
            forceDirection = forceDirection + Vector3.up * log.mass * 0.5f;
            log.constraints = RigidbodyConstraints.None;
            log.AddForce(forceDirection, ForceMode.Impulse);
        }

        public void OnClick()
        {
            // TODO: implement later
        }
    }
}