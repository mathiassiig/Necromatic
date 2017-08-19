using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Necromatic.Char;
using UniRx;
using System;
using Necromatic.Managers;
namespace Necromatic.Char.Combat
{
    public class WeaponBase : MonoBehaviour
    {
        public float Range => _range; 
        public float Speed => _speed;
        // public float Damage { get { return _damage; } }
        public readonly ReactiveProperty<bool> CanAttack = new ReactiveProperty<bool>();
        public bool IsMelee => _projectile == null;
        private Character _owner;
        private int _currentAttackSoundIndex = 0;

        [SerializeField] private List<SoundEffect> _attackSounds;
        [SerializeField] private AudioSource _attackAudio;
        [SerializeField] private float _range = 2f;
        [SerializeField] private float _speed = 0.24f;
        [SerializeField] private float _damage = 0f;
        [SerializeField] private ProjectileBase _projectile; 
        [SerializeField] private Vector3 _projectileSpawnOffset;
        void Awake()
        {
            CanAttack.Value = true;
            _owner = GetComponentInParent<Character>();
        }

        public void PlaySound()
        {
            var clip = SoundManager.Instance.GetClip(_attackSounds[_currentAttackSoundIndex]);
            _currentAttackSoundIndex = (int)Mathf.Repeat(_currentAttackSoundIndex + 1, _attackSounds.Count - 1);
            _attackAudio.PlayOneShot(clip);
        }

        public void Attack(Hurtable c, Character sender)
        {
            if (CanAttack.Value)
            {
                CanAttack.Value = false;
                if (_projectile != null)
                {
                    ThrowProjectile(c, sender);
                }
                else
                {
                    c.Health.Add(-_damage, sender);
                }
                Observable.Timer(TimeSpan.FromSeconds(_speed)).First().Subscribe(_ => CanAttack.Value = true);
            }
        }

        private void ThrowProjectile(Hurtable c, Character sender)
        {
            var projectile = Instantiate(_projectile);
            var offset = Quaternion.AngleAxis(_owner.transform.rotation.eulerAngles.y, Vector3.up) * _projectileSpawnOffset;
            projectile.transform.position = _owner.transform.position + offset;
            projectile.transform.rotation = _owner.transform.rotation;
            projectile.Init(c, _damage, () => c.Health.Add(-_damage, sender));
        }
    }
}