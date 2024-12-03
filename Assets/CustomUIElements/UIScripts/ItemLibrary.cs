using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

using ItemMetaData;

public class CraftingCategory
{
    public GameObject buttonPrefab;

    public CraftingCategory(string buttonPrefabPath)
    {
        buttonPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(buttonPrefabPath);
    }
}

public class CraftingRecipe
{
    public string itemId;
    public List<string> materialItemIds;

    public CraftingRecipe(string itemId, List<string> materialItemIds)
    {
        this.itemId = itemId;
        this.materialItemIds = materialItemIds;
    }
}

public class ItemLibrary : MonoBehaviour
{
    public GameObject itemCategoryButtonPanel;
    public GameObject craftableItemPanel;
    public GameObject usedMaterialsPanel;
    public Button craftItemButton;

    public GameObject craftableItemImagePrefab;
    public GameObject materialItemImagePrefab;

    private Dictionary<CraftingCategory, List<CraftingRecipe>> craftableItemsByCategory;
    private RectTransform itemCategoryButtonsTransform;
    private RectTransform craftableItemPanelTransform;
    private CraftingRecipe selectedRecipe;
    private Inventory inventory;

    void Start()
    {
        inventory = Inventory.instance;

        /* Set up the category buttons and their callbacks. Probably do this dynamically? For now there are only a couple categories, so it's hardcoded. */
        CraftingCategory wood = new CraftingCategory("Assets/CustomUIElements/UISprites/CraftingMenuIcons/WoodCraftingButton.prefab");
        List<CraftingRecipe> woodRecipes = new List<CraftingRecipe>()
        {
            new CraftingRecipe("Bowl", new List<string>() { "Log", "Log", "WaterStaff" })
        };

        itemCategoryButtonsTransform = itemCategoryButtonPanel.GetComponent<RectTransform>();
        GameObject woodIcon = Instantiate(wood.buttonPrefab, itemCategoryButtonsTransform);
        Button woodButton = woodIcon.GetComponent<Button>();
        Action woodButtonOnClick = () =>
        {
            woodIcon.GetComponent<SelectableButton>().OnPointerDown(null);
            craftItemButton.GetComponent<CraftingButton>().selectedRecipe = null;
            ClearUsedMaterialsData();
            SetCratableItems(woodRecipes);
        };
        woodButton.onClick.AddListener(() => woodButtonOnClick());

        /* DELETE THIS! Or replace it with the next crafting category. It's a duplication of the wood category for testing purposes */
        CraftingCategory wood2 = new CraftingCategory("Assets/CustomUIElements/UISprites/CraftingMenuIcons/WoodCraftingButton.prefab");
        List<CraftingRecipe> woodRecipes2 = new List<CraftingRecipe>()
        {
            new CraftingRecipe("Bowl", new List<string>() { "Log", "Log" }),
            new CraftingRecipe("Bowl", new List<string>() { "Log" })
        };

        itemCategoryButtonsTransform = itemCategoryButtonPanel.GetComponent<RectTransform>();
        GameObject woodIcon2 = Instantiate(wood2.buttonPrefab, itemCategoryButtonsTransform);
        Button woodButton2 = woodIcon2.GetComponent<Button>();
        woodButton2.onClick.AddListener(() =>
        {
            woodIcon2.GetComponent<SelectableButton>().OnPointerDown(null);
            craftItemButton.GetComponent<CraftingButton>().selectedRecipe = null;
            ClearUsedMaterialsData();
            SetCratableItems(woodRecipes2);
        });
        /* ------------------------------------------------------------------------------------------------------------------------ */

        craftableItemsByCategory = new Dictionary<CraftingCategory, List<CraftingRecipe>>()
        {
            { wood, woodRecipes },
            { wood2, woodRecipes2 }
        };

        /* Set up "Craft" button callback */
        craftItemButton.onClick.AddListener(() => TryCraftItem());

        /* Initially show wood items */
        woodButtonOnClick();
    }

