using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Star")]
public class StarData : ScriptableObject
{
    [SerializeField] private List<Sprite> _bodySprites = null;
    [SerializeField] private int _minCount = 1;
    [SerializeField] private int _maxCount = 5;
    [SerializeField] private float _minSize = 1f;
    [Tooltip("Adapt the 'minGapBetweenObjects' Spawner parameter to avoid overlapping")]
    [SerializeField] private float _maxSize = 30f;
    [SerializeField] private bool _isCircularOnly = true;
    [SerializeField] private bool _isOverlappingAllowed = false;
    [SerializeField] private float _minRequiredGap = 5f;
    [SerializeField] private float _minPeriod = 5f;
    [SerializeField] private float _maxPeriod = 50f;
    [SerializeField] private bool _isClockwiseOnly = false;

    public List<Sprite> BodySprites { get => _bodySprites; }
    public int MinCount { get => _minCount; }
    public int MaxCount { get => _maxCount; }
    public float MinSize  { get => _minSize; }
    public float MaxSize { get => _maxSize; }
    public bool IsCircularOnly { get => _isCircularOnly; }
    public bool IsOverlappingAllowed { get => _isOverlappingAllowed; }
    public float MinRequiredGap { get => _minRequiredGap; }
    public float MinPeriod { get => _minPeriod; }
    public float MaxPeriod { get => _maxPeriod; }
    public bool IsClockwiseOnly { get => _isClockwiseOnly; }

    private void OnValidate()
    {
        float minPossibleGap = (MaxSize - MinSize) / (MaxCount - 1);
        if(MinRequiredGap >= minPossibleGap)
        {
            Debug.LogError("'Min Required Gap' has to be smaller than " + minPossibleGap + " ('Max Size' - 'Min Size') / ('Max Count' - 1)");
        }
    }
}
