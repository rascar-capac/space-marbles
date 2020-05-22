using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New NameTemplates", menuName = "Name Templates")]
public class NameTemplatesData : ScriptableObject
{
    [SerializeField] private List<string> _templates = null;

    public List<string> Templates { get => _templates; }
}
