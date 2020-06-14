using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Identifiable : MonoBehaviour
{
    [SerializeField] private PanelUpdater idPanelPrefab = null;
    [SerializeField] private float panelOffset = 10f;
    private Canvas canvas;
    private Camera mainCamera;
    private PanelUpdater idPanel;
    private bool isMouseDown;
    private bool isMouseOver;

    public void Init(string name, Canvas canvas, Camera mainCamera)
    {
        this.canvas = canvas;
        this.mainCamera = mainCamera;
        idPanel = Instantiate(idPanelPrefab, canvas.transform);
        idPanel.NameLabel = name;
        idPanel.gameObject.SetActive(false);
    }

    private void Awake()
    {
        isMouseDown = false;
        isMouseOver = false;
    }

    private void OnMouseDown()
    {
        idPanel.gameObject.SetActive(false);
        isMouseDown = true;
    }

    private void OnMouseUp()
    {
        if(isMouseOver)
        {
            idPanel.gameObject.SetActive(true);
        }
        isMouseDown = false;
    }

    private void OnMouseEnter()
    {
        if(!isMouseDown)
        {
           idPanel.gameObject.SetActive(true);
        }
        isMouseOver = true;
    }

    private void OnMouseOver()
    {
        Vector3 panelPosition = transform.position + new Vector3(panelOffset, 0, 0);
        idPanel.transform.position = mainCamera.WorldToScreenPoint(panelPosition);
    }

    private void OnMouseExit()
    {
        idPanel.gameObject.SetActive(false);
        isMouseOver = false;
    }
}
