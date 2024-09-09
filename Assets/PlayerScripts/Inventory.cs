using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    public GameObject inventoryCanvasObject;
    public GameObject toolbarUIContainer;
    public bool inventoryOpen;

    private FPSMovement playerController;

    public string[,] inventoryItemIds;
    public string[] toolbarItemIds;

    public const int inventoryHeight = 3;
    public const int inventoryWidth = 8;
    public const int toolbarSize = 5;

    void Start()
    {
        instance = this;

        inventoryItemIds = new string[inventoryHeight, inventoryWidth];
        toolbarItemIds = new string[toolbarSize];
        /* Delete this. I'm just seeding the inventory with items. */
        inventoryItemIds[0,0] = "Log";
        inventoryItemIds[1,2] = "Log";
        inventoryItemIds[1,3] = "Log";
        inventoryItemIds[1,4] = "Log";

        toolbarItemIds[0] = "Axe";
        /*                                                         */
        playerController = GetComponent<FPSMovement>();
        PlayerInputs inputs = PlayerInputs.GetInstance();
        inputs.RegisterGameplayAction(PlayerInputs.TOGGLE_INVENTORY, OpenInventory);
        inputs.RegisterInventoryAction(PlayerInputs.TOGGLE_INVENTORY, CloseInventory);
        inputs.RegisterInventoryAction(PlayerInputs.FORCE_CLOSE_INVENTORY, CloseInventory);
    }

    private void OpenInventory()
    {
        toolbarUIContainer.SetActive(false);
        inventoryCanvasObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        playerController.enabled = false;
        inventoryOpen = true;
    }

    private void CloseInventory()
    {
        toolbarUIContainer.SetActive(true);
        inventoryCanvasObject.GetComponent<InventoryCanvas>().Close();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerController.enabled = true;
        inventoryOpen = false;
    }

    public string GetItemId(int y, int x)
    {
        return y < inventoryHeight
            ? inventoryItemIds[y, x]
            : toolbarItemIds[x];
    }

    public void SetInventoryItem(int y, int x, string newItemId)
    {
        if (y == inventoryHeight)
        {
            SetToolbarItem(x, newItemId);
        }

        if (x >= inventoryWidth || y >= inventoryHeight || x < 0 || y < 0)
        {
            Debug.Log($"Coord (X: {x}, Y: {y}) is out of bounds for the inventory of shape: ({inventoryWidth}, {inventoryHeight})");
            return;
        }

        string itemIdAtCoord = inventoryItemIds[y, x];
        if (itemIdAtCoord != null)
        {
            Debug.Log($"Item with ID {itemIdAtCoord} at coord (X: {x}, Y: {y}) will be overwritten");
        }

        inventoryItemIds[y, x] = newItemId;
    }

    public void SetToolbarItem(int index, string newItemId)
    {
        if (index >= toolbarSize || index < 0)
        {
            Debug.Log($"Index {index} is out of bounds for the toolbar of size: {toolbarSize}");
        }

        toolbarItemIds[index] = newItemId;
    }

    public bool AddItemToInventory(string itemId)
    {
        for (int y = 0; y < inventoryHeight; y++)
        {
            for (int x = 0; x < inventoryWidth; x++)
            {
                string existingItemId = inventoryItemIds[y, x];
                if (existingItemId == null)
                {
                    SetInventoryItem(y, x, itemId);
                    return true;
                }
            }
        }

        for (int x = 0; x < toolbarSize; x++)
        {
            string existingItemId = toolbarItemIds[x];
            if (existingItemId == null)
            {
                SetInventoryItem(inventoryHeight, x, itemId);
                return true;
            }
        }

        return false;
    }
}
