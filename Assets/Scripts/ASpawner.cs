﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ASpawner<T, U> : MonoBehaviour
        where T : ADataInitializer<U>
{
    public List<T> SpawnedObjects => spawnedObjects;

    [SerializeField] private List<T> prefabs = null;
    [SerializeField] private Transform context = null;
    [SerializeField] private bool hasUniqueData = false;
    [SerializeField] protected List<U> dataDeck = null;

    protected List<T> spawnedObjects;

    public void AddData(U newData)
    {
        dataDeck.Add(newData);
    }

    public void AddData(List<U> newData)
    {
        dataDeck.AddRange(newData);
    }

    protected virtual void Awake()
    {
        spawnedObjects = new List<T>();
    }

    protected virtual T SpawnObject()
    {

        T prefab = prefabs.Count > 1 ? prefabs[Random.Range(0, prefabs.Count)] : prefabs[0];
        T newObject = Instantiate(prefab, context);
        U data = PickRandomData(dataDeck, hasUniqueData);
        newObject.Init(data);
        spawnedObjects.Add(newObject);

        return newObject;
    }

    private U PickRandomData(List<U> deck, bool IsUnique)
    {
        U data = deck[Random.Range(0, deck.Count)];
        if(IsUnique)
        {
            deck.Remove(data);
        }
        return data;
    }

    public void DestroySpawnedObject(T spawnedObject)
    {
        spawnedObjects.Remove(spawnedObject);
        Destroy(spawnedObject);
    }
}
