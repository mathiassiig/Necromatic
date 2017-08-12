using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Necromatic.Utility
{
    public static class Vector3Utils
    {
        public static float Distance(Vector3 a, Vector3 b) => Mathf.Abs((a - b).magnitude);
        public static bool DistanceGreater(Vector3 a, Vector3 b, float threshold) => Distance(a, b) > threshold;
    }
}