using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandlePlanetAnimationEvents : MonoBehaviour
{
    private StarSpawner gameManager;

    private void Awake()
    {
        gameManager = FindObjectOfType<StarSpawner>();
    }

    private void AttachToNearestOrbit()
    {
        Rigidbody2D nearestOrbit = FindNearestOrbit();
        SpringJoint2D joint = GetComponent<SpringJoint2D>();
        if(nearestOrbit)
        {
            joint.connectedBody = nearestOrbit;
            joint.enabled = true;
        }
    }

    private Rigidbody2D FindNearestOrbit()
    {
        Rigidbody2D nearestOrbitAnchor = null;
        float nearestOrbitAnchorDistance = float.MaxValue;
        foreach(StarInitializer star in gameManager.SpawnedObjects)
        {
            foreach(OrbitHandler orbit in star.Orbits)
            {
                if(!orbit.IsOccupied)
                {
                    float distance = Vector2.Distance(transform.position, orbit.transform.position);
                    if(distance < nearestOrbitAnchorDistance)
                    {
                        nearestOrbitAnchor = orbit.Anchor.GetComponent<Rigidbody2D>();
                        nearestOrbitAnchorDistance = distance;
                    }
                }
            }
        }
        if(nearestOrbitAnchor != null)
        {
            nearestOrbitAnchor.GetComponentInParent<OrbitHandler>().IsOccupied = true;
        }
        return nearestOrbitAnchor;
    }
}
