using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ingredient", menuName = "Ingredient")]
public class IngredientData : AData
{
    [SerializeField] private string _ingredientName = "";
    [SerializeField] private NameElements _namingElements = null;
    [SerializeField] private Sprite _bodySprite = null;
    [SerializeField] private IngredientType _type = IngredientType.SOLID;
    [SerializeField] private Texture2D _surface = null;
    [SerializeField] private Texture2D _pattern = null;
    [SerializeField] private Color[] _colors = null;
    [SerializeField] private Texture2D _extra = null;
    [SerializeField] [Range(0, 100f)] private float _mass = 0;
    [SerializeField] [Range(0, 100f)] private float _drag = 0;
    [SerializeField] [Range(0, 100f)] private float _angularDrag = 0;
    [SerializeField] private PhysicsMaterial2D _physicsMaterial = null;
    [SerializeField] [Range(0, 1f)] private float _precision = 1f;
    [SerializeField] [Range(0, 10f)] private float _influenceZone = 3f;
    [SerializeField] private RuntimeAnimatorController _animatorController = null;

    public string IngredientName { get => _ingredientName; }
    public NameElements NamingElements { get => _namingElements; }
    public Sprite BodySprite { get => _bodySprite; }
    public IngredientType Type { get => _type; }
    public Texture2D Surface { get => _surface; }
    public Texture2D Pattern { get => _pattern; }
    public Color[] Colors { get => _colors; }
    public Texture2D Extra { get => _extra; }
    public float Mass { get => _mass; }
    public float Drag { get => _drag; }
    public float AngularDrag { get => _angularDrag; }
    public PhysicsMaterial2D PhysicsMaterial { get => _physicsMaterial; }
    public float Precision { get => _precision; }
    public float InfluenceZone { get => _influenceZone; }
    public RuntimeAnimatorController AnimatorController { get => _animatorController; }

    [System.Serializable]
    public class NameElements
    {
        public Noun planetSynonym;
        public Noun concept;
        public Noun firstName;
        public Noun nickname;
        public Noun matriculation;
        public Qualifier styleAdjective;
        public Qualifier personalityAdjective;
        public Qualifier color;
    }

    public abstract class Element
    {
        public bool hasContractedArticle;
    }

    [System.Serializable]
    public class Noun : Element
    {
        public string word;
        public Genre genre;
        public Plurality plurality;
    }

    [System.Serializable]
    public class Qualifier : Element
    {
        public string invariable;
        public string singularFeminine;
        public string singularMasculine;
        public string pluralFeminine;
        public string pluralMasculine;
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
