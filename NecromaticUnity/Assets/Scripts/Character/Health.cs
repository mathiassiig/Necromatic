using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Necromatic.Character
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UniRx;
    [System.Serializable]
    public class Stat
    {
        public ReactiveProperty<float> Max = new ReactiveProperty<float>();
        public ReactiveProperty<float> Current = new ReactiveProperty<float>();

        [SerializeField] private float _initial;
        //[SerializeField] private float _regen; // per second

        public bool LastSenderAdded { get; private set; }

        public void Init()
        {
            Max.Value = _initial;
            Current.Value = _initial;
            /*Observable.EveryUpdate().Subscribe(_ =>
            {
                if (Current.Value < Max.Value)
                {
                    Add(_regen * Time.deltaTime);
                }
            });*/
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
            return Mathf.Clamp(before-Current.Value, 0, Max.Value);
        }
    }
}