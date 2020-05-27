using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Identifiable : MonoBehaviour
{
    [SerializeField] private float panelOffset = 10f;

    public string Name { get; set; }

    private A2DSpawner<ADataInitializer<ScriptableObject>, ScriptableObject> gameManager;
    private Camera mainCamera;

    private void Awake()
    {
        gameManager = FindObjectOfType<A2DSpawner<ADataInitializer<ScriptableObject>, ScriptableObject>>();
        mainCamera = Camera.main;
    }

    private void OnMouseDown()
    {
        gameManager.ElementIDPanel.gameObject.SetActive(false);
    }

    private void OnMouseEnter()
    {
        gameManager.ElementIDPanel.NameLabel = Name;
        gameManager.ElementIDPanel.gameObject.SetActive(true);
    }

    private void OnMouseOver()
    {
        Vector3 panelPosition = transform.position + new Vector3(panelOffset, 0, 0);
        gameManager.ElementIDPanel.transform.position = mainCamera.WorldToScreenPoint(panelPosition);
    }

    private void OnMouseExit()
    {
        gameManager.ElementIDPanel.gameObject.SetActive(false);
    }
}
