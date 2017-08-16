using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceTree : MonoBehaviour
{
    [SerializeField] private Rigidbody _log;

    private void Awake()
    {
        Timber(1.5f * Vector3.left);
    }

    public void Timber(Vector3 force)
    {
        _log.constraints = RigidbodyConstraints.None;
        _log.AddForce(force, ForceMode.Impulse);
    }
}
