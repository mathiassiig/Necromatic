using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
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

        // todo: in XZ plane
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

        public static Vector2 ToXZVector(Vector3 v)
        {
            return new Vector2(v.x, v.z);
        }

        // point in, closest point out
        public static Vector2 ClosestPointOnCircle(Vector3 center, float radius, Vector3 point)
        {
            Vector2 c = ToXZVector(center);
            Vector2 p = ToXZVector(point);
            return c + radius * ((p - c) / (p - c).magnitude);
        }

        public static Vector3 AngleOnCircleToPoint(Vector3 center, float radius, float angle)
        {
            var x = center.x + radius * Mathf.Cos(angle);
            var z = center.z + radius * Mathf.Sin(angle);
            return new Vector3(x, center.y, z);
        }
        
        // return a vector2 representing the start (x) and end (y) angle of the pie slice
        public static Vector2 GetClosestPieSlice(Vector3 center, float radius, Vector2 pointOnCircle, int amountOfSlices)
        {
            Vector2 c = ToXZVector(center);
            var p_angle = Mathf.Atan2(pointOnCircle.y - c.y, pointOnCircle.x - c.x);
            p_angle = Mathf.Sign(p_angle) == -1 ? 2*Mathf.PI+p_angle : p_angle;
            var anglePerSlice = (Mathf.PI * 2) / amountOfSlices;
            var sliceMin = Mathf.FloorToInt(p_angle / anglePerSlice);
            var sliceMax = Mathf.Repeat(sliceMin + 1, amountOfSlices);
            var sliceMinAngle = anglePerSlice * sliceMin;
            var sliceMaxAngle = anglePerSlice * sliceMax;
            return new Vector2(sliceMinAngle, sliceMaxAngle);
        }
    }
}