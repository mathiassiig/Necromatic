using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Necromatic.Character
{
    public class Ragdollifier : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private List<Rigidbody> _ragdollParts;

        public void Ragdollify()
        {
            _animator.enabled = false;
            foreach(var v in _ragdollParts)
            {
                v.isKinematic = false;
            }
        }

        public void Ragdollify(Transform t, Rigidbody destroyer)
        {
            Ragdollify();
            var rb = t.GetComponent<Rigidbody>();
            if(rb != null)
            {
                rb.velocity = destroyer.velocity;
            }
        }
    }
}