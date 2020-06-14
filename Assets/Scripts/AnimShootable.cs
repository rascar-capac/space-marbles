using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimShootable : MonoBehaviour
{
    private Animator animator;

    public void SetSpeed(float speed)
    {
        animator.SetFloat("Speed", speed);
    }

    public void Collide()
    {
        animator.SetTrigger("Collide");
    }

    public void SetAiming(bool isAiming)
    {
        animator.SetBool("IsAiming", isAiming);
    }

    public void SetAimLevel(int aimLevel)
    {
        animator.SetInteger("AimLevel", aimLevel);
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
}
