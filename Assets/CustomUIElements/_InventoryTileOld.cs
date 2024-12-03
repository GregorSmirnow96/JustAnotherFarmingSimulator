using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class _InventoryTileOld : MonoBehaviour
{
    public Vector2 tileCoordinate;
    public int? itemId;

    private GameObject canvasObject;
    private Canvas canvas;
    private RectTransform canvasRectTransform;
    private Image itemImage;
    private float hoveredAlpha = 140f / 255f;
    private float notHoveredAlpha = 0f;

    void Start()
    {
        //canvasObject = GameObject.Find("InventoryCanvas");
        //canvas = canvasObject.GetComponent<Canvas>();
        //canvasRectTransform = canvasObject.GetComponent<RectTransform>();
        itemImage = gameObject.transform.GetChild(0).GetComponent<Image>();
    }

    void Update()
    {
        Vector3 cursorPosition = Input.mousePosition;

        RectTransform tileRectTransform = GetComponent<RectTransform>();
        bool cursorIsOverTile = RectTransformUtility.RectangleContainsScreenPoint(
            tileRectTransform,
            cursorPosition);
        
        Color itemImageColor = itemImage.color;
        itemImageColor.a = cursorIsOverTile ? hoveredAlpha : notHoveredAlpha;
        itemImage.color = itemImageColor;
    }
}
