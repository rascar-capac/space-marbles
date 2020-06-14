using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbiter : MonoBehaviour
{
    public Transform Anchor => anchor;
    public bool IsOccupied { get; set; }

    [SerializeField] private Transform anchor = null;
    private float width;
    private float heigth;
    private float period;
    private bool isClockwise;
    private float timer;

    public void Init(float width, float height, float period, bool isClockwise, Camera mainCamera)
    {
        this.width = width;
        this.heigth = height;
        this.period = period;
        this.isClockwise = isClockwise;
        GetComponent<OrbitRenderer>()?.Init(mainCamera);
    }

    private void Start()
    {
        timer = 0;
        IsOccupied = false;
    }

    private void Update()
    {
        anchor.localPosition = Evaluate(timer / period);
        timer = Mathf.Repeat(timer + Time.deltaTime, period);
    }

    public Vector2 Evaluate(float t)
    {
        float radAngle = (isClockwise ? 360 - 360 * t : 360 * t) * Mathf.Deg2Rad;
        float x = Mathf.Cos(radAngle) * width;
        float y = Mathf.Sin(radAngle) * heigth;
        return new Vector2(x, y);
    }
}
