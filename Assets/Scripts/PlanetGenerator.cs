using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class PlanetGenerator : MonoBehaviour
{
    [SerializeField] private IngredientInitializer ingredient = null;
    [SerializeField] private GameObject planetPrefab = null;
    [SerializeField] private NameTemplatesData nameTemplates = null;
    [SerializeField] private int segmentsCount = 30;

    public Dictionary<IngredientData.IngredientType, IngredientInitializer> Ingredients { get; set; }
    public bool CanGeneratePlanet { get; set; }

    private List<IngredientInitializer> alreadyCollidingIngredients;
    private Vector2 spawnPosition;
    private LineRenderer lineRenderer;
    private Camera mainCamera;

    private void Awake()
    {
        alreadyCollidingIngredients = new List<IngredientInitializer>();
        Ingredients = new Dictionary<IngredientData.IngredientType, IngredientInitializer>();
        lineRenderer = GetComponent<LineRenderer>();
        mainCamera = Camera.main;
    }

    private void Start()
    {
        spawnPosition = Vector2.zero;
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
        foreach(IngredientInitializer alreadyCollidingIngredient in alreadyCollidingIngredients)
        {
            if(alreadyCollidingIngredient.Data.Type != type &&
                    collidingIngredient.Data.Type != type &&
                    alreadyCollidingIngredient.Data.Type != collidingIngredient.Data.Type)
            {
                CanGeneratePlanet = true;
                IngredientInitializer[] ingredients = new IngredientInitializer[]{collidingIngredient, alreadyCollidingIngredient, ingredient};
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
        foreach(IngredientInitializer ingredient in Ingredients.Values)
        {
            spawnPosition += (Vector2) ingredient.transform.position;
            Rigidbody2D rb = ingredient.GetComponent<Rigidbody2D>();
            rb.velocity = Vector2.zero;

            foreach(Collider2D collider in ingredient.GetComponentsInChildren<Collider2D>())
            {
                collider.enabled = false;
            }
        }
        spawnPosition /= Ingredients.Count;

        LeanTween.move(Ingredients[IngredientData.IngredientType.SOLID].gameObject, spawnPosition, 1f);
        LeanTween.move(Ingredients[IngredientData.IngredientType.LIQUID].gameObject, spawnPosition, 1f);
        LeanTween.move(Ingredients[IngredientData.IngredientType.GASEOUS].gameObject, spawnPosition, 1f).setOnComplete(() => StartMergingAnimation());
    }

    private void StartMergingAnimation()
    {
        foreach(IngredientInitializer ingredient in Ingredients.Values)
        {
            ingredient.GetComponent<Animator>().SetTrigger("Merge");
        }
    }

    public void SpawnPlanet()
    {
        Texture2D surface = null;
        Texture2D pattern = null;
        Color[] colors = new Color[2];
        Texture2D extra = null;
        Rigidbody2D rb;
        float averageMass = 0;
        float averageDrag = 0;
        float averageAngularDrag = 0;

        surface = Ingredients[IngredientData.IngredientType.SOLID].Data.Surface;
        pattern = Ingredients[IngredientData.IngredientType.SOLID].Data.Pattern;
        colors = Ingredients[IngredientData.IngredientType.LIQUID].Data.Colors;
        extra = Ingredients[IngredientData.IngredientType.GASEOUS].Data.Extra;
        foreach(IngredientInitializer ingredient in Ingredients.Values)
        {
            rb = ingredient.GetComponent<Rigidbody2D>();
            averageMass += rb.mass;
            averageDrag += rb.drag;
            averageAngularDrag += rb.angularDrag;
        }
        averageMass /= Ingredients.Count;
        averageDrag /= Ingredients.Count;
        averageAngularDrag /= Ingredients.Count;

        GameObject generatedPlanet = Instantiate(planetPrefab, spawnPosition, Quaternion.identity);
        // generatedPlanet.GetComponent<Identifiable>().Name = ComputeRandomName();
        // Debug.Log(generatedPlanet.GetComponent<Identifiable>().Name);
        SpriteRenderer renderer = generatedPlanet.GetComponent<SpriteRenderer>();
        renderer.material.SetTexture("_MainTex", surface);
        renderer.material.SetTexture("_Pattern", pattern);
        renderer.material.SetColor("_ColorA", colors[0]);
        renderer.material.SetColor("_ColorB", colors[1]);
        renderer.material.SetTexture("_Extra", extra);

        rb = generatedPlanet.GetComponent<Rigidbody2D>();
        rb.mass = averageMass;
        rb.drag = averageDrag;
        rb.angularDrag = averageAngularDrag;
        generatedPlanet.GetComponent<CircleCollider2D>().radius = surface.width / 200;

        generatedPlanet.transform.localScale = Vector3.zero;
        generatedPlanet.GetComponent<SpringJoint2D>().enabled = false;
    }

    private string ComputeRandomName()
    {
        string template = nameTemplates.Templates[Random.Range(0, nameTemplates.Templates.Count)];
        List<IngredientData.Noun> nouns = new List<IngredientData.Noun>();
        nouns.Add(null);

        while(template.Contains("§"))
        {
            string element = GetTextBetweenTags(template, "§", "§");

            int index = 0;
            if(element.Contains("#"))
            {
                index = int.Parse(element.Substring(element.Length - 1));
                element = element.Remove(element.Length - 2);
            }
            if(element.Contains(","))
            {
                element = GetRandomChoice(element);
            }

            IngredientData.Noun noun = null;
            switch(element)
            {
                case "planet synonym" :
                    noun = Ingredients[IngredientData.IngredientType.SOLID].Data.NamingElements.planetSynonym;
                    break;
                case "concept" :
                    noun = Ingredients[IngredientData.IngredientType.SOLID].Data.NamingElements.concept;
                    break;
                case "first name" :
                    noun = Ingredients[IngredientData.IngredientType.LIQUID].Data.NamingElements.firstName;
                    break;
                case "nickname" :
                    noun = Ingredients[IngredientData.IngredientType.SOLID].Data.NamingElements.nickname;
                    break;
                case "matriculation" :
                    noun = Ingredients[IngredientData.IngredientType.GASEOUS].Data.NamingElements.matriculation;
                    break;
            }

            if(index != 0)
            {
                nouns.Insert(index, noun);
            }

            element = noun.word;
            template = ReplaceTextBetweenTags(template, element, "§", "§");
        }

        while(template.Contains("+"))
        {
            string element = GetTextBetweenTags(template, "+", "+");

            IngredientData.Qualifier qualifier = null;
            int index = 0;
            index = int.Parse(element.Substring(element.Length - 1).ToString());
            element = element.Remove(element.Length - 2);
            switch(element)
            {
                case "style adjective" :
                    qualifier = Ingredients[IngredientData.IngredientType.GASEOUS].Data.NamingElements.styleAdjective;
                    break;
                case "personality adjective" :
                    qualifier = Ingredients[IngredientData.IngredientType.LIQUID].Data.NamingElements.personalityAdjective;
                    break;
                case "color" :
                    qualifier = Ingredients[IngredientData.IngredientType.LIQUID].Data.NamingElements.color;
                    break;
            }
            if(index == 0)
            {
                element = qualifier.singularFeminine;
            }
            else
            {
                element = MatchQualifier(qualifier, nouns[index]);
            }

            template = ReplaceTextBetweenTags(template, element, "+", "+");
        }

        while(template.Contains("<"))
        {
            string article = GetTextBetweenTags(template, "<", ">");
            int index = int.Parse(article.Substring(article.Length - 1));
            template = ReplaceTextBetweenTags(template, MatchArticle(article, nouns[index]), "<", ">");
        }

        while(template.Contains("{"))
        {
            string article = template.Substring(template.IndexOf("{") + 4, template.IndexOf("}") - 1);
            template = ReplaceTextBetweenTags(template, MatchPossessive(article), "{", "}");
        }

        return template.Substring(0, 1).ToUpper() + template.Substring(1);
    }

    private string GetTextBetweenTags(string context, string tag1, string tag2)
    {
        int startIndex = context.IndexOf(tag1) + 1;
        int length = context.Substring(startIndex).IndexOf(tag2);
        return context.Substring(startIndex, length);
    }

    private string ReplaceTextBetweenTags(string context, string replacement, string tag1, string tag2)
    {
        int startIndex = context.IndexOf(tag1);
        int length = context.Substring(startIndex + 1).IndexOf(tag2) + 2;
        return context.Replace(context.Substring(startIndex, length), replacement);
    }

    private string MatchQualifier(IngredientData.Qualifier qualifier, IngredientData.Noun noun)
    {
        if(qualifier.invariable != "")
        {
            return qualifier.invariable;
        }

        if(noun == null)
        {
            return qualifier.singularFeminine;
        }

        if(noun.genre == IngredientData.Genre.FEMININE)
        {
            if(noun.plurality == IngredientData.Plurality.SINGULAR)
            {
                return qualifier.singularFeminine;
            }
            return qualifier.pluralFeminine;
        }

        if(noun.plurality == IngredientData.Plurality.SINGULAR)
        {
            return qualifier.singularMasculine;
        }
        return qualifier.pluralMasculine;
    }

    private string MatchArticle(string article, IngredientData.Noun noun)
    {
        if(noun == null)
        {
            return "la";
        }
        bool isUndefined = article.StartsWith("u");
        if(noun.plurality == IngredientData.Plurality.SINGULAR)
        {
            if(noun.genre == IngredientData.Genre.FEMININE)
            {
                if(isUndefined)
                {
                    return "une";
                }
                return noun.hasContractedArticle ? "l’" : "la";
            }
            if(isUndefined)
            {
                return "un";
            }
            return noun.hasContractedArticle ? "l’" : "le";
        }

        return isUndefined ? "des" : "les";
    }

    private string MatchPossessive(string article)
    {
        switch(article)
        {
            case "un" :
                return "d’un";
            case "une" :
                return "d’une";
            case "le" :
                return "du";
            case "la" :
                return "de la";
            case "les" :
                return "des";
            default :
                return "de";
        }
    }

    private string GetRandomChoice(string context)
    {
        string[] choices = context.Split(',');
        return choices[Random.Range(0, choices.Length)];
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
