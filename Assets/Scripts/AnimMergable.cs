using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimMergable : MonoBehaviour
{
    [SerializeField] private Mergable detector = null;
    private Animator animator;

    public void Merge()
    {
        animator.SetTrigger("Merge");
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void SpawnPlanet()
    {
        detector?.SpawnPlanet();
    }

    private void DestroyIngredient()
    {
        detector?.DestroyIngredient();
    }
}
