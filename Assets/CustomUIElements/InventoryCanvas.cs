using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using ItemMetaData;

public class InventoryCanvas : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public GameObject inventoryTilePrefab;
    
    private const int inventoryHeight = 3;
    private const int inventoryWidth = 8;
    private const int toolbarWidth = 5;

    private const float inventoryLeftX = -224f;
    private const float inventoryTopY = 64f;
    private const float rightXValue = 224f;
    private const float rightYValue = -64f;
    private const float xTileDelta = 64f;
    private const float yTileDelta = 64f;
    private const float toolbarY = -192;
    private const float toolbarLeftX = -128f;

    private const float hoveredAlpha = 255f / 255f;
    private const float notHoveredAlpha = 220f / 255f;
    private const float noItemAlpha = 0f;
    
    private GameObject[,] inventoryTiles;
    private Dictionary<GameObject, (int, int)> inventoryTileCoords;
    private GameObject hoveredTile;
    // Properties for click-and-drag functionality
    private const string itemImageObjectName = "ItemImage";
    private GraphicRaycaster raycaster;
    private Canvas canvas;
    private GameObject clickedItemImageObject;
    private RectTransform draggedRectTransform;
    private Vector2 originalDraggedPosition;

    void Start()
    {
        raycaster = GetComponent<GraphicRaycaster>();
        canvas = GetComponent<Canvas>();

        inventoryTiles = new GameObject[inventoryHeight + 1, inventoryWidth]; // Add 1 to the height for the toolbar.
        inventoryTileCoords = new Dictionary<GameObject, (int, int)>();

        for (int y = 0; y < inventoryHeight; y++)
        {
            for (int x = 0; x < inventoryWidth; x++)
            {
                GameObject newTile = Instantiate(inventoryTilePrefab);
                newTile.transform.SetParent(transform, false);
                RectTransform tileRectTransform = newTile.GetComponent<RectTransform>();
                tileRectTransform.anchoredPosition = new Vector2(x * xTileDelta + inventoryLeftX, inventoryTopY - y * yTileDelta);
                inventoryTiles[y, x] = newTile;
                inventoryTileCoords.Add(newTile, (y, x));
            }
        }

        const int toolbarYCoord = 3;
        for (int x = 0; x < toolbarWidth; x++)
        {
            GameObject newTile = Instantiate(inventoryTilePrefab);
            newTile.transform.SetParent(transform, false);
            RectTransform tileRectTransform = newTile.GetComponent<RectTransform>();
            tileRectTransform.anchoredPosition = new Vector2(x * xTileDelta + toolbarLeftX, toolbarY);
            inventoryTiles[toolbarYCoord, x] = newTile;
            inventoryTileCoords.Add(newTile, (toolbarYCoord, x));
        }
    }

    void Update()
    {
        Vector3 cursorPosition = Input.mousePosition;

        hoveredTile = null;
        foreach (var kvp in inventoryTileCoords)
        {
            GameObject tile = kvp.Key;
            (int, int) tileCoord = kvp.Value;
            int y = tileCoord.Item1;
            int x = tileCoord.Item2;

            RectTransform tileRectTransform = tile.GetComponent<RectTransform>();
            bool cursorIsOverTile = RectTransformUtility.RectangleContainsScreenPoint(
                tileRectTransform,
                cursorPosition);
            if (cursorIsOverTile)
            {
                hoveredTile = tile;
            }

            Item tileItem = Inventory.instance.GetItem(y, x);
            bool tileItemIsNull = tileItem == null;

            Transform childTransform = tile.transform.GetChild(0);

            childTransform.GetComponent<TooltipOnHover>().item = tileItem;

            Image itemImage = childTransform.GetComponent<Image>();
            Color itemImageColor = itemImage.color;
            itemImageColor.a = tileItemIsNull
                ? noItemAlpha
                : cursorIsOverTile
                    ? hoveredAlpha
                    : notHoveredAlpha;
            itemImage.color = itemImageColor;
            
            ItemType equippedItemType = tileItemIsNull
                ? null
                : tileItem.type;
            itemImage.sprite = equippedItemType?.inventorySprite;
        }
    }

    public void Close()
    {
        CleanUpDragState();
        gameObject.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(eventData, results);

        clickedItemImageObject = results
            .Select(result => result.gameObject)
            .Where(result => result.name.Equals(itemImageObjectName))
            .FirstOrDefault();

        if (clickedItemImageObject == null) return;

        Image clickedItemImage = clickedItemImageObject.GetComponent<Image>();

        if (clickedItemImage == null)
        {
            Debug.Log("No image was clicked");
            return;
        }

        draggedRectTransform = clickedItemImage.GetComponent<RectTransform>();
        originalDraggedPosition = draggedRectTransform.anchoredPosition;
        Canvas spriteCanvas = clickedItemImageObject.AddComponent<Canvas>();
        spriteCanvas.overrideSorting = true;
        spriteCanvas.sortingOrder = 999;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (draggedRectTransform == null) return;
        draggedRectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(eventData, results);

        GameObject droppedItemImageObject = results
            .Select(result => result.gameObject)
            .Where(result => result.name.Equals(itemImageObjectName))
            .FirstOrDefault();

        if (droppedItemImageObject != null)
        {
            GameObject droppedInventoryTile = droppedItemImageObject.transform.parent.gameObject;
            (int, int) targetTileCoord = inventoryTileCoords[droppedInventoryTile];

            GameObject sourceTile = clickedItemImageObject.transform.parent.gameObject;
            (int, int) sourceTileCoord = inventoryTileCoords[sourceTile];

            Item sourceItemId = Inventory.instance.GetItem(sourceTileCoord.Item1, sourceTileCoord.Item2);
            Item targetItemId = Inventory.instance.GetItem(targetTileCoord.Item1, targetTileCoord.Item2);

            Inventory.instance.SetInventoryItem(targetTileCoord.Item1, targetTileCoord.Item2, sourceItemId);
            Inventory.instance.SetInventoryItem(sourceTileCoord.Item1, sourceTileCoord.Item2, targetItemId);
        }

        CleanUpDragState();
    }

    public void CleanUpDragState()
    {
        if (draggedRectTransform != null)
        {
            draggedRectTransform.anchoredPosition = originalDraggedPosition;
        }

        if (clickedItemImageObject != null)
        {
            Destroy(clickedItemImageObject.GetComponent<Canvas>());
        }

        clickedItemImageObject = null;
        draggedRectTransform = null;
    }
}