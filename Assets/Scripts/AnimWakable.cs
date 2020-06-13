using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class AnimWakable : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnMouseDown()
    {
        WakeUp();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        WakeUp();
    }

    private void WakeUp()
    {
        animator.SetTrigger("WakeUp");
        // TODO wtf?
        animator.ResetTrigger("WakeUp");
    }
}
