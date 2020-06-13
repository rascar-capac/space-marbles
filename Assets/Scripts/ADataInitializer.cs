using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class ADataInitializer<T> : MonoBehaviour
{
    public T Data => data;
    public DataInitializedEvent OnDataInitialized => onDataInitialized;

    protected T data;
    private GameObject gameManager;
    private DataInitializedEvent onDataInitialized;

    public virtual void InitData(T data)
    {
        this.data = data;
        onDataInitialized.Invoke(data);
    }

    protected void Init(GameObject gameManager)
    {
        this.gameManager = gameManager;
    }

    protected virtual void Awake()
    {
        onDataInitialized = new DataInitializedEvent();
    }

    public class DataInitializedEvent : UnityEvent<T> {}
}
