﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private const string PAN_BUTTON = "Fire3";
    private Vector3 _delta;
    private const float _panSpeed = 0.025f;
    private float _beginHeightT = 0.5f;
    private const float _heightSpeed = 5f;
    private float _minHeight = 3;
    private float _maxHeight = 14;

    void Start()
    {
        transform.position = new Vector3(0, Mathf.Lerp(_minHeight,_maxHeight, _beginHeightT), 0);
        transform.rotation = Quaternion.Euler(56, 0, 0);
    }

    void Update()
    {
        if (Input.GetButtonDown(PAN_BUTTON))
        {
            _delta = Vector3.zero;
            Cursor.lockState = CursorLockMode.Locked;
        }
        if (Input.GetButton(PAN_BUTTON))
        {
            _delta += new Vector3(Input.GetAxis("Mouse X"), 0, Input.GetAxis("Mouse Y"));
            transform.position += _delta * _panSpeed;
        }
        if (Input.GetButtonUp(PAN_BUTTON))
        {
            Cursor.lockState = CursorLockMode.None;
        }
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            transform.position = new Vector3(
                transform.position.x,
                Mathf.Clamp(transform.position.y + -Input.GetAxis("Mouse ScrollWheel") * _heightSpeed, _minHeight, _maxHeight),
                transform.position.z);
        }
    }
}
