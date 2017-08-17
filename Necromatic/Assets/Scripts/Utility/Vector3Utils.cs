using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Necromatic.Utility
{
    public static class Vector3Utils
    {
        public static bool PointingTowards(Transform a, Transform b, float angle)
        {
            Vector2 aForward = new Vector2(a.forward.x, a.forward.z).normalized;
            Vector2 aPos = new Vector2(a.position.x, a.position.z);
            Vector2 bPos = new Vector2(b.position.x, b.position.z);
            var angleBetween = Vector2.Angle(aForward, bPos - aPos);
            return angleBetween <= angle;
        }
        public static float Distance(Vector3 a, Vector3 b) => Mathf.Abs((a - b).magnitude);
        public static bool DistanceGreater(Vector3 a, Vector3 b, float threshold) => Distance(a, b) > threshold;
        public static float XZDistance(Vector3 a, Vector3 b)
        {
            Vector2 a2 = new Vector2(a.x, a.z);
            Vector2 b2 = new Vector2(b.x, b.z);
            return Mathf.Abs((a2 - b2).magnitude);
        }

        public static bool XZDistanceGreater(Vector3 a, Vector3 b, float threshold) => XZDistance(a, b) > threshold;

        public static Transform GetClosestTransform(Transform[] targets, Transform root)
        {
            Transform bestTarget = null;
            float closestDistanceSqr = Mathf.Infinity;
            Vector3 currentPosition = root.position;
            foreach (Transform potentialTarget in targets)
            {
                Vector3 directionToTarget = potentialTarget.position - currentPosition;
                float dSqrToTarget = directionToTarget.sqrMagnitude;
                if (dSqrToTarget < closestDistanceSqr)
                {
                    closestDistanceSqr = dSqrToTarget;
                    bestTarget = potentialTarget;
                }
            }

            return bestTarget;
        }
    }
}