using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitFollower : MonoBehaviour
{
    private GameObject gameManager;

    public void Init(GameObject gameManager)
    {
        this.gameManager = gameManager;
    }

    public void AttachToNearestOrbit()
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
        if(gameManager.TryGetComponent<StarSpawner>(out StarSpawner spawner))
        {
            foreach(StarInitializer star in spawner.SpawnedObjects)
            {
                if(star.TryGetComponent<Orbitable>(out Orbitable orbitable))
                {
                    foreach(Orbiter orbit in orbitable.Orbits)
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
            }
        }
        if(nearestOrbitAnchor != null)
        {
            nearestOrbitAnchor.GetComponentInParent<Orbiter>().IsOccupied = true;
        }
        return nearestOrbitAnchor;
    }
}
