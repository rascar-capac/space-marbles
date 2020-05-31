using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A2DDataInitializer<T> : ADataInitializer<T> where T : ScriptableObject
{
    public Canvas Canvas => canvas;
    public Camera MainCamera => mainCamera;

    private Canvas canvas;
    private Camera mainCamera;

    public void Init(Canvas canvas, Camera mainCamera)
    {
        this.canvas = canvas;
        this.mainCamera = mainCamera;
    }
}
