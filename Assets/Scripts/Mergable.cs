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
    private float detectionZoneFactor;
    private IngredientData.NameElements namingElements;
    private Texture2D surface;
    private Texture2D pattern;
    private Color[] colors;
    private Sprite extra;
    private IngredientInitializer ingredient;
    private GameObject gameManager;
    private Canvas canvas;
    private Camera mainCamera;
    private float detectionZone;
    private Dictionary<IngredientData.IngredientType, Mergable> ingredients;
    private List<Mergable> alreadyCollidingIngredients;
    private bool canGeneratePlanet;
    private Vector3 spawnPosition;

    public void Init(IngredientData.IngredientType type, float detectionZoneFactor,
            IngredientData.NameElements namingElements, Texture2D surface, Texture2D pattern,
            Color[] colors, Sprite extra, IngredientInitializer ingredient, GameObject gameManager, Canvas canvas, Camera mainCamera)
    {
        this.type = type;
        this.detectionZoneFactor = detectionZoneFactor;
        this.namingElements = namingElements;
        this.surface = surface;
        this.pattern = pattern;
        this.colors = colors;
        this.extra = extra;
        this.ingredient = ingredient;
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
        Sprite extra = ingredients[IngredientData.IngredientType.GASEOUS].extra;
        float averageMass = 0;
        float averageDrag = 0;
        float averageAngularDrag = 0;
        foreach(Mergable detector in ingredients.Values)
        {
            if(detector.ingredient.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
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
        detectionZone = ingredient.GetComponent<CircleCollider2D>().radius * detectionZoneFactor;
        GetComponent<CircleCollider2D>().radius = detectionZone;
    }

    private void LateUpdate()
    {
        ComputeLine();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!other.TryGetComponent<IngredientInitializer>(out IngredientInitializer collidingIngredient))
        {
            return;
        }
        if(!collidingIngredient.Detector.TryGetComponent<Mergable>(out Mergable mergableIngredient))
        {
            return;
        }

        IngredientData.IngredientType collidingType = mergableIngredient.type;
        foreach(Mergable alreadyCollidingIngredient in alreadyCollidingIngredients)
        {
            IngredientData.IngredientType alreadyCollidingType = alreadyCollidingIngredient.type;
            if(alreadyCollidingType != type &&
                    collidingType != type &&
                    alreadyCollidingType != collidingType)
            {
                canGeneratePlanet = true;
                Mergable[] ingredients = new Mergable[]{ mergableIngredient, alreadyCollidingIngredient, this };
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
            alreadyCollidingIngredients.Add(mergableIngredient);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(!other.TryGetComponent<IngredientInitializer>(out IngredientInitializer exitingIngredient))
        {
            return;
        }
        if(!exitingIngredient.Detector.TryGetComponent<Mergable>(out Mergable mergableIngredient))
        {
            return;
        }

        alreadyCollidingIngredients.Remove(mergableIngredient);
    }

    private void HandleGeneration()
    {
        // spawnPosition = Vector3.zero;
        foreach(Mergable detector in ingredients.Values)
        {
            spawnPosition += detector.transform.position;
            if(detector.ingredient.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
            {
                rb.velocity = Vector2.zero;
            }

            detector.GetComponent<Collider2D>().enabled = false;
            if(detector.ingredient.TryGetComponent<Collider2D>(out Collider2D collider))
            {
                collider.enabled = false;
            }
        }
        spawnPosition /= ingredients.Count;

        LeanTween.move(ingredients[IngredientData.IngredientType.SOLID].ingredient.gameObject, spawnPosition, 1f);
        LeanTween.move(ingredients[IngredientData.IngredientType.LIQUID].ingredient.gameObject, spawnPosition, 1f);
        LeanTween.move(ingredients[IngredientData.IngredientType.GASEOUS].ingredient.gameObject, spawnPosition, 1f)
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
            float x = Mathf.Cos(radAngle) * detectionZone;
            float y = Mathf.Sin(radAngle) * detectionZone;
            points[i] = transform.TransformPoint(new Vector2(x, y));
        }
        points[segmentsCount] = points[0];

        lineRenderer.positionCount = segmentsCount + 1;
        lineRenderer.SetPositions(points);
        lineRenderer.widthMultiplier = mainCamera.orthographicSize / 50;
    }
}
