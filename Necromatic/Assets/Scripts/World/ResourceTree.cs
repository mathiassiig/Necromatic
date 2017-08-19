using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Necromatic.Char;
namespace Necromatic
{
    public class ResourceTree : Hurtable, IClickable
    {
        [SerializeField] private Rigidbody _log;
        public bool Cut { get; private set; }

        private void Awake()
        {
            Health.Init();
        }

        public void Timber(Vector3 forceDirection)
        {
            Cut = true;
            gameObject.layer = LayerMask.NameToLayer("Default");
            forceDirection *= _log.mass;
            _log.constraints = RigidbodyConstraints.None;
            _log.AddForce(forceDirection, ForceMode.Impulse);
        }

        public void OnClick()
        {
            // TODO: implement later
        }
    }
}