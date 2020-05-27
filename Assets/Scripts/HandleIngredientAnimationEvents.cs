using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleIngredientAnimationEvents : MonoBehaviour
{
    private IngredientSpawner gameManager;

    private void Awake()
    {
        gameManager = FindObjectOfType<IngredientSpawner>();
    }

    private void SpawnPlanet()
    {
        PlanetGenerator detector = GetComponentInChildren<PlanetGenerator>();
        if(detector.CanGeneratePlanet)
        {
            detector.SpawnPlanet();
        }
    }

    private void DestroyIngredient()
    {
        gameManager.DestroySpawnedObject(this.GetComponent<IngredientInitializer>());
    }
}
