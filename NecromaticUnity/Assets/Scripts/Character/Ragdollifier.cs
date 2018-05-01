using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;

namespace Necromatic.Character
{
    public class BipRotation
    {
        public string Name;
        public Quaternion Rotation;
        public BipRotation(string n, Quaternion r)
        {
            Name = n;
            Rotation = r;
        }
    }
    public class Ragdollifier : MonoBehaviour
    {
        public static string PELVIS = "Bip001 Pelvis";
        public static string SPINE = "Bip001 Spine";
        public static string LEFT_HAND = "Bip001 L Hand";
        public static string RIGHT_HAND = "Bip001 R Hand";
        [SerializeField] private Animator _animator;
        [SerializeField] private List<Transform> _ragdollParts;

        public Vector3 PelvisPosition
        {
            get
            {
                return _ragdollParts.FirstOrDefault(x => x.gameObject.name == PELVIS).transform.position;
            }
            set
            {
                _ragdollParts.FirstOrDefault(x => x.gameObject.name == PELVIS).transform.position = value;
            }
        }

        public void WakeToLife()
        {
            _animator.SetTrigger("Stand");
            _animator.enabled = true;
        }

        public void LiftToLife()
        {
            var spine = _ragdollParts.FirstOrDefault(x => x.transform.name == SPINE);
            var pelvis = _ragdollParts.FirstOrDefault(x => x.transform.name == PELVIS);
            var leftHand = spine.FindDeepChild(LEFT_HAND);
            var rightHand = spine.FindDeepChild(RIGHT_HAND);
            DetachFromHand(leftHand);
            DetachFromHand(rightHand);
            foreach (var part in _ragdollParts)
            {
                if (part != spine && part != pelvis)
                {
                    part.GetComponent<Rigidbody>().isKinematic = false;
                }
                part.GetComponent<Collider>().enabled = false;
            }
            pelvis.DOLocalMove(pelvis.localPosition + new Vector3(0, 0, 1), 2f);
            pelvis.DOLocalRotate(new Vector3(0, 90, 0), 2f);
        }

        private void DetachFromHand(Transform hand)
        {
            var device = hand.GetChild(0);
            if(device != null)
            {
                device.SetParent(null);
                var rb = device.GetComponent<Rigidbody>();
                if(rb != null)
                {
                    rb.isKinematic = false;
                }
            }
        }

        public List<BipRotation> GetRagdollPosition()
        {
            var positions = new List<BipRotation>();
            foreach (var part in _ragdollParts)
            {
                positions.Add(new BipRotation(part.gameObject.name, part.transform.localRotation));
            }
            return positions;
        }

        public void ApplyRagdollPosition(List<BipRotation> positions)
        {
            _animator.enabled = false;
            foreach (var part in positions)
            {
                var rdp = _ragdollParts.FirstOrDefault(x => x.gameObject.name == part.Name);
                if (rdp != null)
                {
                    rdp.transform.localRotation = part.Rotation;
                }
            }
        }

        public void Ragdollify()
        {
            _animator.enabled = false;
            DropHands();
            foreach (var v in _ragdollParts)
            {
                v.GetComponent<Rigidbody>().isKinematic = false;
            }
        }

        private void DropHands()
        {
            var charr = GetComponent<CharacterInstance>();
            if (charr != null)
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
            if (rb != null)
            {
                rb.velocity = destroyer.velocity;
            }
        }
    }
}