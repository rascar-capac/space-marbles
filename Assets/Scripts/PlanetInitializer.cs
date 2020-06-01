using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetInitializer : A2DDataInitializer<PlanetData>
{
    public override void Init(Canvas canvas, Camera mainCamera)
    {
        base.Init(canvas, mainCamera);

        GetComponent<Identifiable>()?.Init(data.PlanetName, canvas, mainCamera);
        transform.localScale = Vector3.zero;
        if(TryGetComponent<SpriteRenderer>(out SpriteRenderer renderer))
        {
            renderer.material.SetTexture("_MainTex", data.Surface);
            renderer.material.SetTexture("_Pattern", data.Pattern);
            renderer.material.SetColor("_ColorA", data.Colors[0]);
            renderer.material.SetColor("_ColorB", data.Colors[1]);
            renderer.material.SetTexture("_Extra", data.Extra);
        }
        if(TryGetComponent<Rigidbody2D>(out Rigidbody2D rigidbody))
        {
            rigidbody.mass = data.Mass;
            rigidbody.drag = data.Drag;
            rigidbody.angularDrag = data.AngularDrag;
        }
        if(TryGetComponent<CircleCollider2D>(out CircleCollider2D collider))
        {
            collider.radius = data.Surface.width / 200;
        }
        if(TryGetComponent<SpringJoint2D>(out SpringJoint2D joint))
        {
            joint.enabled = false;
        }
    }
}
