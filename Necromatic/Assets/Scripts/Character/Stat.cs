using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat
{
    public float Max;
    public float Current;
    public float Regen; // per second

    public void SetToMax()
    {
        Current = Max;
    }

    public void Add(float value)
    {
        Current += value;
    }
}
