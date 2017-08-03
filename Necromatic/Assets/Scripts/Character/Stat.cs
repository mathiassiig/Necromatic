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



    public void Init()
    {
        Max.Value = _initial;
        Current.Value = _initial;
    }

    public void Add(float value)
    {
        Current.Value += value;
    }
}
