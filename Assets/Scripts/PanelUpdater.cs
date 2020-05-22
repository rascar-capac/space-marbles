using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PanelUpdater : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nameLabel = null;
    [SerializeField] private TextMeshProUGUI _typeLabel = null;

    public string NameLabel
    {
        get => _nameLabel.text;
        set => _nameLabel.text = value;
    }

    public string TypeLabel
    {
        get => _typeLabel.text;
        set => _typeLabel.text = value;
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }
}
