using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryTile : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    protected static InventoryTile mouseDownTile;
    protected static InventoryTile hoveredTile;

    public GameObject itemImagePrefab;
    public Vector2 tileCoord;
    public bool representsInventory = true;
    public Item backingItem;

    protected GameObject itemImageObject;
    protected Image itemImage;
    protected RectTransform itemImageTransform;
    protected Inventory inventory;
    protected bool draggingItem;
    protected Canvas draggedImageCanvas;
    protected TooltipOnHover tooltipScript;

    protected ItemQuantityText itemQuantity;

    void Awake()
    {
        itemImageObject = Instantiate(itemImagePrefab, transform);
        tooltipScript = itemImageObject.GetComponent<TooltipOnHover>();
        itemImage = itemImageObject.GetComponent<Image>();
        itemImageTransform = itemImageObject.GetComponent<RectTransform>();

        inventory = Inventory.instance;

        itemQuantity = GetComponent<ItemQuantityText>();
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

        bool itemStacks = itemInSlot?.stacks ?? false;
        if (itemStacks)
        {
            itemQuantity.SetQuantity(itemInSlot.stackSize);
        }
        itemQuantity.SetActive(itemStacks);
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
        if (hoveredTile == null || mouseDownTile == null)
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

            bool atLeastOneTileIsEquipement = mouseDownTile is EquipmentTile || hoveredTile is EquipmentTile;
            if (atLeastOneTileIsEquipement)
            {
                HandleEquipmentSwap(mouseDownTile, sourceItem, hoveredTile, targetItem);
            }
            else
            {
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
        }

        Destroy(draggedImageCanvas);
        mouseDownTile = null;
        draggingItem = false;
        itemImageTransform.anchoredPosition = Vector2.zero;
    }

    protected void HandleEquipmentSwap(InventoryTile mouseDownTile, Item mouseDownItem, InventoryTile hoveredTile, Item hoveredItem)
    {
        EquipmentTile mouseDownEquipmentTile = mouseDownTile as EquipmentTile;
        EquipmentTile hoveredEquipmentTile = hoveredTile as EquipmentTile;

        mouseDownItem = mouseDownEquipmentTile?.GetEquippedItem() ?? mouseDownItem;
        hoveredItem = hoveredEquipmentTile?.GetEquippedItem() ?? hoveredItem;

        bool onlyOneTileIsEquipmentTile = mouseDownEquipmentTile != null && hoveredEquipmentTile == null
            || mouseDownEquipmentTile == null && hoveredEquipmentTile != null;

        if (onlyOneTileIsEquipmentTile)
        {
            EquipmentTile equipmentTile = mouseDownEquipmentTile == null ? hoveredEquipmentTile : mouseDownEquipmentTile;
            InventoryTile nonEquipmentTile = mouseDownEquipmentTile == null ? mouseDownTile : hoveredTile;

            string mouseDownItemId = mouseDownItem?.type?.id;
            string hoveredItemId = hoveredItem?.type?.id;
            EquipmentItems equipmentItemRepo = EquipmentItems.GetInstance();
            if (equipmentTile.isQuiver)
            {
                if (mouseDownEquipmentTile == null && equipmentItemRepo.ItemIdIsQuiverItem(mouseDownItemId))
                {
                    inventory.SetInventoryItem((int) mouseDownTile.tileCoord.y, (int) mouseDownTile.tileCoord.x, inventory.quiver);
                    inventory.quiver = mouseDownItem;
                }
                else if (equipmentItemRepo.ItemIdIsQuiverItem(mouseDownItemId) && equipmentItemRepo.ItemIdIsQuiverItem(hoveredItemId))
                {
                    inventory.SetInventoryItem((int) hoveredTile.tileCoord.y, (int) hoveredTile.tileCoord.x, inventory.quiver);
                    inventory.quiver = hoveredItem;
                }
            }
            else if (equipmentTile.isNecklace)
            {
                if (mouseDownEquipmentTile == null && equipmentItemRepo.ItemIdIsNecklaceItem(mouseDownItemId))
                {
                    inventory.SetInventoryItem((int) mouseDownTile.tileCoord.y, (int) mouseDownTile.tileCoord.x, inventory.necklace);
                    inventory.necklace = mouseDownItem;
                }
                else if (equipmentItemRepo.ItemIdIsNecklaceItem(mouseDownItemId) && equipmentItemRepo.ItemIdIsNecklaceItem(hoveredItemId))
                {
                    inventory.SetInventoryItem((int) hoveredTile.tileCoord.y, (int) hoveredTile.tileCoord.x, inventory.necklace);
                    inventory.necklace = hoveredItem;
                }
            }
            else if (equipmentTile.isArmour)
            {
                if (mouseDownEquipmentTile == null && equipmentItemRepo.ItemIdIsArmourItem(mouseDownItemId))
                {
                    inventory.SetInventoryItem((int) mouseDownTile.tileCoord.y, (int) mouseDownTile.tileCoord.x, inventory.armour);
                    inventory.armour = mouseDownItem;
                }
                else if (equipmentItemRepo.ItemIdIsArmourItem(mouseDownItemId) && equipmentItemRepo.ItemIdIsArmourItem(hoveredItemId))
                {
                    inventory.SetInventoryItem((int) hoveredTile.tileCoord.y, (int) hoveredTile.tileCoord.x, inventory.armour);
                    inventory.armour = hoveredItem;
                }
            }
            else if (equipmentTile.isRing)
            {
                if (mouseDownEquipmentTile == null && equipmentItemRepo.ItemIdIsRingItem(mouseDownItemId))
                {
                    inventory.SetInventoryItem((int) mouseDownTile.tileCoord.y, (int) mouseDownTile.tileCoord.x, inventory.ring);
                    inventory.ring = mouseDownItem;
                }
                else if (equipmentItemRepo.ItemIdIsRingItem(mouseDownItemId) && equipmentItemRepo.ItemIdIsRingItem(hoveredItemId))
                {
                    inventory.SetInventoryItem((int) hoveredTile.tileCoord.y, (int) hoveredTile.tileCoord.x, inventory.ring);
                    inventory.ring = hoveredItem;
                }
            }
        }
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
