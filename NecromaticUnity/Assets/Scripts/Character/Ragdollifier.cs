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
            DropHands();
            foreach (var v in _ragdollParts)
            {
                v.isKinematic = false;
            }
        }

        private void DropHands()
        {
            var charr = GetComponent<CharacterInstance>();
            if(charr != null)
            {
                Drop(charr.Inventory.WeaponSlot.Value.GameObjectInstance);
                Drop(charr.Inventory.OffhandSlot.Value.GameObjectInstance);

            }
        }

        private void Drop(GameObject g)
        {
            if (g != null)
            {
                var rb = g.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = false;
                }
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