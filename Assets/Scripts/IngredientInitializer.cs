using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientInitializer : A2DDataInitializer<IngredientData>
{
    public GameObject Detector => detector;

    [SerializeField] private GameObject body = null;
    [SerializeField] private GameObject detector = null;

    public override void Init(GameObject gameManager, Canvas canvas, Camera mainCamera)
    {
        base.Init(gameManager, canvas, mainCamera);

        GetComponent<Identifiable>()?.Init(data.IngredientName, canvas, mainCamera);
        GetComponent<Shootable>()?.Init(data.Precision, mainCamera);
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
        detector.GetComponent<Mergable>()?.Init(data.Type, data.InfluenceZone, data.NamingElements,
                data.Surface, data.Pattern, data.Colors, data.Extra, this, gameManager, canvas, mainCamera);
    }
}
