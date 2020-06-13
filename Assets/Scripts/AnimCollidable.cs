using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class AnimCollidable : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.TryGetComponent<StarInitializer>(out StarInitializer star))
        {
            foreach(Collider2D collider in GetComponentsInChildren<Collider2D>())
            {
                collider.enabled = false;
            }
            animator.SetTrigger("CollideWithStar");
        }
        else
        {
            animator.SetTrigger("Collide");
        }
    }
}
