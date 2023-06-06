using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float sensitivityX = 100.0f;
    public float sensitivityY = 100.0f;

    public float minimumX = -90.0f;
    public float maximumX = 90.0f;

    private float _rotationX = 0.0f;
    private float _rotationY = 0.0f;

    public Transform cameraHolder;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        _rotationY += Input.GetAxis("Mouse X") * sensitivityX * Time.deltaTime;
        _rotationX -= Input.GetAxis("Mouse Y") * sensitivityY * Time.deltaTime;
        _rotationX = Mathf.Clamp(_rotationX, minimumX, maximumX);

        cameraHolder.localRotation = Quaternion.Euler(_rotationX, _rotationY, 0);
        transform.rotation = Quaternion.Euler(0, _rotationY, 0);
    }
}