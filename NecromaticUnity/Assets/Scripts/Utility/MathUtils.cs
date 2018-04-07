using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Necromatic.Utility
{
    public static class MathUtils
    {
        public static Vector2 CirclePoint(Vector2 center, float radius, float angle)
        {
            var x = center.x + radius * Mathf.Cos(angle);
            var y = center.y + radius * Mathf.Sin(angle);
            return new Vector2(x, y);
        }

        public static float Distance(Vector3 a, Vector3 b) => (a - b).magnitude;

        public static Vector2 PlaneDirection(Transform from, Transform to)
        {
            var fromV = new Vector2(from.position.x, from.position.z);
            var toV = new Vector2(to.position.x, to.position.z);
            return (toV - fromV).normalized;
        }
    }
}