using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace Necromatic.Character
{
    [System.Serializable]
    public class Stat
    {
        public ReactiveProperty<float> Max = new ReactiveProperty<float>();
        public ReactiveProperty<float> Current = new ReactiveProperty<float>();
        public ReactiveProperty<float> Regen = new ReactiveProperty<float>();

        public float _initialRegen = 0;
        public float _initial;
        //[SerializeField] private float _regen; // per second
        public bool LastSenderAdded { get; private set; }
        private IDamagable _owner;

        public void Init(IDamagable owner)
        {
            Max.Value = _initial;
            Current.Value = _initial;
            Regen.Value = _initialRegen;
            _owner = owner;
            Observable
                .EveryUpdate()
                .TakeUntilDestroy(owner.gameObject)
                .TakeWhile(x => !owner.Death.Dead.Value)
                .Subscribe(_ =>
                {
                    Add(Regen.Value * Time.deltaTime);
                });
        }

        public void Set(float value)
        {
            Current.Value = value;
        }

        public float Add(float value)
        {
            var before = Current.Value;
            if (value > 0 && Current.Value == Max.Value)
            {
                return 0;
            }
            else
            {
                Current.Value = Mathf.Clamp(Current.Value + value, 0, Max.Value);
            }
            return Mathf.Clamp(before - Current.Value, 0, Max.Value);
        }
    }
}