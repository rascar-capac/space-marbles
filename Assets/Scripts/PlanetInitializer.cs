using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetInitializer : A2DDataInitializer<PlanetData>
{
    [SerializeField] private GameObject body = null;
    [SerializeField] private GameObject extra = null;

    public override void Init(GameObject gameManager, Canvas canvas, Camera mainCamera)
    {
        base.Init(gameManager, canvas, mainCamera);

        GetComponent<Identifiable>()?.Init(data.PlanetName, canvas, mainCamera);
        GetComponent<OrbitFollower>()?.Init(gameManager);
        transform.localScale = Vector3.zero;
        if(TryGetComponent<Rigidbody2D>(out Rigidbody2D rigidbody))
        {
            rigidbody.mass = data.Mass;
            rigidbody.drag = data.Drag;
            rigidbody.angularDrag = data.AngularDrag;
        }
        if(TryGetComponent<SpringJoint2D>(out SpringJoint2D joint))
        {
            joint.enabled = false;
        }
        if(body.TryGetComponent<SpriteRenderer>(out SpriteRenderer renderer))
        {
            renderer.sprite = data.Surface;
            renderer.material.SetTexture("_Pattern", data.Pattern);
            renderer.material.SetColor("_ColorA", data.Colors[0]);
            renderer.material.SetColor("_ColorB", data.Colors[1]);
        }
        if(extra.TryGetComponent<SpriteRenderer>(out SpriteRenderer extraRenderer))
        {
            extraRenderer.sprite = data.Extra;
            extraRenderer.color = data.Colors[1];
        }
    }
}
