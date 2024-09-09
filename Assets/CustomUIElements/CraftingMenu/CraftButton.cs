using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CraftButton : MonoBehaviour
{
    public Button craftButton;

    void Start()
    {
        craftButton = GetComponent<Button>();
        craftButton.onClick.AddListener(OnButtonClick);
    }

    void Update()
    {
        RecipeCard selectedCard = RecipeCard.selectedCard;

        if (selectedCard != null)
        {
            bool hasComponents = selectedCard
                .componentQuantities
                .ToList()
                .TrueForAll(componentQuantity =>
                    {
                        string itemName = componentQuantity.Key;
                        int itemQuantity = componentQuantity.Value;

                        string[,] inventoryItems = Inventory.instance.inventoryItemIds;
                        string[] toolbarItems = Inventory.instance.toolbarItemIds;
                        string[] flattenedInventoryItems = inventoryItems.Cast<string>().ToArray();
                        string[] flattenedItems = flattenedInventoryItems.Concat(toolbarItems).ToArray();

                        int ownedItemCount = flattenedItems.Count(item => item == itemName);
                        return ownedItemCount >= itemQuantity;
                    });

            craftButton.interactable = hasComponents;
        }
        else
        {
            craftButton.interactable = false;
        }
    }

    void OnButtonClick()
    {
        RecipeCard selectedCard = RecipeCard.selectedCard;
        Inventory inventory = Inventory.instance;

        selectedCard
            .componentQuantities
            .ToList()
            .ForEach(componentQuantity =>
                {
                    string itemName = componentQuantity.Key;
                    int itemQuantity = componentQuantity.Value;

                    string[,] inventoryItems = inventory.inventoryItemIds;
                    string[] toolbarItems = inventory.toolbarItemIds;
                    string[] flattenedInventoryItems = inventoryItems.Cast<string>().ToArray();
                    List<string> flattenedItems = flattenedInventoryItems.Concat(toolbarItems).ToList();

                    int itemIndex = 0;
                    List<int> componentIndices = flattenedItems
                        .ToDictionary(item => itemIndex++, item => item)
                        .ToList()
                        .Where(kvp => kvp.Value == itemName)
                        .Select(kvp => kvp.Key)
                        .Take(itemQuantity)
                        .ToList();

                    const int maxInventoryIndex = Inventory.inventoryHeight * Inventory.inventoryWidth - 1;
                    foreach (int componentIndex in componentIndices)
                    {
                        if (componentIndex <= maxInventoryIndex)
                        {
                            int x = componentIndex % Inventory.inventoryWidth;
                            int y = (int)(componentIndex / Inventory.inventoryWidth);
                            inventory.SetInventoryItem(y, x, null);
                        }
                        else
                        {
                            int toolbarIndex = componentIndex - Inventory.inventoryHeight * Inventory.inventoryWidth;
                            inventory.SetToolbarItem(toolbarIndex, null);
                        }
                    }
                });

        inventory.AddItemToInventory(selectedCard.craftableItemName);
    }
}
