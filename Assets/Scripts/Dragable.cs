﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragable : MonoBehaviour
{
    [SerializeField] [Range(0, 1f)] private float sensitivity = .5f;

    public bool IsAiming { get; set; }

    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = GetComponent<Camera>();
    }

    private void Start()
    {
        IsAiming = false;
    }

    private void LateUpdate()
    {
        if(!IsAiming && Input.GetMouseButton(0))
        {
            Vector3 mouseMouvement =new Vector3(- Input.GetAxis("Mouse X"), - Input.GetAxis("Mouse Y"), 0);
            mainCamera.transform.position += mouseMouvement * mainCamera.orthographicSize / 10 * sensitivity;
        }
    }
}
