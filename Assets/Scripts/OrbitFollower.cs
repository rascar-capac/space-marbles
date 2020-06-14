using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpringJoint2D))]
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
        if(!gameManager.TryGetComponent<StarSpawner>(out StarSpawner spawner))
        {
            return null;
        }

        Rigidbody2D nearestOrbitAnchor = null;
        float nearestOrbitAnchorDistance = float.MaxValue;
        foreach(StarInitializer star in spawner.SpawnedObjects)
        {
            if(!star.TryGetComponent<Orbitable>(out Orbitable orbitable))
            {
                break;
            }

            foreach(Orbiter orbit in orbitable.Orbits)
            {
                if(orbit.IsOccupied)
                {
                    break;
                }

                float distance = Vector2.Distance(transform.position, orbit.transform.position);
                if(distance < nearestOrbitAnchorDistance &&
                        orbit.Anchor.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
                {
                    nearestOrbitAnchor = rb;
                    nearestOrbitAnchorDistance = distance;
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
