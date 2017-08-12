using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
namespace Necromatic.Char.Combat
{
    public class Corpse : MonoBehaviour
    {
        private const float _autoDeleteTime = 10f;
        private const float _timeToEnableRessurection = 0.8f;
        private Character _originalCharacter;
        [SerializeField] private Animator _animator;
        [SerializeField] private Gibber _gibber;
        private bool _used = false;
        public void Init(Character originalCharacter)
        {
            transform.rotation = originalCharacter.transform.rotation;
            _animator.SetTrigger("Death");
            _originalCharacter = originalCharacter;
            _originalCharacter.gameObject.SetActive(false);
            gameObject.tag = "Untagged";
            Observable.Timer(TimeSpan.FromSeconds(_timeToEnableRessurection)).First().Subscribe(_ =>
            {
                gameObject.tag = "Corpse";
            });
        }

        // Character is turned undead
        public void Resurrect()
        {
            if(_used)
            {
                return;
            }
            _used = true;
            if (_gibber)
            {
                _gibber.Gib();
            }
            var undeadType = UndeathConverter.LivingToDead[_originalCharacter.Type];
            var undeadInstance = MasterPoolManager.Instance.GetCharacter(undeadType);
            undeadInstance.transform.rotation = _originalCharacter.transform.rotation;
            undeadInstance.gameObject.transform.position = _gibber.transform.position;
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
            if (_used)
            {
                return;
            }
            _used = true;
            _originalCharacter.gameObject.SetActive(true);
            _originalCharacter.Health.Set(_originalCharacter.Health.Max.Value);
            _originalCharacter.IsDead.Value = false;
            Destroy(gameObject); // todo: pooling
        }
    }
}