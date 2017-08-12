using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Necromatic.Char;
using UniRx;
using System;

namespace Necromatic.Char.Combat
{
    public class WeaponBase : MonoBehaviour
    {
        public float Range { get { return _range; } }
        public float Cooldown { get { return _cooldown; } }
        // public float Damage { get { return _damage; } }
        public readonly ReactiveProperty<bool> CanAttack = new ReactiveProperty<bool>();
        private Character _owner;
        public bool IsMelee { get { return _projectile == null; } }

        [SerializeField] private float _range;
        [SerializeField] private float _cooldown; // cooldown same as attack time?
        [SerializeField] private float _damage;
        [SerializeField] private ProjectileBase _projectile; 
        [SerializeField] private Vector3 _projectileSpawnOffset;
        void Awake()
        {
            CanAttack.Value = true;
            _owner = GetComponentInParent<Character>();
        }

        public void Attack(Character c)
        {
            if (CanAttack.Value)
            {
                CanAttack.Value = false;
                if (_projectile != null)
                {
                    ThrowProjectile(c);
                }
                else
                {
                    c.Health.Add(-_damage);
                }
                Observable.Timer(TimeSpan.FromSeconds(_cooldown)).First().Subscribe(_ => CanAttack.Value = true);
            }
        }

        private void ThrowProjectile(Character c)
        {
            var projectile = Instantiate(_projectile);
            var offset = Quaternion.AngleAxis(_owner.transform.rotation.eulerAngles.y, Vector3.up) * _projectileSpawnOffset;
            projectile.transform.position = _owner.transform.position + offset;
            projectile.transform.rotation = _owner.transform.rotation;
            projectile.Init(c, _damage);
        }
    }
}