using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private List<GO> GameObjects = null;
    // [SerializeField] private List<SO> ScriptableObjects = null;
    [SerializeField] private Collider2D spawnArea = null;
    [SerializeField] private float maxInitialForce = 10f;

    public PanelUpdater ElementIDPanel { get; set; }

    public List<List<GameObject>> SpawnedObjects { get; set; }

    private void Awake()
    {
        ElementIDPanel = FindObjectOfType<PanelUpdater>();
    }

    private void Start()
    {
        bool isCircle = spawnArea.TryGetComponent<CircleCollider2D>(out CircleCollider2D circularArea);
        float spawnX = 0;
        float spawnY = 0;
        if(isCircle)
        {
            spawnX = - spawnArea.bounds.extents.x;
            spawnY = spawnArea.bounds.extents.y;
        }

        SpawnedObjects = new List<List<GameObject>>();
        for(int i = 0 ; i < GameObjects.Count ; i++)
        {
            SpawnedObjects.Add(new List<GameObject>());
            for(int j = 0 ; j < GameObjects[i].number ; j++)
            {
                GameObject randomPrefab = GameObjects[i].prefabs[Random.Range(0, GameObjects[i].prefabs.Count)];

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

                    if(GameObjects[i].minGapBetweenObjects != 0)
                    {
                        foreach(GameObject neighbor in SpawnedObjects[i])
                        {
                            if(Vector2.Distance(randomPosition, neighbor.transform.position) < GameObjects[i].minGapBetweenObjects)
                            {
                                isPositionValid = false;
                                break;
                            }
                        }
                    }
                }

                GameObject instance = Instantiate(randomPrefab, randomPosition, Quaternion.identity);
                SpawnedObjects[i].Add(instance);

                if(!GameObjects[i].isKinematic)
                {
                    instance.GetComponent<Rigidbody2D>().AddForce(maxInitialForce * Random.insideUnitCircle, ForceMode2D.Impulse);
                }
            }
        }

        // foreach(SO so in ScriptableObjects)
        // {
        //     for(int i = 0 ; i < so.number ; i++)
        //     {
        //         Vector2 randomPosition = Random.insideUnitCircle * playground.radius;
        //         GameObject instance = Instantiate(so.prefab, randomPosition, Quaternion.identity);
        //         instance.GetComponent<Rigidbody2D>().velocity = Random.insideUnitCircle * initialVelocityIntensity;
        //     }
        // }
    }

    public void DestroySpawnedObject<T>(GameObject spawnedObject) where T : MonoBehaviour
    {
        List<GameObject> SpawnedObjectList = FindSpawnedObjectListOfType<T>();
        SpawnedObjectList.Remove(spawnedObject);
        Destroy(spawnedObject);
    }

    public List<GameObject> FindSpawnedObjectListOfType<T>() where T : MonoBehaviour
    {
        List<GameObject> spawnedObjectList = new List<GameObject>();
        foreach(List<GameObject> list in SpawnedObjects)
        {
            if(list[0].TryGetComponent<T>(out T element))
            {
                spawnedObjectList = list;
            }
        }
        return spawnedObjectList;
    }

    [System.Serializable]
    private class GO
    {
        public int number = 0;
        public List<GameObject> prefabs = null;
        public bool isKinematic = false;
        public float minGapBetweenObjects = 30f;
    }

    // [System.Serializable]
    // private class SO
    // {
    //     public int number = 0;
    //     public GameObject prefab = null;
    //     public List<IData> datas = null;
    // }
}
