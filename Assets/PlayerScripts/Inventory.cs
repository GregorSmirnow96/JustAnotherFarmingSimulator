using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    public GameObject inventoryCanvasObject;
    public GameObject toolbarUIContainer;
    public bool inventoryOpen;

    private FPSMovement playerController;

    public Item[,] inventoryItems;
    public Item[] toolbarItems;

    public const int inventoryHeight = 3;
    public const int inventoryWidth = 8;
    public const int toolbarSize = 5;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        inventoryItems = new Item[inventoryHeight, inventoryWidth];
        toolbarItems = new Item[toolbarSize];
        /* Delete this. I'm just seeding the inventory with items. */
        inventoryItems[0,0] = new Item("Carrot");
        inventoryItems[0,1] = new Item("BlueBerry");
        inventoryItems[0,2] = new Item("Wheat");
        inventoryItems[1,1] = new Item("Log");
        inventoryItems[1,2] = new Item("WaterWand");
        inventoryItems[1,3] = new Item("BlueBerrySeed");
        inventoryItems[1,4] = new Item("Pickaxe");
        inventoryItems[2,0] = new Item("FaeStaff");
        inventoryItems[2,1] = new Item("FaeWand");
        inventoryItems[2,2] = null;
        inventoryItems[2,3] = null;
        inventoryItems[2,4] = new Item("LightningWand");
        inventoryItems[2,5] = new Item("LightningStaff");

        toolbarItems[0] = new Item("WaterStaff");
        toolbarItems[1] = new Item("IronOre");
        toolbarItems[2] = new Item("IronOre");
        toolbarItems[3] = new Item("FireWand");
        toolbarItems[4] = new Item("FireStaff");
        /*                                                         */
        playerController = GetComponent<FPSMovement>();
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

    public bool IsFull() => inventoryItems.Cast<Item>().All(id => id != null);

    public Item GetItem(int y, int x)
    {
        return y < inventoryHeight
            ? inventoryItems[y, x]
            : toolbarItems[x];
    }

    public void SetInventoryItem(int y, int x, Item newItem)
    {
        if (y == inventoryHeight)
        {
            Item preItem = GetItem(y, x);
            SetToolbarItem(x, newItem);
            Item postItem = GetItem(y, x);
        }

        if (x >= inventoryWidth || y >= inventoryHeight || x < 0 || y < 0)
        {
            Debug.Log($"Coord (X: {x}, Y: {y}) is out of bounds for the inventory of shape: ({inventoryWidth}, {inventoryHeight})");
            return;
        }

        Item itemAtCoord = inventoryItems[y, x];
        if (itemAtCoord != null)
        {
            Debug.Log($"Item with ID {itemAtCoord} at coord (X: {x}, Y: {y}) will be overwritten");
        }

        inventoryItems[y, x] = newItem;
    }

    public void SetToolbarItem(int index, Item newItem)
    {
        if (index >= toolbarSize || index < 0)
        {
            Debug.Log($"Index {index} is out of bounds for the toolbar of size: {toolbarSize}");
        }

        toolbarItems[index] = newItem;
    }

    public bool AddItemToInventory(Item item)
    {
        for (int y = 0; y < inventoryHeight; y++)
        {
            for (int x = 0; x < inventoryWidth; x++)
            {
                Item existingItemId = inventoryItems[y, x];
                if (existingItemId == null)
                {
                    SetInventoryItem(y, x, item);
                    return true;
                }
            }
        }

        for (int x = 0; x < toolbarSize; x++)
        {
            Item existingItemId = toolbarItems[x];
            if (existingItemId == null)
            {
                SetInventoryItem(inventoryHeight, x, item);
                return true;
            }
        }

        return false;
    }

    public void RemoveItemsById(string id, int count)
    {
        int removedItemCount = 0;
        for (int y = 0; y < inventoryHeight; y++)
        {
            for (int x = 0; x < inventoryWidth; x++)
            {
                if (inventoryItems[y, x]?.type.id == id)
                {
                    SetInventoryItem(y, x, null);
                    removedItemCount++;
                }

                if (removedItemCount >= count) return;
            }
        }

        for (int x = 0; x < toolbarSize; x++)
        {
            if (toolbarItems[x]?.type.id == id)
            {
                SetToolbarItem(x, null);
                removedItemCount++;
            }

            if (removedItemCount >= count) return;
        }
    }

    public bool PlayerHasItems(Dictionary<string, int> itemCounts)
    {
        Item[] flatInventoryItems = inventoryItems.Cast<Item>().ToArray();

        Item[] heldItems = toolbarItems.Concat(flatInventoryItems).ToArray();
        Dictionary<string, int> heldItemCounts = heldItems
            .Select(item => item == null ? "null" : item.type.id)
            .GroupBy(id => id)
            .ToDictionary(group => group.Key, group => group.Count());

        foreach(KeyValuePair<string, int> item in itemCounts)
        {
            string requiredItemId = item.Key;
            if (!heldItemCounts.ContainsKey(requiredItemId))
            {
                return false;
            }

            int requiredQuantity = item.Value;
            int heldQuantity = heldItemCounts[requiredItemId];

            if (requiredQuantity > heldQuantity)
            {
                return false;
            }
        }

        return true;
    }
}