    private void SetCratableItems(List<CraftingRecipe> craftingRecipes)
    {
        if (craftingRecipes.Count >= 18)
        {
            Debug.Log("Too many recipes exist in this category to render them all. Implement a scroll wheel... ?");
        }

        int recipeIndex = 0;
        foreach (Transform slotTransform in craftableItemPanel.transform)
        {
            while (slotTransform.childCount > 0)
            {
                DestroyImmediate(slotTransform.GetChild(0).gameObject);
            }

            if (recipeIndex >= craftingRecipes.Count)
            {
                continue;
            }

            GameObject craftableItemButton = Instantiate(craftableItemImagePrefab, slotTransform);
            GameObject craftableItemImage = craftableItemButton.transform.GetChild(0).gameObject;

            CraftingRecipe recipe = craftingRecipes.ElementAt(recipeIndex);
            Sprite recipeItemSprite = ItemTypeRepo.GetInstance().TryFindItemType(recipe.itemId)?.inventorySprite;
            craftableItemImage.GetComponent<Image>().sprite = recipeItemSprite;

            Button craftableItemButtonScript = craftableItemButton.GetComponent<Button>();
            craftableItemButtonScript.onClick.AddListener(() =>
            {
                selectedRecipe = recipe;
                CraftingButton craftingButtonScript = craftItemButton.GetComponent<CraftingButton>();
                craftingButtonScript.SetSelectedRecipe(selectedRecipe);
                SetUsedMaterialsData(recipe.materialItemIds);
            });

            recipeIndex++;
        }
    }

    private void SetUsedMaterialsData(List<string> materialItemIds)
    {
        ClearUsedMaterialsData();

        List<string> uniqueMaterials = materialItemIds.Distinct().ToList();
        if (uniqueMaterials.Count() > 4)
        {
            Debug.Log("The UI only supports 4 unique materials being used to craft an item. Yuck.");
            return;
        }

        Dictionary<string, int> materialCounts = materialItemIds
            .GroupBy(id => id)
            .ToDictionary(group => group.Key, group => group.Count());

        int materialIndex = 0;
        foreach (Transform slotTransform in usedMaterialsPanel.transform)
        {
            if (materialIndex >= uniqueMaterials.Count())
            {
                continue;
            }

            string materialId = uniqueMaterials.ElementAt(materialIndex);
            int materialCount = materialCounts[materialId];

            GameObject materialItemImage = Instantiate(materialItemImagePrefab, slotTransform);
            Sprite materialItemSprite = ItemTypeRepo.GetInstance().TryFindItemType(materialId)?.inventorySprite;
            materialItemImage.GetComponent<Image>().sprite = materialItemSprite;

            materialItemImage.GetComponent<MaterialCountIcon>().SetItemQuantity(materialId, materialCount);

            materialIndex++;
        }
    }

    private void ClearUsedMaterialsData()
    {
        foreach (Transform slotTransform in usedMaterialsPanel.transform)
        {
            while (slotTransform.childCount > 0)
            {
                DestroyImmediate(slotTransform.GetChild(0).gameObject);
            }
        }
    }

    private void TryCraftItem()
    {
        Dictionary<string, int> materialCounts = selectedRecipe.materialItemIds
            .GroupBy(id => id)
            .ToDictionary(group => group.Key, group => group.Count());

        bool hasRequiredMaterials = inventory.PlayerHasItems(materialCounts);

        if (hasRequiredMaterials)
        {
            foreach(KeyValuePair<string, int> item in materialCounts)
            {
                string requiredItemId = item.Key;
                int requiredQuantity = item.Value;

                inventory.RemoveItemsById(requiredItemId, requiredQuantity);
            }
        }

        ItemType creaftedItemType = ItemTypeRepo.GetInstance().TryFindItemType(selectedRecipe.itemId);
        Item craftedItem = new Item(creaftedItemType);
        inventory.AddItemToInventory(craftedItem);
    }
}
