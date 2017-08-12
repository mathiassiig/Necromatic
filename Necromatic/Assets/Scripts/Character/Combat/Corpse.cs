using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
namespace Necromatic.Character.Combat
{
    public class Corpse : MonoBehaviour
    {
        private const float _autoDeleteTime = 10f;
        private Character _originalCharacter;
        [SerializeField] private Animator _animator;
        [SerializeField] private Gibber _gibber;
        public void Init(Character originalCharacter)
        {
            transform.rotation = originalCharacter.transform.rotation;
            _animator.SetTrigger("Death");
            _originalCharacter = originalCharacter;
            _originalCharacter.gameObject.SetActive(false);
            Observable.Timer(TimeSpan.FromSeconds(3)).First().Subscribe(_ =>
            {
                Resurrect();
            });
        }

        // Character is turned undead
        public void Resurrect()
        {
            if(_gibber)
            {
                _gibber.Gib();
            }
            var undeadType = UndeathConverter.LivingToDead[_originalCharacter.Type];
            var undeadInstance = MasterPoolManager.Instance.GetCharacter(undeadType);
            undeadInstance.transform.rotation = _originalCharacter.transform.rotation;
            undeadInstance.gameObject.transform.position = transform.position;
            Destroy(_originalCharacter.gameObject); // todo: pooling
            Destroy(_animator.gameObject);
            Observable.Timer(TimeSpan.FromSeconds(_autoDeleteTime)).First().Subscribe(_ =>
            {
                Destroy(gameObject);
            });
        }

        // Character comes back as whatever it was
        public void Revive()
        {
            _originalCharacter.gameObject.SetActive(true);
            _originalCharacter.Health.Set(_originalCharacter.Health.Max.Value);
            _originalCharacter.IsDead.Value = false;
            Destroy(gameObject); // todo: pooling
        }
    }
}