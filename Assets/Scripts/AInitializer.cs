using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AInitializer<T> : MonoBehaviour where T : AData
{
    [SerializeField] private List<T> datas = null;

    public T Data { get; set; }

    protected virtual void Init()
    {
        Data = datas[Random.Range(0, datas.Count)];
    }
}
