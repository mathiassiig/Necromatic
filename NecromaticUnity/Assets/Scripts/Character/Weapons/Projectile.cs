using Necromatic.Character.Inventory;
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
        private CharacterInstance _sender;
        private RangedWeapon _weaponData;

        public void Fire(Transform target, Vector3 offset, RangedWeapon weaponData, CharacterInstance sender)
        {
            _weaponData = weaponData;
            _sender = sender;
            if (_rb != null)
            {
                _fired = true;
                transform.SetParent(null);
                _rb.isKinematic = false;
                var dis = MathUtils.Distance(transform.position, target.position + offset);
                var time = 0.075f + dis / 36f;
                var velocity = MathUtils.CalculateBestThrowSpeed(transform.position, target.position + offset, time);
                _rb.velocity = velocity;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            var closestpos = collision.collider.ClosestPointOnBounds(transform.position);
            var normal = collision.contacts[0].normal;
            var randomness = 4f;
            gameObject.transform.rotation = Quaternion.Euler(normal) * 
                Quaternion.Euler(Random.Range(-randomness, randomness), Random.Range(-randomness, randomness), Random.Range(-randomness, randomness));
            Destroy(gameObject.GetComponent<Collider>());
            gameObject.transform.position = gameObject.transform.position - gameObject.transform.forward * 0.15f;
            gameObject.transform.SetParent(collision.gameObject.transform);
            _rb.isKinematic = true;
            var bodyPart = collision.collider.gameObject.GetComponent<BodyPart>();
            if(bodyPart != null)
            {
                bodyPart.Owner.Combat.ReceiveAttack(_weaponData.BaseDamage, _sender);
            }
        }
    }
}