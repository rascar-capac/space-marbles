using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Star")]
public class StarData : ScriptableObject
{
    public List<Sprite> BodySprites => bodySprites;
    public int MinCount => minCount;
    public int MaxCount => maxCount;
    public float MinSize  => minSize;
    public float MaxSize => maxSize;
    public bool IsCircularOnly => isCircularOnly;
    public bool IsOverlappingAllowed => isOverlappingAllowed;
    public float MinRequiredGap => minRequiredGap;
    public float MinPeriod => minPeriod;
    public float MaxPeriod => maxPeriod;
    public bool IsClockwiseOnly => isClockwiseOnly;

    [SerializeField] private List<Sprite> bodySprites = null;
    [Header("Orbits")]
    [SerializeField] private int minCount = 1;
    [SerializeField] private int maxCount = 5;
    [SerializeField] private float minSize = 1f;
    [Tooltip("Adapt the 'minGapBetweenObjects' Spawner parameter to avoid overlapping")]
    [SerializeField] private float maxSize = 30f;
    [SerializeField] private bool isCircularOnly = true;
    [SerializeField] private bool isOverlappingAllowed = false;
    [SerializeField] private float minRequiredGap = 5f;
    [SerializeField] private float minPeriod = 5f;
    [SerializeField] private float maxPeriod = 50f;
    [SerializeField] private bool isClockwiseOnly = false;

    private void OnValidate()
    {
        float minPossibleGap = (MaxSize - MinSize) / (MaxCount - 1);
        if(MinRequiredGap >= minPossibleGap)
        {
            Debug.LogError("'Min Required Gap' has to be smaller than " + minPossibleGap + " ('Max Size' - 'Min Size') / ('Max Count' - 1)");
        }
    }
}
