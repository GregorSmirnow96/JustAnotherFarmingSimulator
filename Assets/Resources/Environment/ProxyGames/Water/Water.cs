using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour, IInteractable
{
    private ModularInteractText interactText;
    private WaterContainers waterContainers;
    private Toolbar playerToolbar;

    void Start()
    {
        interactText = GetComponent<ModularInteractText>();
        interactText.SetText("'E' to Fill");
        waterContainers = WaterContainers.GetInstance();
        playerToolbar = Toolbar.instance;
    }

    public void Interact()
    {
        Item item = playerToolbar.GetEquippedItem();
        string itemId = item.type.id;
        bool isEmptyContainer = waterContainers.ItemIsEmptyContainer(itemId);
        if (isEmptyContainer)
        {
            string fullItemId = waterContainers.GetContainerMap(itemId).fullItemId;
            Item fullItem = new Item(fullItemId);
            int equippedItemIndex = playerToolbar.equippedItemIndex;
            playerToolbar.SetInventorySlot(equippedItemIndex, fullItem);
        }
    }

    public void ShowIndicator()
    {
        Item equippedItem = playerToolbar.GetEquippedItem();
        string itemId = equippedItem.type.id;
        bool isEmptyContainer = waterContainers.ItemIsEmptyContainer(itemId);
        if (isEmptyContainer)
        {
            interactText.Enable();
        }
        else
        {
            HideIndicator();
        }
    }

    public void HideIndicator()
    {
        interactText.Disable();
    }
}
