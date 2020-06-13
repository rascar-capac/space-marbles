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
    private Rigidbody2D rb;
    private Vector2 mouseOffset;
    private AnimShootable anim;

    public void Init(Camera mainCamera)
    {
        this.mainCamera = mainCamera;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<AnimShootable>();
    }

    private void Update()
    {
        anim.SetSpeed(rb.velocity.magnitude);
    }

    private void OnMouseDown()
    {
        anim?.Collide();
        anim?.SetAiming(true);
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
        anim?.SetAimLevel(aimLevel);
    }

    private void OnMouseUp()
    {
        anim?.SetAiming(false);
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
