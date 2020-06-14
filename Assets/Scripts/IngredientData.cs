using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ingredient")]
public class IngredientData : ScriptableObject
{
    public string IngredientName => ingredientName;
    public Sprite BodySprite => bodySprite;
    public float Mass => mass;
    public float Drag => drag;
    public float AngularDrag => angularDrag;
    public PhysicsMaterial2D PhysicsMaterial => physicsMaterial;
    public float Precision => precision;
    public RuntimeAnimatorController AnimatorController => animatorController;
    public IngredientType Type => type;
    public float InfluenceZone => influenceZone;
    public NameElements NamingElements => namingElements;
    public Texture2D Surface => surface;
    public Texture2D Pattern => pattern;
    public Color[] Colors => colors;
    public Texture2D Extra => extra;

    [SerializeField] private string ingredientName = "";
    [SerializeField] private Sprite bodySprite = null;
    [Space] [Space]
    [SerializeField] [Range(0, 100f)] private float mass = 0;
    [SerializeField] [Range(0, 100f)] private float drag = 0;
    [SerializeField] [Range(0, 100f)] private float angularDrag = 0;
    [SerializeField] private PhysicsMaterial2D physicsMaterial = null;
    [SerializeField] [Range(0, 1f)] private float precision = 1f;
    [Space] [Space]
    [SerializeField] private RuntimeAnimatorController animatorController = null;

    [Header("Planet generation")]
    [SerializeField] private IngredientType type = IngredientType.SOLID;
    [SerializeField] [Range(0, 10f)] private float influenceZone = 3f;
    [Space]
    [SerializeField] private NameElements namingElements = null;
    [Space]
    [SerializeField] private Texture2D surface = null;
    [SerializeField] private Texture2D pattern = null;
    [SerializeField] private Color[] colors = null;
    [SerializeField] private Texture2D extra = null;

    [System.Serializable]
    public class NameElements
    {
        public Noun PlanetSynonym => planetSynonym;
        public Noun Concept => concept;
        public Noun FirstName => firstName;
        public Noun Nickname => nickname;
        public Noun Matriculation => matriculation;
        public Qualifier StyleAdjective => styleAdjective;
        public Qualifier PersonalityAdjective => personalityAdjective;
        public Qualifier Color => color;

        [SerializeField] private Noun planetSynonym = null;
        [SerializeField] private Noun concept = null;
        [SerializeField] private Noun firstName = null;
        [SerializeField] private Noun nickname = null;
        [SerializeField] private Noun matriculation = null;
        [SerializeField] private Qualifier styleAdjective = null;
        [SerializeField] private Qualifier personalityAdjective = null;
        [SerializeField] private Qualifier color = null;
    }

    public abstract class Element
    {
        public bool HasContractedArticle => hasContractedArticle;

        [SerializeField] private bool hasContractedArticle = false;
    }

    [System.Serializable]
    public class Noun : Element
    {
        public string Word => word;
        public Genre Genre => genre;
        public Plurality Plurality => plurality;

        [SerializeField] private string word = null;
        [SerializeField] private Genre genre = Genre.FEMININE;
        [SerializeField] private Plurality plurality = Plurality.SINGULAR;
    }

    [System.Serializable]
    public class Qualifier : Element
    {
        public string Invariable => invariable;
        public string SingularFeminine => singularFeminine;
        public string SingularMasculine => singularMasculine;
        public string PluralFeminine => pluralFeminine;
        public string PluralMasculine => pluralMasculine;

        [SerializeField] private string invariable = null;
        [SerializeField] private string singularFeminine = null;
        [SerializeField] private string singularMasculine = null;
        [SerializeField] private string pluralFeminine = null;
        [SerializeField] private string pluralMasculine = null;
    }

    public enum Genre
    {
        FEMININE,
        MASCULINE
    }

    public enum Plurality
    {
        SINGULAR,
        PLURAL
    }

    public enum IngredientType
    {
        SOLID,
        LIQUID,
        GASEOUS
    }
}
