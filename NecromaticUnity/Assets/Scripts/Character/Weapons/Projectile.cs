using Necromatic.Character.Inventory;
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

        public void Fire(Vector3 direction, RangedWeapon weaponData, CharacterInstance sender)
        {
            if (_rb != null)
            {
                _fired = true;
                transform.SetParent(null);
                _rb.isKinematic = false;
                _rb.AddForce(direction * weaponData.ProjectileForce);
            }
        }

        private void FixedUpdate()
        {
            if (_fired)
            {
                /*RaycastHit hit;
                Debug.DrawLine(transform.position, _rb.velocity.normalized, Color.red, Time.fixedDeltaTime);
                if (Physics.Raycast(transform.position, _rb.velocity.normalized, out hit, 1f, _collisionMask))
                {
                    Debug.Log(hit.collider.gameObject.name);
                    Destroy(gameObject);
                }*/
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
        }
    }
}