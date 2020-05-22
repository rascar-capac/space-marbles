using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Shootable : MonoBehaviour
{
    [SerializeField] private float power = 10f;
    [SerializeField] private float maxRadius = 10f;
    [SerializeField] private float maxPrecisionOffset = 30f;

    private Camera mainCamera;
    private Animator animator;
    private Rigidbody2D rb;
    private Vector2 mouseOffset;

    private void Awake()
    {
        mainCamera = Camera.main;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // private void Start()
    // {
    //     aiming = false;
    // }

    // private void Update()
    // {
    //     if(Input.GetMouseButtonDown(0))
    //     {
    //         RaycastHit hit;
    //         Ray mouseRay = mainCamera.ScreenPointToRay(Input.mousePosition);
    //         if(Physics.Raycast(mouseRay, LayerMask.))
    //     }

    //     if(Input.GetMouseButtonUp(0))
    //     {
    //         Vector2 velocity = transform.position - mainCamera.ScreenToWorldPoint(Input.mousePosition);
    //         if(velocity.magnitude > maxRadius)
    //         {
    //             velocity = velocity.normalized * maxRadius;
    //         }
    //         GetComponent<Rigidbody2D>().AddForce(velocity * power, ForceMode2D.Impulse);
    //     }
    // }

    private void Update()
    {
        animator.SetFloat("Speed", rb.velocity.magnitude);
    }

    private void OnMouseDown()
    {
        animator.SetTrigger("Collide");
        animator.SetBool("IsAiming", true);
        if(mainCamera.TryGetComponent<Dragable>(out Dragable camera))
        {
            camera.IsAiming = true;
        }
    }

    private void OnMouseDrag()
    {
        mouseOffset = transform.position - mainCamera.ScreenToWorldPoint(Input.mousePosition);
        float aimIntensity = mouseOffset.magnitude;
        int aimLevel = 0;
        if(aimIntensity < maxRadius / 2)
        {
            aimLevel = 0;
        }
        else if(aimIntensity < maxRadius)
        {
            aimLevel = 1;
        }
        else
        {
            mouseOffset = mouseOffset.normalized * maxRadius;
            aimLevel = 2;
        }
        animator.SetInteger("AimLevel", aimLevel);
    }

    private void OnMouseUp()
    {
        animator.SetBool("IsAiming", false);
        if(mainCamera.TryGetComponent<Dragable>(out Dragable camera))
        {
            camera.IsAiming = false;
        }

        float maxOffsetAngle = (1 - GetComponent<IngredientInitializer>().Data.Precision) * maxPrecisionOffset;
        float offsetAngle = Mathf.Deg2Rad * Random.Range(- maxOffsetAngle, maxOffsetAngle);
        Vector2 force = mouseOffset * power;
        force = new Vector2(
            force.x * Mathf.Cos(offsetAngle) - force.y * Mathf.Sin(offsetAngle),
            force.x * Mathf.Sin(offsetAngle) + force.y * Mathf.Cos(offsetAngle)
        );
        rb.AddForce(force, ForceMode2D.Impulse);
    }
}
