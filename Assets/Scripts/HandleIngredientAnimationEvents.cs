using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleIngredientAnimationEvents : MonoBehaviour
{
    private GameObject gameManager;

    public void Init(GameObject gameManager)
    {
        this.gameManager = gameManager;
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
        gameManager.GetComponent<IngredientSpawner>()?.DestroySpawnedObject(GetComponent<IngredientInitializer>());
    }
}
