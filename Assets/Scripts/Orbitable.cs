using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Orbitable : MonoBehaviour
{
    public List<Orbiter> Orbits { get; set; }

    [SerializeField] private Orbiter orbitPrefab = null;

    private void Awake()
    {
        Orbits = new List<Orbiter>();
    }

    public void Init(StarData data, Camera mainCamera)
    {
        int orbitCount = Random.Range(data.MinCount, data.MaxCount + 1);

        List<Vector2> sizes = new List<Vector2>();
        if(data.IsOverlappingAllowed)
        {
            sizes = ComputeOverlappingSizes(orbitCount, data);
        }
        else
        {
            sizes = ComputeNonOverlappingSizes(orbitCount, data);
        }

        for(int i = 0 ; i < sizes.Count ; i++)
        {
            Orbiter orbit = Instantiate(orbitPrefab, transform);

            orbit.Width = sizes[i].x;
            orbit.Heigth = sizes[i].y;

            orbit.Period = Random.Range(data.MinPeriod, data.MaxPeriod);

            orbit.IsClockwise = data.IsClockwiseOnly ? true : Random.value < 0.5f;

            orbit.GetComponent<OrbitRenderer>()?.Init(mainCamera);

            Orbits.Add(orbit);
        }
    }

    private List<Vector2> ComputeOverlappingSizes(int sizeCount, StarData data)
    {
        List<Vector2> sizes = new List<Vector2>();
        for(int i = 0 ; i < sizeCount ; i++)
        {
            float width = Random.Range(data.MinSize, data.MaxSize);
            float height = data.IsCircularOnly ? width : Random.Range(data.MinSize, data.MaxSize);
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

    private List<Vector2> ComputeNonOverlappingSizes(int sizeCount, StarData data)
    {
        List<float> widths = ComputeRandomNonOverlappingDimensions(sizeCount, data);
        List<float> heights = data.IsCircularOnly ? widths : ComputeRandomNonOverlappingDimensions(sizeCount, data);

        List<Vector2> sizes = new List<Vector2>();
        for(int i = 0 ; i < Mathf.Min(widths.Count, heights.Count) ; i++)
        {
            sizes.Add(new Vector2(widths[i], heights[i]));
        }
        return sizes;
    }

    private List<float> ComputeRandomNonOverlappingDimensions(int sizeCount, StarData data)
    {
        List<float> dimensions = new List<float>();
        for(int i = 0 ; i < sizeCount ; i++)
        {
            int j = 0;
            if(dimensions.Count >= 1)
            {
                bool isFull = true;
                if(dimensions[j] - data.MinSize > data.MinRequiredGap)
                {
                    isFull = false;
                }
                else if (data.MaxSize - dimensions.Last() > data.MinRequiredGap)
                {
                    isFull = false;
                }
                else
                {
                    for(j = 0 ; j < dimensions.Count - 1 ; j++)
                    {
                        if(dimensions[j + 1] - dimensions[j] > data.MinRequiredGap * 2)
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
                dimension = Random.Range(data.MinSize, data.MaxSize);

                for(j = 0 ; j < dimensions.Count ; j++)
                {
                    if(dimension < dimensions[j])
                    {
                        if((j != 0 && dimension - dimensions[j - 1]< data.MinRequiredGap) || dimensions[j] - dimension < data.MinRequiredGap)
                        {
                            isSizeWrong = true;
                        }
                        break;
                    }
                    else if(j == dimensions.Count - 1)
                    {
                        if(dimension - dimensions[j] < data.MinRequiredGap)
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
