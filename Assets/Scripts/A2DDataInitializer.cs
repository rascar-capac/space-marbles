using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A2DDataInitializer<T> : ADataInitializer<T>
{
    protected Canvas canvas;
    protected Camera mainCamera;

    public virtual void Init(Canvas canvas, Camera mainCamera)
    {
        this.canvas = canvas;
        this.mainCamera = mainCamera;
    }
}
