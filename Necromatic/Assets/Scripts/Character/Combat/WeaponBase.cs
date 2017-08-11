using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Necromatic.Character;
using UniRx;
using System;

namespace Necromatic.Character.Combat
{
    public class WeaponBase : MonoBehaviour
    {
        public float Range { get { return _range; } }
        public float Cooldown { get { return _cooldown; } }
        // public float Damage { get { return _damage; } }
        public readonly ReactiveProperty<bool> CanAttack = new ReactiveProperty<bool>();
        private Character _owner;

        [SerializeField] private float _range;
        [SerializeField] private float _cooldown; // cooldown same as attack time?
        [SerializeField] private float _damage;
        [SerializeField] private ProjectileBase _projectile; // if null, melee
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
                ThrowProjectile(c);
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