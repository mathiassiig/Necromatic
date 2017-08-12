using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
namespace Necromatic.Character.Combat
{
    public class Corpse : MonoBehaviour
    {
        private Character _originalCharacter;
        public void Init(Character originalCharacter)
        {
            _originalCharacter = originalCharacter;
            _originalCharacter.gameObject.SetActive(false);
            Observable.Timer(TimeSpan.FromSeconds(5)).First().Subscribe(_ =>
            {
                Resurrect();
            });
        }

        public void Resurrect()
        {
            _originalCharacter.gameObject.SetActive(true);
            _originalCharacter.Health.Set(_originalCharacter.Health.Max.Value);
            _originalCharacter.IsDead.Value = false;
            Destroy(gameObject);
        }
    }
}