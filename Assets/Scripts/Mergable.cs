using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer), typeof(CircleCollider2D))]
public class Mergable : MonoBehaviour
{
    public IngredientData.NameElements NamingElements => namingElements;

    [SerializeField] private PlanetInitializer planetPrefab = null;
    [SerializeField] private TextAsset nameData = null;
    [SerializeField] private int segmentsCount = 30;
    private LineRenderer lineRenderer;
    private IngredientData.IngredientType type;
    private float influenceZone;
    private IngredientData.NameElements namingElements;
    private Texture2D surface;
    private Texture2D pattern;
    private Color[] colors;
    private Texture2D extra;
    private GameObject gameManager;
    private Canvas canvas;
    private Camera mainCamera;
    private Dictionary<IngredientData.IngredientType, Mergable> ingredients;
    private List<Mergable> alreadyCollidingIngredients;
    private bool canGeneratePlanet;
    private Vector3 spawnPosition;

    public void Init(IngredientData.IngredientType type, float influenceZone,
            IngredientData.NameElements namingElements, Texture2D surface, Texture2D pattern,
            Color[] colors, Texture2D extra, GameObject gameManager, Canvas canvas, Camera mainCamera)
    {
        this.type = type;
        this.influenceZone = influenceZone;
        this.namingElements = namingElements;
        this.surface = surface;
        this.pattern = pattern;
        this.colors = colors;
        this.extra = extra;
        this.gameManager = gameManager;
        this.canvas = canvas;
        this.mainCamera = mainCamera;
    }

    public void SpawnPlanet()
    {
        if(!canGeneratePlanet)
        {
            return;
        }

        string planetName = ComputeRandomName();
        Texture2D surface = ingredients[IngredientData.IngredientType.SOLID].surface;
        Texture2D pattern = ingredients[IngredientData.IngredientType.SOLID].pattern;
        Color[] colors = ingredients[IngredientData.IngredientType.LIQUID].colors;
        Texture2D extra = ingredients[IngredientData.IngredientType.GASEOUS].extra;
        float averageMass = 0;
        float averageDrag = 0;
        float averageAngularDrag = 0;
        foreach(Mergable ingredient in ingredients.Values)
        {
            if(ingredient.transform.parent.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
            {
                averageMass += rb.mass;
                averageDrag += rb.drag;
                averageAngularDrag += rb.angularDrag;
            }
        }
        averageMass /= ingredients.Count;
        averageDrag /= ingredients.Count;
        averageAngularDrag /= ingredients.Count;
        PlanetData data = new PlanetData(planetName, surface, pattern, colors, extra,
                averageMass, averageDrag, averageAngularDrag);

        PlanetInitializer generatedPlanet = Instantiate(planetPrefab, spawnPosition, Quaternion.identity);
        generatedPlanet.InitData(data);
        generatedPlanet.Init(gameManager, canvas, mainCamera);
    }

    public void DestroyIngredient()
    {
        gameManager.GetComponent<IngredientSpawner>()?.DestroySpawnedObject(GetComponentInParent<IngredientInitializer>());
    }

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        alreadyCollidingIngredients = new List<Mergable>();
        ingredients = new Dictionary<IngredientData.IngredientType, Mergable>();
        canGeneratePlanet = false;
    }

    private void Start()
    {
        GetComponent<CircleCollider2D>().radius = influenceZone;
    }

    private void LateUpdate()
    {
        ComputeLine();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!other.TryGetComponent<Mergable>(out Mergable collidingIngredient))
        {
            return;
        }

        IngredientData.IngredientType collidingType = collidingIngredient.type;
        foreach(Mergable alreadyCollidingIngredient in alreadyCollidingIngredients)
        {
            IngredientData.IngredientType alreadyCollidingType = alreadyCollidingIngredient.type;
            if(alreadyCollidingType != type &&
                    collidingType != type &&
                    alreadyCollidingType != collidingType)
            {
                canGeneratePlanet = true;
                Mergable[] ingredients =
                        new Mergable[]{collidingIngredient, alreadyCollidingIngredient, this};
                foreach(Mergable ingredient in ingredients)
                {
                    switch(ingredient.type)
                    {
                        case IngredientData.IngredientType.SOLID :
                            this.ingredients.Add(IngredientData.IngredientType.SOLID, ingredient);
                            break;
                        case IngredientData.IngredientType.LIQUID :
                            this.ingredients.Add(IngredientData.IngredientType.LIQUID, ingredient);
                            break;
                        default :
                            this.ingredients.Add(IngredientData.IngredientType.GASEOUS, ingredient);
                            break;
                    }
                }
                break;
            }
        }
        if(canGeneratePlanet)
        {
            HandleGeneration();
        }
        else
        {
            alreadyCollidingIngredients.Add(collidingIngredient);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(!other.TryGetComponent<Mergable>(out Mergable exitingIngredient))
        {
            return;
        }

        alreadyCollidingIngredients.Remove(exitingIngredient);
    }

    private void HandleGeneration()
    {
        // spawnPosition = Vector3.zero;
        foreach(Mergable ingredient in ingredients.Values)
        {
            spawnPosition += ingredient.transform.position;
            if(ingredient.transform.parent.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
            {
                rb.velocity = Vector2.zero;
            }

            foreach(Collider2D collider in ingredient.GetComponentsInChildren<Collider2D>())
            {
                collider.enabled = false;
            }
        }
        spawnPosition /= ingredients.Count;

        LeanTween.move(ingredients[IngredientData.IngredientType.SOLID].gameObject, spawnPosition, 1f);
        LeanTween.move(ingredients[IngredientData.IngredientType.LIQUID].gameObject, spawnPosition, 1f);
        LeanTween.move(ingredients[IngredientData.IngredientType.GASEOUS].gameObject, spawnPosition, 1f)
                .setOnComplete(() => StartMergingAnimation());
    }

    private void StartMergingAnimation()
    {
        foreach(Mergable ingredient in ingredients.Values)
        {
            ingredient.GetComponentInParent<AnimMergable>()?.Merge();
        }
    }

    private string ComputeRandomName()
    {
        Dictionary<string, string[]> categoryElements = TextGenerator.FetchCategories(nameData.text);
        string template = TextGenerator.PickRandom("template", categoryElements);
        return TextGenerator.Format(template, ingredients);
    }

    private void ComputeLine()
    {
        Vector3[] points = new Vector3[segmentsCount + 1];
        for(int i = 0 ; i < segmentsCount ; i++)
        {
            float radAngle = 360 * (float) i / (float) segmentsCount * Mathf.Deg2Rad;
            float x = Mathf.Cos(radAngle) * influenceZone;
            float y = Mathf.Sin(radAngle) * influenceZone;
            points[i] = transform.TransformPoint(new Vector2(x, y));
        }
        points[segmentsCount] = points[0];

        lineRenderer.positionCount = segmentsCount + 1;
        lineRenderer.SetPositions(points);
        lineRenderer.widthMultiplier = mainCamera.orthographicSize / 50;
    }
}
