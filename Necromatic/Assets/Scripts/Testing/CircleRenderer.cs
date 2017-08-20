using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleRenderer : MonoBehaviour
{
    void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, 2f);
    }
}
