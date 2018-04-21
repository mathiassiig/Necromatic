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

        public static Vector3 Direction(Transform from, Transform to) => (to.position - from.position).normalized;
        public static Vector3 Direction(Vector3 from, Vector3 to) => (to - from).normalized;

        // from https://answers.unity.com/questions/248788/calculating-ball-trajectory-in-full-3d-world.html
        public static Vector3 CalculateBestThrowSpeed(Vector3 origin, Vector3 target, float timeToTarget)
        {
            // calculate vectors
            Vector3 toTarget = target - origin;
            Vector3 toTargetXZ = toTarget;
            toTargetXZ.y = 0;

            // calculate xz and y
            float y = toTarget.y;
            float xz = toTargetXZ.magnitude;

            // calculate starting speeds for xz and y. Physics forumulase deltaX = v0 * t + 1/2 * a * t * t
            // where a is "-gravity" but only on the y plane, and a is 0 in xz plane.
            // so xz = v0xz * t => v0xz = xz / t
            // and y = v0y * t - 1/2 * gravity * t * t => v0y * t = y + 1/2 * gravity * t * t => v0y = y / t + 1/2 * gravity * t
            float t = timeToTarget;
            float v0y = y / t + 0.5f * Physics.gravity.magnitude * t;
            float v0xz = xz / t;

            // create result vector for calculated starting speeds
            Vector3 result = toTargetXZ.normalized;        // get direction of xz but with magnitude 1
            result *= v0xz;                                // set magnitude of xz to v0xz (starting speed in xz plane)
            result.y = v0y;                                // set y to v0y (starting speed of y plane)

            return result;
        }
    }
}