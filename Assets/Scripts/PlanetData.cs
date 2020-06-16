using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetData
{
    public string PlanetName => planetName;
    public Texture2D Surface => surface;
    public Texture2D Pattern => pattern;
    public Color[] Colors => colors;
    public Sprite Extra => extra;
    public float Mass => mass;
    public float Drag => drag;
    public float AngularDrag => angularDrag;

    private string planetName;
    private Texture2D surface;
    private Texture2D pattern;
    private Color[] colors;
    private Sprite extra;
    private float mass;
    private float drag;
    private float angularDrag;

    public PlanetData(string planetName, Texture2D surface, Texture2D pattern, Color[] colors, Sprite extra, float mass, float drag, float angularDrag)
    {
        this.planetName = planetName;
        this.surface = surface;
        this.pattern = pattern;
        this.colors = colors;
        this.extra = extra;
        this.mass = mass;
        this.drag = drag;
        this.angularDrag = angularDrag;
    }
}
