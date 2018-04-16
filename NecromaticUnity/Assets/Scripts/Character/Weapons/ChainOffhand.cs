using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
namespace Necromatic.Character.Weapons
{
    public class ChainOffhand : MonoBehaviour, IOffhand
    {
        [SerializeField] private LineRenderer _chainRenderer;
        [SerializeField] private Transform _chainEnd;
        [SerializeField] private ConfigurableJoint _joint;
        private Rigidbody _chainEndRb;
        private Vector3 _passivePosition;
        private Vector3 _passiveRotation;

        void Awake()
        {
            _chainEndRb = _chainEnd.GetComponent<Rigidbody>();
        }

        void OnDestroy()
        {
            Use(null);
            Destroy(_chainEnd.gameObject);
        }

        private void Update()
        {
            _chainRenderer.SetPosition(0, transform.position);
            _chainRenderer.SetPosition(1, _chainEnd.position);
        }

        public void Use(GameObject target)
        {
            if (target == null)
            {
                _chainEnd.SetParent(_joint.transform);
                _chainEnd.localPosition = _passivePosition;
                _chainEnd.localRotation = Quaternion.Euler(_passiveRotation);
                _joint.linearLimit = new SoftJointLimit() { limit = 0.33f };
                _joint.linearLimitSpring = new SoftJointLimitSpring() { spring = 500 };
                _joint.connectedBody = _chainEndRb;
                _chainEndRb.isKinematic = false;
            }
            else
            {
                _chainEnd.SetParent(null);
                _chainEndRb.isKinematic = true;
                _chainEnd.DOMove(target.transform.position, 0.15f).SetEase(Ease.Linear).OnComplete(() =>
                {
                    _joint.linearLimit = new SoftJointLimit() { limit = 5 };
                    _joint.linearLimitSpring = new SoftJointLimitSpring() { spring = 500 };
                    _joint.connectedAnchor = new Vector3(0, 0, 0);
                    _joint.connectedBody = target.GetComponent<Rigidbody>();
                    _chainEnd.SetParent(target.transform);
                });
            }
        }
    }
}