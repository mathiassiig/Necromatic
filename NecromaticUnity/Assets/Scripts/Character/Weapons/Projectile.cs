﻿using Necromatic.Character.Inventory;
using Necromatic.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Necromatic.Character.Weapons
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private LayerMask _collisionMask;
        [SerializeField] private Rigidbody _rb;
        private bool _fired;
        private bool _hit;
        private CharacterInstance _sender;
        private RangedWeapon _weaponData;

        public void Fire(Transform target, Vector3 offset, RangedWeapon weaponData, CharacterInstance sender, float drift = 0)
        {
            _weaponData = weaponData;
            _sender = sender;
            var collider = GetComponent<Collider>();
            if(collider != null)
            {
                collider.enabled = true;
            }
            if (_rb != null)
            {
                _fired = true;
                transform.SetParent(null);
                _rb.isKinematic = false;
                var dis = MathUtils.Distance(transform.position, target.position + offset);
                var time = 0.075f + dis / 36f;
                var velocity = MathUtils.CalculateBestThrowSpeed(transform.position, target.position + offset, time);
                velocity = Quaternion.AngleAxis(Random.Range(-drift, drift), Vector3.forward) *
                    Quaternion.AngleAxis(Random.Range(-drift, drift), Vector3.left) *
                    Quaternion.AngleAxis(Random.Range(-drift, drift), Vector3.up) * velocity;
                _rb.velocity = velocity;
            }
        }

        private void Update()
        {
            if(_fired && !_hit)
            {
                transform.rotation = Quaternion.LookRotation(-1*_rb.velocity);
            }
        }

        private void OnTriggerEnter(Collider collider)
        {
            _hit = true;
            Destroy(gameObject.GetComponent<Collider>());
            gameObject.transform.SetParent(collider.gameObject.transform, true);
            var bodyPart = collider.gameObject.GetComponent<BodyPart>();
            if(bodyPart != null)
            {
                if(bodyPart.Owner.Death.Dead.Value == false)
                {
                    bodyPart.Owner.Combat.ReceiveAttack(_weaponData.BaseDamage, _sender);
                }
                if (bodyPart.Owner.Death.Dead.Value == true)
                {
                    bodyPart.Owner.GetComponent<Ragdollifier>()?.Ragdollify(bodyPart.transform, _rb);
                }
            }
            _rb.isKinematic = true;
        }
    }
}