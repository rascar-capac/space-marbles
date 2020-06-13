using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimOrbitFollower : MonoBehaviour
{
    private OrbitFollower orbitFollower;

    private void Awake()
    {
        orbitFollower = GetComponent<OrbitFollower>();
    }

    private void AttachToNearestOrbit()
    {
        orbitFollower.AttachToNearestOrbit();
    }
}
