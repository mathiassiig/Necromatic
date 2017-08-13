using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
namespace Necromatic.Char
{
    [System.Serializable]
    public class Stat
    {
        public ReactiveProperty<float> Max = new ReactiveProperty<float>();
        public ReactiveProperty<float> Current = new ReactiveProperty<float>();

        [SerializeField] private float _initial;
        [SerializeField] private float _regen; // per second

        public Character LastSender { get; private set; }
        public bool LastSenderAdded { get; private set; }
        
        public void Init()
        {
            Max.Value = _initial;
            Current.Value = _initial;
            Observable.EveryUpdate().Subscribe(_ =>
            {
                if (Current.Value < Max.Value)
                {
                    Add(_regen * Time.deltaTime);
                }
            });
        }

        public void Set(float value)
        {
            Current.Value = value;
        }

        public void Add(float value)
        {
            if(value > 0 && Current.Value == Max.Value)
            {
                return;
            }
            Current.Value += value;
            Current.Value = Mathf.Clamp(Current.Value, 0, Max.Value);
        }

        public void Add(float value, Character sender)
        {
            LastSender = sender;
            LastSenderAdded = value > 0;
            Add(value);
        }
    }
}
