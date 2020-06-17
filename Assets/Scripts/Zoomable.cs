using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Camera))]
public class Zoomable : MonoBehaviour
{
    [SerializeField] [Range(1, 100)] private int zoomLevelCount = 10;
    [SerializeField] [Range(1, 200)] private int minZoom = 10;
    [SerializeField] [Range(1, 200)] private int maxZoom = 100;
    [SerializeField] [Range(0.1f, 1f)] private float zoomDuration = 0.1f;
    private Camera mainCamera;
    private float targetedCameraSize;
    private Vector3 targetedCameraPosition;
    private Sequence zoom;
    private int currentZoomLevel;

    public void Stop()
    {
        mainCamera.DOKill();
        mainCamera.transform.DOKill();
    }

    private void Awake()
    {
        mainCamera = GetComponent<Camera>();
        zoom = DOTween.Sequence();
        currentZoomLevel = zoomLevelCount - 1;
    }

    private void Start()
    {
        mainCamera.orthographicSize = maxZoom;
        targetedCameraPosition = mainCamera.transform.position;
    }

    private void LateUpdate()
    {
        float zoomIntensity = Input.mouseScrollDelta.y;
        if(zoomIntensity != 0)
        {
            int unclampedCurrentZoomLevel = currentZoomLevel - (int) zoomIntensity;
            currentZoomLevel = Mathf.Clamp(unclampedCurrentZoomLevel, 0, zoomLevelCount - 1);
            if(currentZoomLevel == unclampedCurrentZoomLevel)
            {
                targetedCameraSize = ComputeCameraSize(currentZoomLevel);

                Vector3 mouseOffset = mainCamera.ScreenToWorldPoint(Input.mousePosition) - mainCamera.transform.position;
                targetedCameraPosition = mainCamera.transform.position + mouseOffset - mouseOffset * (targetedCameraSize / mainCamera.orthographicSize);
            }

            Stop();
            zoom.Append(mainCamera.DOOrthoSize(targetedCameraSize, zoomDuration))
                .Join(mainCamera.transform.DOMove(targetedCameraPosition, zoomDuration));
        }
    }

    private float ComputeCameraSize(int zoomLevel)
    {
        // return Mathf.Pow(maxZoom - minZoom + 1, (float) zoomLevel / (zoomLevelCount - 1)) + minZoom - 1;
        return (maxZoom - minZoom) * Mathf.Pow(zoomLevel, 2) / Mathf.Pow(zoomLevelCount - 1, 2) + minZoom;
    }
}
