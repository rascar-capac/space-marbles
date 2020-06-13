using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class StarInitializer : A2DDataInitializer<StarData>
{
    [SerializeField] private GameObject body = null;

    public override void Init(GameObject gameManager, Canvas canvas, Camera mainCamera)
    {
        base.Init(gameManager, canvas, mainCamera);

        if(body.TryGetComponent<SpriteRenderer>(out SpriteRenderer renderer))
        {
            renderer.sprite = data.BodySprites[Random.Range(0, data.BodySprites.Count)];
        }

        GetComponent<Orbitable>()?.Init(data);
    }
}
