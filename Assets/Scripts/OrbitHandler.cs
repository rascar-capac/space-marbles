using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitHandler : MonoBehaviour
{
    [SerializeField] private Transform _anchor = null;

    public float Width { get; set; }
    public float Heigth { get; set; }
    public float Period { get; set; }
    public bool IsClockwise { get; set; }
    public bool IsOccupied { get; set; }
    public Transform Anchor { get => _anchor; }

    private float timer;

    private void Start()
    {
        timer = 0;
        IsOccupied = false;
    }

    private void Update()
    {
        Anchor.localPosition = Evaluate(timer / Period);
        timer = Mathf.Repeat(timer + Time.deltaTime, Period);
    }

    public Vector2 Evaluate(float t)
    {
        float radAngle = (IsClockwise ? 360 - 360 * t : 360 * t) * Mathf.Deg2Rad;
        float x = Mathf.Cos(radAngle) * Width;
        float y = Mathf.Sin(radAngle) * Heigth;
        return new Vector2(x, y);
    }
}
