using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

using ItemMetaData;

public class Toolbar : MonoBehaviour
{
    public static Toolbar instance;

    public const int inventorySize = 5;
    private const float equippedTileAlpha = 255.0f / 255.0f;
    private const float unequippedTileAlpha = 176.0f / 255.0f;
    private const float equippedItemAlpha = 229.5f / 255.0f;
    private const float unequippedItemAlpha = 176.0f / 255.0f;
    public Canvas inventoryCanvas;
    private List<Image> toolbarTiles;
    private List<Image> itemImages;
    public int equippedItemIndex = 0;

    public bool IsFull => Inventory.instance.toolbarItemIds.All(id => id != null);

    void Start()
    {
        instance = this;

        toolbarTiles = inventoryCanvas
            .GetComponentsInChildren<Image>()
            .Where(component => component.gameObject.name.Contains("ToolbarSlot"))
            .ToList();
        itemImages = toolbarTiles
            .Select(tile => tile.gameObject.transform.GetChild(0).GetComponent<Image>())
            .ToList();
    }

    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0.0f)
        {
            equippedItemIndex = (equippedItemIndex + 1) % inventorySize;
        }
        else if (scroll < 0.0f)
        {
            equippedItemIndex = (equippedItemIndex + inventorySize - 1) % inventorySize;
        }

        for (int i = 0; i < inventorySize; i++)
        {
            string itemId = Inventory.instance.toolbarItemIds[i];
            Image itemImage = itemImages[i];

            if (itemId == null)
            {
                itemImage.sprite = null;
                continue;
            }

            ItemType equippedItemType = ItemTypeRepo.GetInstance().TryFindItemType(itemId);

            itemImage.sprite = equippedItemType?.inventorySprite;
        }

        // Set the alphas:
        //      The tile alpha is simple. Set it to equipped or unequipped alpha based on equipped index.
        //      The itemSpriteAlpha has 3 states instead of 2.
        //          If the sprite exists, set it to the equipped or unequipped alpha just like the tile.
        //          If the sprite doesn't exist, set the alpha to 0.
        for (int i = 0; i < inventorySize; i++)
        {
            bool isEquippedSlot = i == equippedItemIndex;
            
            Image toolbarTile = toolbarTiles[i];
            Color toolbarTileColor = toolbarTile.color;
            toolbarTileColor.a = isEquippedSlot ? equippedTileAlpha : unequippedTileAlpha;
            toolbarTile.color = toolbarTileColor;

            string itemId = Inventory.instance.toolbarItemIds[i];
            float alpha = 0;
            if (itemId != null)
            {
                alpha = isEquippedSlot ? equippedItemAlpha : unequippedTileAlpha;
            }
            Image itemImage = itemImages[i];
            Color itemImageColor = itemImage.color;
            itemImageColor.a = alpha;
            itemImage.color = itemImageColor;
        }
    }

    public string GetItemId(int i) => Inventory.instance.toolbarItemIds[i];

    public bool AddItem(string newItemId)
    {
        int nextOpenIndex = -1;
        int currentIndex = 0;
        foreach (string itemId in Inventory.instance.toolbarItemIds)
        {
            if (itemId == null)
            {
                nextOpenIndex = currentIndex;
                break;
            }

            currentIndex++;
        }

        if (nextOpenIndex == -1)
        {
            Debug.Log("Your inventory is full");
            return false;
        }

        SetInventorySlot(nextOpenIndex, newItemId);
        return true;
    }

    public void SetInventorySlot(int index, string itemId)
    {
        if (index < 0 && index >= Inventory.instance.toolbarItemIds.Length)
        {
            Debug.Log($"Cannot set item in inventory at index {index}. This is out of bounds");
            return;
        }

        string currentItemId = Inventory.instance.toolbarItemIds[index];
        if (currentItemId != null && itemId != null)
        {
            Debug.Log($"Overwrote an item in the inventory at index: {index}. Should the item be dropped? Should this operation fail?");
        }

        Inventory.instance.toolbarItemIds[index] = itemId;
        string itemText = itemId == null ? "null" : itemId.ToString();
    }

    public void DeleteItemInSlot(int index)
    {
        SetInventorySlot(index, null);
    }

    public string GetEquippedItemId()
    {
        return Inventory.instance.toolbarItemIds[equippedItemIndex];
    }

    public void DeleteEquippedItem() => DeleteItemInSlot(equippedItemIndex);
}
