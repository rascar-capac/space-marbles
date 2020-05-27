using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Identifiable))]
public class IngredientInitializer : ADataInitializer<IngredientData>
{
    [SerializeField] private GameObject body = null;
    [SerializeField] private PlanetGenerator detector = null;

    public override void Init(IngredientData data)
    {
        base.Init(data);
        GetComponent<Identifiable>().Name = Data.IngredientName;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.mass = Data.Mass;
        rb.drag = Data.Drag;
        GetComponent<Collider2D>().sharedMaterial = Data.PhysicsMaterial;
        GetComponent<Animator>().runtimeAnimatorController = Data.AnimatorController;
        body.GetComponent<SpriteRenderer>().sprite = Data.BodySprite;
        detector.GetComponent<CircleCollider2D>().radius = Data.InfluenceZone;
    }
}
