using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PanelUpdater : MonoBehaviour
{
    public string NameLabel
    {
        get => nameLabel.text;
        set => nameLabel.text = value;
    }
    public string TypeLabel
    {
        get => typeLabel.text;
        set => typeLabel.text = value;
    }

    [SerializeField] private TextMeshProUGUI nameLabel = null;
    [SerializeField] private TextMeshProUGUI typeLabel = null;
}
