using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryTile : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    private static InventoryTile mouseDownTile;
    private static InventoryTile hoveredTile;

    public GameObject itemImagePrefab;
    public Vector2 tileCoord;
    public bool representsInventory = true;
    public Item backingItem;

    private GameObject itemImageObject;
    private Image itemImage;
    private RectTransform itemImageTransform;
    private Inventory inventory;
    private bool draggingItem;
    private Canvas draggedImageCanvas;
    private TooltipOnHover tooltipScript;

    void Start()
    {
        itemImageObject = Instantiate(itemImagePrefab, transform);
        tooltipScript = itemImageObject.GetComponent<TooltipOnHover>();
        itemImage = itemImageObject.GetComponent<Image>();
        itemImageTransform = itemImageObject.GetComponent<RectTransform>();

        inventory = Inventory.instance;
    }

    void Update()
    {
        Item itemInSlot = representsInventory
            ? inventory.GetItem((int) tileCoord.y, (int) tileCoord.x)
            : backingItem;

        tooltipScript.item = itemInSlot;
        Sprite itemSprite = itemInSlot?.type.inventorySprite;

        itemImage.sprite = itemSprite;
        itemImage.color = itemSprite == null
            ? itemImage.color = new Color(1f, 1f, 1f, 0f)
            : itemImage.color = new Color(1f, 1f, 1f, 1f);

        if (mouseDownTile != null && draggingItem)
        {
            // Get the mouse position in screen coordinates
            Vector2 cursorPosition = Input.mousePosition;
            
            // Convert the screen position to a local position relative to the parent
            RectTransform parentRectTransform = itemImageTransform.parent as RectTransform;
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTransform, cursorPosition, null, out localPoint);
            
            // Set the anchored position to the calculated local point
            itemImageTransform.anchoredPosition = localPoint;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (itemImage.sprite != null)
        {
            mouseDownTile = this;
            draggingItem = true;

            draggedImageCanvas = itemImageObject.AddComponent<Canvas>();
            draggedImageCanvas.overrideSorting = true;
            draggedImageCanvas.sortingOrder = 1;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {

        if (hoveredTile == null)
        {
            Debug.Log($"Invalid target; reset dragged sprite position");
            itemImageTransform.anchoredPosition = Vector2.zero;
        }
        else
        {
            Item sourceItem = mouseDownTile?.representsInventory ?? false
                ? inventory.GetItem((int) mouseDownTile.tileCoord.y, (int) mouseDownTile.tileCoord.x)
                : mouseDownTile?.backingItem;
            Item targetItem = hoveredTile?.representsInventory ?? false
                ? inventory.GetItem((int) hoveredTile.tileCoord.y, (int) hoveredTile.tileCoord.x)
                : hoveredTile?.backingItem;

            if (hoveredTile != null && hoveredTile.representsInventory)
            {
                inventory.SetInventoryItem((int) hoveredTile.tileCoord.y, (int) hoveredTile.tileCoord.x, sourceItem);
            }
            else if (hoveredTile != null)
            {
                hoveredTile.backingItem = sourceItem;
            }

            if (mouseDownTile != null && mouseDownTile.representsInventory)
            {
                inventory.SetInventoryItem((int) mouseDownTile.tileCoord.y, (int) mouseDownTile.tileCoord.x, targetItem);
            }
            else if (mouseDownTile != null)
            {
                mouseDownTile.backingItem = targetItem;
            }
        }

        Destroy(draggedImageCanvas);
        mouseDownTile = null;
        draggingItem = false;
        itemImageTransform.anchoredPosition = Vector2.zero;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hoveredTile = this;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hoveredTile = null;
    }
}
