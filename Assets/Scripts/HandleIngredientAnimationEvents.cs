using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleIngredientAnimationEvents : MonoBehaviour
{
    private Spawner gameManager;

    private void Awake()
    {
        gameManager = FindObjectOfType<Spawner>();
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
        gameManager.DestroySpawnedObject<IngredientInitializer>(this.gameObject);
    }
}
