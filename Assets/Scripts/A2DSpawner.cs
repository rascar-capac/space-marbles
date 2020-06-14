using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class A2DSpawner<T, U> : ASpawner<T, U>
        where T : A2DDataInitializer<U>
{
    [SerializeField] private Collider2D spawnArea = null;
    [SerializeField] private int count = 0;
    [SerializeField] private float minGapBetweenObjects = 30f;
    [SerializeField] private bool hasInitialMotion = false;
    [SerializeField] private float maxInitialForce = 10f;
    [SerializeField] private Canvas canvas = null;
    private Camera mainCamera;
    private CircleCollider2D circularArea;
    private bool isCircle;
    private float spawnX;
    private float spawnY;

    protected override void Awake()
    {
        base.Awake();
        mainCamera = Camera.main;
        spawnX = 0;
        spawnY = 0;
    }

    private void Start()
    {
        isCircle = spawnArea.TryGetComponent<CircleCollider2D>(out circularArea);
        if(isCircle)
        {
            spawnX = - spawnArea.bounds.extents.x;
            spawnY = spawnArea.bounds.extents.y;
        }

        for(int i = 0 ; i < count ; i++)
        {
            SpawnObject();
        }
    }

    protected override T SpawnObject()
    {
        T newObject = base.SpawnObject();

        newObject.Init(this.gameObject, canvas, mainCamera);

        Vector2 randomPosition = Vector2.zero;
        bool isPositionValid = false;
        while(!isPositionValid)
        {
            isPositionValid = true;
            if(isCircle)
            {
                randomPosition = Random.insideUnitCircle * circularArea.radius;
            }
            else
            {
                randomPosition = new Vector2(
                    Random.Range(- spawnX, spawnX),
                    Random.Range(- spawnY, spawnY)
                );
            }

            if(minGapBetweenObjects != 0)
            {
                foreach(T neighbor in spawnedObjects)
                {
                    if(Vector2.Distance(randomPosition, neighbor.transform.position) < minGapBetweenObjects)
                    {
                        isPositionValid = false;
                        break;
                    }
                }
            }
        }
        newObject.transform.position = randomPosition;

        if(hasInitialMotion)
        {
            newObject.GetComponent<Rigidbody2D>().AddForce(maxInitialForce * Random.insideUnitCircle, ForceMode2D.Impulse);
        }

        return newObject;
    }
}
