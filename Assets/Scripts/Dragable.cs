using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class Dragable : MonoBehaviour
{
    public bool IsAiming { get; set; }

    [SerializeField] [Range(0, 1f)] private float sensitivity = 0.5f;
    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = GetComponent<Camera>();
        IsAiming = false;
    }

    private void LateUpdate()
    {
        if(!IsAiming && Input.GetMouseButton(0))
        {
            Vector3 mouseMouvement =new Vector3(- Input.GetAxis("Mouse X"), - Input.GetAxis("Mouse Y"), 0);
            transform.position += mouseMouvement * mainCamera.orthographicSize / 10 * sensitivity;
        }
    }
}
