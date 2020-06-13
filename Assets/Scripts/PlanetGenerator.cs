using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer), typeof(CircleCollider2D))]
public class PlanetGenerator : MonoBehaviour
{
    [SerializeField] private IngredientInitializer ingredient = null;
    [SerializeField] private PlanetInitializer planetPrefab = null;
    [SerializeField] private int segmentsCount = 30;
    [SerializeField] private TextAsset nameData = null;

    public Dictionary<IngredientData.IngredientType, IngredientInitializer> Ingredients { get; set; }
    public bool CanGeneratePlanet { get; set; }

    private List<IngredientInitializer> alreadyCollidingIngredients;
    private Vector3 spawnPosition;
    private LineRenderer lineRenderer;
    private GameObject gameManager;
    private Camera mainCamera;
    private Canvas canvas;

    public void Init(float influenceZone, GameObject gameManager, Canvas canvas, Camera mainCamera)
    {
        GetComponent<CircleCollider2D>().radius = influenceZone;
        this.gameManager = gameManager;
        this.canvas = canvas;
        this.mainCamera = mainCamera;
    }

    public void SpawnPlanet()
    {
        string planetName = ComputeRandomName();
        Texture2D surface = Ingredients[IngredientData.IngredientType.SOLID].Data.Surface;
        Texture2D pattern = Ingredients[IngredientData.IngredientType.SOLID].Data.Pattern;
        Color[] colors = Ingredients[IngredientData.IngredientType.LIQUID].Data.Colors;
        Texture2D extra = Ingredients[IngredientData.IngredientType.GASEOUS].Data.Extra;
        float averageMass = 0;
        float averageDrag = 0;
        float averageAngularDrag = 0;
        foreach(IngredientInitializer ingredient in Ingredients.Values)
        {
            Rigidbody2D rb = ingredient.GetComponent<Rigidbody2D>();
            averageMass += rb.mass;
            averageDrag += rb.drag;
            averageAngularDrag += rb.angularDrag;
        }
        averageMass /= Ingredients.Count;
        averageDrag /= Ingredients.Count;
        averageAngularDrag /= Ingredients.Count;
        PlanetData data = new PlanetData(planetName, surface, pattern, colors, extra,
                averageMass, averageDrag, averageAngularDrag);

        PlanetInitializer generatedPlanet = Instantiate(planetPrefab, spawnPosition, Quaternion.identity);
        generatedPlanet.InitData(data);
        generatedPlanet.Init(gameManager, canvas, mainCamera);
    }

    private void Awake()
    {
        alreadyCollidingIngredients = new List<IngredientInitializer>();
        Ingredients = new Dictionary<IngredientData.IngredientType, IngredientInitializer>();
        lineRenderer = GetComponent<LineRenderer>();
        CanGeneratePlanet = false;
    }

    private void LateUpdate()
    {
        ComputeLine();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        IngredientInitializer collidingIngredient = other.GetComponentInParent<IngredientInitializer>();
        if(!collidingIngredient)
        {
            return;
        }

        IngredientData.IngredientType type = ingredient.Data.Type;
        IngredientData.IngredientType collidingType = collidingIngredient.Data.Type;
        foreach(IngredientInitializer alreadyCollidingIngredient in alreadyCollidingIngredients)
        {
            IngredientData.IngredientType alreadyCollidingType = alreadyCollidingIngredient.Data.Type;
            if(alreadyCollidingType != type &&
                    collidingType != type &&
                    alreadyCollidingType != collidingType)
            {
                CanGeneratePlanet = true;
                IngredientInitializer[] ingredients =
                        new IngredientInitializer[]{collidingIngredient, alreadyCollidingIngredient, ingredient};
                foreach(IngredientInitializer ingredient in ingredients)
                {
                    switch(ingredient.Data.Type)
                    {
                        case IngredientData.IngredientType.SOLID :
                            Ingredients.Add(IngredientData.IngredientType.SOLID, ingredient);
                            break;
                        case IngredientData.IngredientType.LIQUID :
                            Ingredients.Add(IngredientData.IngredientType.LIQUID, ingredient);
                            break;
                        default :
                            Ingredients.Add(IngredientData.IngredientType.GASEOUS, ingredient);
                            break;
                    }
                }
                break;
            }
        }
        if(CanGeneratePlanet)
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
        IngredientInitializer exitingIngredient = other.GetComponentInParent<IngredientInitializer>();
        if(!exitingIngredient)
        {
            return;
        }

        alreadyCollidingIngredients.Remove(exitingIngredient);
    }

    private void HandleGeneration()
    {
        // spawnPosition = Vector3.zero;
        foreach(IngredientInitializer ingredient in Ingredients.Values)
        {
            spawnPosition += ingredient.transform.position;
            ingredient.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

            foreach(Collider2D collider in ingredient.GetComponentsInChildren<Collider2D>())
            {
                collider.enabled = false;
            }
        }
        spawnPosition /= Ingredients.Count;

        LeanTween.move(Ingredients[IngredientData.IngredientType.SOLID].gameObject, spawnPosition, 1f);
        LeanTween.move(Ingredients[IngredientData.IngredientType.LIQUID].gameObject, spawnPosition, 1f);
        LeanTween.move(Ingredients[IngredientData.IngredientType.GASEOUS].gameObject, spawnPosition, 1f)
                .setOnComplete(() => StartMergingAnimation());
    }

    private void StartMergingAnimation()
    {
        foreach(IngredientInitializer ingredient in Ingredients.Values)
        {
            ingredient.GetComponent<Animator>().SetTrigger("Merge");
        }
    }

    private string ComputeRandomName()
    {
        Dictionary<string, string[]> categoryElements = TextGenerator.FetchCategories(nameData.text);
        string template = TextGenerator.PickRandom("template", categoryElements);
        return TextGenerator.Format(template, Ingredients);
    }

    private void ComputeLine()
    {
        Vector3[] points = new Vector3[segmentsCount + 1];
        for(int i = 0 ; i < segmentsCount ; i++)
        {
            float radAngle = 360 * (float) i / (float) segmentsCount * Mathf.Deg2Rad;
            float x = Mathf.Cos(radAngle) * ingredient.Data.InfluenceZone;
            float y = Mathf.Sin(radAngle) * ingredient.Data.InfluenceZone;
            points[i] = transform.TransformPoint(new Vector2(x, y));
        }
        points[segmentsCount] = points[0];

        lineRenderer.positionCount = segmentsCount + 1;
        lineRenderer.SetPositions(points);
        lineRenderer.widthMultiplier = mainCamera.orthographicSize / 50;
    }
}
