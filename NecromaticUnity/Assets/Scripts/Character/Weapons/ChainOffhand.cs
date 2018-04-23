using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Necromatic.Character.Inventory;
using UnityEngine;
namespace Necromatic.Character.Weapons
{
    public class ChainOffhand : MonoBehaviour, IOffhand, IItemInstance
    {
        [SerializeField] private LineRenderer _chainRenderer;
        [SerializeField] private Transform _chainEnd;
        [SerializeField] private ConfigurableJoint _joint;
        private Rigidbody _chainEndRb;
        private Vector3 _passivePosition;
        private Vector3 _passiveRotation;
        private CharacterInstance _sender;
        private SpeedModifier _speedModifier;
        private const float _slownessFactor = 20;

        void Awake()
        {
            _chainEndRb = _chainEnd.GetComponent<Rigidbody>();
        }

        public void Init(CharacterInstance sender)
        {

        }

        void OnDestroy()
        {
            Use(null, null);
            Destroy(_chainEnd.gameObject);
        }

        private void Update()
        {
            _chainRenderer.SetPosition(0, transform.position);
            _chainRenderer.SetPosition(1, _chainEnd.position);
        }

        public void Use(GameObject target, CharacterInstance sender)
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
                if(_speedModifier != null)
                {
                    sender.Movement.RemoveModifier(_speedModifier);
                    _speedModifier = null;
                }
            }
            else
            {
                _chainEnd.SetParent(null);
                _chainEndRb.isKinematic = true;
                _chainEnd.LookAt(target.transform);
                _chainEnd.localRotation = _chainEnd.localRotation * Quaternion.Euler(0, -90, 180);
                var targetCollider = target.GetComponent<Collider>();
                var targetRb = target.GetComponent<Rigidbody>();
                if(targetRb != null)
                {
                    _speedModifier = new SpeedModifier() { Modifier = -targetRb.mass / _slownessFactor };
                    sender.Movement.ModifySpeed(_speedModifier);
                }
                _chainEnd.DOMoveX(target.transform.position.x, 0.15f).SetEase(Ease.Linear);
                _chainEnd.DOMoveY(target.transform.position.y, 0.15f).SetEase(Ease.InBack);
                _chainEnd.DOMoveZ(target.transform.position.z, 0.15f).SetEase(Ease.Linear).OnUpdate(() =>
                {
                    if(targetCollider != null)
                    {
                        if(targetCollider.bounds.Contains(_chainEnd.position))
                        {   
                            
                            _chainEnd.DOComplete();
                        }
                    }
                })
                .OnComplete(() =>                
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