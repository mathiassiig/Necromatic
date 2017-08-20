using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Necromatic.Utility;
public class ClosestPointRenderer : MonoBehaviour
{
    public Transform _target;

    void OnDrawGizmos()
    {
        var pos = Vector3Utils.ClosestPointOnCircle(_target.position, 2, transform.position);
        var sliceAngles = Vector3Utils.GetClosestPieSlice(_target.position, 2, pos, 36);
        var first = Vector3Utils.AngleOnCircleToPoint(_target.position, 2, sliceAngles.x);
        var second = Vector3Utils.AngleOnCircleToPoint(_target.position, 2, sliceAngles.y);
        Gizmos.DrawCube(first, Vector3.one/4);
        Gizmos.DrawCube(second, Vector3.one / 4);
    }
}
