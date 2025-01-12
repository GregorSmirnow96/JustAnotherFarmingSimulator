using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentTile : InventoryTile, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    public bool isQuiver;
    public bool isNecklace;
    public bool isArmour;
    public bool isRing;

    void Update()
    {
        Item itemInSlot = GetEquippedItem();

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

    public Item GetEquippedItem()
    {
        Item itemInSlot = null;
        if (isQuiver)
        {
            itemInSlot = inventory.quiver;
        }
        else if (isNecklace)
        {
            itemInSlot = inventory.necklace;
        }
        else if (isArmour)
        {
            itemInSlot = inventory.armour;
        }
        else if (isRing)
        {
            itemInSlot = inventory.ring;
        }

        return itemInSlot;
    }
}
