using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Identifiable))]
public class IngredientInitializer : A2DDataInitializer<IngredientData>
{
    [SerializeField] private GameObject body = null;
    [SerializeField] private GameObject detector = null;

    public override void InitData(IngredientData data)
    {
        base.InitData(data);

        GetComponent<Identifiable>()?.Init(Data.IngredientName, Canvas, MainCamera);

        if(TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
        {
            rb.mass = Data.Mass;
            rb.drag = Data.Drag;
        }
        if(TryGetComponent<Collider2D>(out Collider2D collider))
        {
            collider.sharedMaterial = Data.PhysicsMaterial;
        }
        if(TryGetComponent<Animator>(out Animator animator))
        {
            animator.runtimeAnimatorController = Data.AnimatorController;
        }
        if(body.TryGetComponent<SpriteRenderer>(out SpriteRenderer renderer))
        {
            renderer.sprite = Data.BodySprite;
        }
        if(detector.TryGetComponent<CircleCollider2D>(out CircleCollider2D detectorCollider))
        {
            detectorCollider.radius = Data.InfluenceZone;
        }
        // TODO detector.GetComponent<PlanetGenerator>()?.Init(Data.InfluenceZone);
    }
}
