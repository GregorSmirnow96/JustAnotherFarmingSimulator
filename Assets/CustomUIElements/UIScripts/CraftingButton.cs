using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CraftingButton : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    public CraftingRecipe selectedRecipe;
    private Inventory inventory;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        inventory = Inventory.instance;
    }

    void Update()
    {
        bool canCraftSelectedItem = GetCanCraftSelectedItem();
        canvasGroup.interactable = canCraftSelectedItem;
        canvasGroup.alpha = canCraftSelectedItem ? 1f : 0.4f;
    }

    public void SetSelectedRecipe(CraftingRecipe selectedRecipe)
    {
        this.selectedRecipe = selectedRecipe;
    }

    private bool GetCanCraftSelectedItem()
    {
        if (selectedRecipe == null)
        {
            return false;
        }

        Dictionary<string, int> materialCounts = selectedRecipe.materialItemIds
            .GroupBy(id => id)
            .ToDictionary(group => group.Key, group => group.Count());

        return inventory.PlayerHasItems(materialCounts);
    }
}
