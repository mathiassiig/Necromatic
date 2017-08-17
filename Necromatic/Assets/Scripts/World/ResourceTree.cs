using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceTree : MonoBehaviour, IClickable
{
    [SerializeField] private Rigidbody _log;

    public bool Cut { get; private set; }
    private void Awake()
    {

    }

    public void Timber(Vector3 forceDirection)
    {
        Cut = true;
        gameObject.layer = LayerMask.NameToLayer("Default");
        forceDirection *= _log.mass;
        _log.constraints = RigidbodyConstraints.None;
        _log.AddForce(forceDirection, ForceMode.Impulse);
    }

    public void OnClick()
    {
        // TODO: implement later
    }
}
