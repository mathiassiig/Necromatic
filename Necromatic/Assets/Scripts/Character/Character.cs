using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
namespace Necromatic.Character
{
    public class Character : MonoBehaviour
    {
        public Stat Health;

        public ReactiveProperty<bool> IsDead = new ReactiveProperty<bool>();

        private void Awake()
        {
            Health.Init();
            IsDead.Subscribe(x =>
            {
                if (x) HandleDeath();
            });
            Health.Current.Subscribe(x =>
            {
                if (x <= 0) IsDead.Value = true;
            });
        }

        void Update()
        {

        }

        protected virtual void HandleDeath()
        {
            Destroy(gameObject);
        }
    }
}