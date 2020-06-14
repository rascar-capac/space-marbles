using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(Orbiter))]
public class OrbitRenderer : MonoBehaviour
{
    [SerializeField] private int segmentsCount = 30;
    private Camera mainCamera;
    private LineRenderer lineRenderer;
    private Orbiter orbitHandler;

    public void Init(Camera mainCamera)
    {
        this.mainCamera = mainCamera;
    }

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        orbitHandler = GetComponent<Orbiter>();
    }

    private void LateUpdate()
    {
        ComputeLine();
    }

    private void ComputeLine()
    {
        Vector3[] points = new Vector3[segmentsCount + 1];
        for(int i = 0 ; i < segmentsCount ; i++)
        {
            Vector2 localPoint = orbitHandler.Evaluate((float) i / (float) segmentsCount);
            points[i] = transform.TransformPoint(localPoint);
        }
        points[segmentsCount] = points[0];

        lineRenderer.positionCount = segmentsCount + 1;
        lineRenderer.SetPositions(points);
        lineRenderer.widthMultiplier = mainCamera.orthographicSize / 50;
    }
}
