using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer), typeof(CircleCollider2D))]
public class Mergable : MonoBehaviour
{
    [SerializeField] private IngredientInitializer ingredient = null;
    [SerializeField] private PlanetInitializer planetPrefab = null;
    [SerializeField] private TextAsset nameData = null;
    [SerializeField] private int segmentsCount = 30;
    private LineRenderer lineRenderer;
    private GameObject gameManager;
    private Canvas canvas;
    private Camera mainCamera;
    private Dictionary<IngredientData.IngredientType, IngredientInitializer> ingredients;
    private List<IngredientInitializer> alreadyCollidingIngredients;
    private bool canGeneratePlanet;
    private Vector3 spawnPosition;

    public void Init(float influenceZone, GameObject gameManager, Canvas canvas, Camera mainCamera)
    {
        GetComponent<CircleCollider2D>().radius = influenceZone;
        this.gameManager = gameManager;
        this.canvas = canvas;
        this.mainCamera = mainCamera;
    }

    public void SpawnPlanet()
    {
        if(canGeneratePlanet)
        {
            string planetName = ComputeRandomName();
            Texture2D surface = ingredients[IngredientData.IngredientType.SOLID].Data.Surface;
            Texture2D pattern = ingredients[IngredientData.IngredientType.SOLID].Data.Pattern;
            Color[] colors = ingredients[IngredientData.IngredientType.LIQUID].Data.Colors;
            Texture2D extra = ingredients[IngredientData.IngredientType.GASEOUS].Data.Extra;
            float averageMass = 0;
            float averageDrag = 0;
            float averageAngularDrag = 0;
            foreach(IngredientInitializer ingredient in ingredients.Values)
            {
                Rigidbody2D rb = ingredient.GetComponent<Rigidbody2D>();
                averageMass += rb.mass;
                averageDrag += rb.drag;
                averageAngularDrag += rb.angularDrag;
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
    }

    public void DestroyIngredient()
    {
        gameManager.GetComponent<IngredientSpawner>()?.DestroySpawnedObject(ingredient);
    }

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        alreadyCollidingIngredients = new List<IngredientInitializer>();
        ingredients = new Dictionary<IngredientData.IngredientType, IngredientInitializer>();
        canGeneratePlanet = false;
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
                canGeneratePlanet = true;
                IngredientInitializer[] ingredients =
                        new IngredientInitializer[]{collidingIngredient, alreadyCollidingIngredient, ingredient};
                foreach(IngredientInitializer ingredient in ingredients)
                {
                    switch(ingredient.Data.Type)
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
        foreach(IngredientInitializer ingredient in ingredients.Values)
        {
            spawnPosition += ingredient.transform.position;
            ingredient.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

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
        foreach(IngredientInitializer ingredient in ingredients.Values)
        {
            ingredient.GetComponent<AnimMergable>().Merge();
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
