using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zoomable : MonoBehaviour
{
    [SerializeField] [Range(0.1f, 1f)] private float sensitivity = 1f;
    [SerializeField] [Range(0.1f, 1f)] private float speed = 0.1f;
    [SerializeField] [Range(1f, 200f)] private float minZoom = 3f;
    [SerializeField] [Range(1f, 200f)] private float maxZoom = 100f;

    private Camera mainCamera;
    private float currentZoomingVelocity;
    private Vector3 currentMovingVelocity;
    private float targetCameraSize;
    private Vector3 targetCameraPosition;

    private const float ZOOM_DELTA = 0.001f;

    private void Awake()
    {
        mainCamera = GetComponent<Camera>();
        currentZoomingVelocity = 0;
        currentMovingVelocity = Vector3.zero;
    }

    private void Start()
    {
        targetCameraSize = mainCamera.orthographicSize;
        targetCameraPosition = mainCamera.transform.position;
    }

    private void LateUpdate()
    {
        float scrollWheelAxis = Input.GetAxis("Mouse ScrollWheel");
        if(scrollWheelAxis != 0)
        {
            // TODO dynamic zoom
            // float rawTargetCameraSize = targetCameraSize - (scrollWheelAxis * (mainCamera.orthographicSize / maxZoom) * (sensitivity * 500));
            float rawTargetCameraSize = targetCameraSize - (scrollWheelAxis * (sensitivity * 50));
            targetCameraSize = Mathf.Clamp(rawTargetCameraSize, minZoom, maxZoom);
            if(targetCameraSize == rawTargetCameraSize)
            {
                Vector3 mouseOffset = mainCamera.ScreenToWorldPoint(Input.mousePosition) - targetCameraPosition;
                targetCameraPosition += mouseOffset / Mathf.Abs(mainCamera.orthographicSize - targetCameraSize);
            }
        }

        if(Mathf.Abs(mainCamera.orthographicSize - targetCameraSize) > ZOOM_DELTA)
        {
            mainCamera.orthographicSize = LeanSmooth.damp(mainCamera.orthographicSize, targetCameraSize, ref currentZoomingVelocity, speed);
            mainCamera.transform.position = LeanSmooth.damp(mainCamera.transform.position, targetCameraPosition, ref currentMovingVelocity, speed);
        }
    }
}
