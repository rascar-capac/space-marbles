using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class StarInitializer : ADataInitializer<StarData>
{
    [SerializeField] private GameObject body = null;
    [SerializeField] private OrbitHandler orbitPrefab = null;

    public List<OrbitHandler> Orbits { get; set; }

    protected override void Awake()
    {
        base.Awake();
        Orbits = new List<OrbitHandler>();
    }

    public override void Init(StarData data)
    {
        base.Init(data);
        Sprite bodySprite = data.BodySprites[Random.Range(0, data.BodySprites.Count)];
        body.GetComponent<SpriteRenderer>().sprite = bodySprite;

        int orbitCount = Random.Range(data.MinCount, data.MaxCount + 1);

        List<Vector2> sizes = new List<Vector2>();
        if(data.IsOverlappingAllowed)
        {
            sizes = ComputeOverlappingSizes(orbitCount);
        }
        else
        {
            sizes = ComputeNonOverlappingSizes(orbitCount);
        }

        for(int i = 0 ; i < sizes.Count ; i++)
        {
            OrbitHandler orbit = Instantiate(orbitPrefab, transform);

            orbit.Width = sizes[i].x;
            orbit.Heigth = sizes[i].y;

            orbit.Period = Random.Range(data.MinPeriod, data.MaxPeriod);

            orbit.IsClockwise = data.IsClockwiseOnly ? true : Random.value < 0.5f;

            Orbits.Add(orbit);
        }
    }

    private List<Vector2> ComputeOverlappingSizes(int sizeCount)
    {
        List<Vector2> sizes = new List<Vector2>();
        for(int i = 0 ; i < sizeCount ; i++)
        {
            float width = Random.Range(Data.MinSize, Data.MaxSize);
            float height = Data.IsCircularOnly ? width : Random.Range(Data.MinSize, Data.MaxSize);
            sizes.Add(new Vector2(width, height));
        }

        for(int i = 0 ; i < sizes.Count - 1 ; i++)
        {
            int min = i;
            for(int j = i + 1 ; j < sizes.Count ; j++)
            {
                if(sizes[j].x < sizes[min].x)
                {
                    min = j;
                }
            }
            Vector2 sortedSize = sizes[min];
            sizes.Remove(sortedSize);
            sizes.Insert(i, sortedSize);
        }
        // sizes = sizes.OrderBy(v => v.x).ToList();

        return sizes;
    }

    private List<Vector2> ComputeNonOverlappingSizes(int sizeCount)
    {
        List<float> widths = ComputeRandomNonOverlappingDimensions(sizeCount);
        List<float> heights = Data.IsCircularOnly ? widths : ComputeRandomNonOverlappingDimensions(sizeCount);

        List<Vector2> sizes = new List<Vector2>();
        for(int i = 0 ; i < Mathf.Min(widths.Count, heights.Count) ; i++)
        {
            sizes.Add(new Vector2(widths[i], heights[i]));
        }
        return sizes;
    }

    private List<float> ComputeRandomNonOverlappingDimensions(int sizeCount)
    {
        List<float> dimensions = new List<float>();
        for(int i = 0 ; i < sizeCount ; i++)
        {
            int j = 0;
            if(dimensions.Count >= 1)
            {
                bool isFull = true;
                if(dimensions[j] - Data.MinSize > Data.MinRequiredGap)
                {
                    isFull = false;
                }
                else if (Data.MaxSize - dimensions.Last() > Data.MinRequiredGap)
                {
                    isFull = false;
                }
                else
                {
                    for(j = 0 ; j < dimensions.Count - 1 ; j++)
                    {
                        if(dimensions[j + 1] - dimensions[j] > Data.MinRequiredGap * 2)
                        {
                            isFull = false;
                            break;
                        }
                    }
                }

                if(isFull)
                {
                    break;
                }
            }

            j = 0;
            float dimension = 0;
            bool isSizeWrong = true;
            while(isSizeWrong)
            {
                isSizeWrong = false;
                dimension = Random.Range(Data.MinSize, Data.MaxSize);

                for(j = 0 ; j < dimensions.Count ; j++)
                {
                    if(dimension < dimensions[j])
                    {
                        if((j != 0 && dimension - dimensions[j - 1]< Data.MinRequiredGap) || dimensions[j] - dimension < Data.MinRequiredGap)
                        {
                            isSizeWrong = true;
                        }
                        break;
                    }
                    else if(j == dimensions.Count - 1)
                    {
                        if(dimension - dimensions[j] < Data.MinRequiredGap)
                        {
                            isSizeWrong = true;
                        }
                    }
                }
            }
            dimensions.Insert(j, dimension);
        }
        return dimensions;
    }
}
