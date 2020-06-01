using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Identifiable))]
public class IngredientInitializer : A2DDataInitializer<IngredientData>
{
    [SerializeField] private GameObject body = null;
    [SerializeField] private GameObject detector = null;

    public override void Init(Canvas canvas, Camera mainCamera)
    {
        base.Init(canvas, mainCamera);

        GetComponent<Identifiable>()?.Init(data.IngredientName, canvas, mainCamera);
        if(TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
        {
            rb.mass = data.Mass;
            rb.drag = data.Drag;
        }
        if(TryGetComponent<Collider2D>(out Collider2D collider))
        {
            collider.sharedMaterial = data.PhysicsMaterial;
        }
        if(TryGetComponent<Animator>(out Animator animator))
        {
            animator.runtimeAnimatorController = data.AnimatorController;
        }
        if(body.TryGetComponent<SpriteRenderer>(out SpriteRenderer renderer))
        {
            renderer.sprite = data.BodySprite;
        }
        detector.GetComponent<PlanetGenerator>()?.Init(data.InfluenceZone, canvas, mainCamera);
    }
}
