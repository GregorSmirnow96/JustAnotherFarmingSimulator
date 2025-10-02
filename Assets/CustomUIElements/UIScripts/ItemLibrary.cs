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
    public int quantityMadePerCraft;

    public CraftingRecipe(string itemId, List<string> materialItemIds)
    {
        this.itemId = itemId;
        this.materialItemIds = materialItemIds;
        quantityMadePerCraft = 1;
    }

    public CraftingRecipe(string itemId, List<string> materialItemIds, int quantityMadePerCraft)
    {
        this.itemId = itemId;
        this.materialItemIds = materialItemIds;
        this.quantityMadePerCraft = quantityMadePerCraft;
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

    private void InjectCraftingRecipes(
        string categoryButtonPrefabName,
        List<CraftingRecipe> recipes,
        bool isDefaultCategory = false)
    {
        CraftingCategory category = new CraftingCategory($"Assets/CustomUIElements/UISprites/CraftingMenuIcons/{categoryButtonPrefabName}.prefab");

        craftableItemsByCategory.Add(category, recipes);

        GameObject buttonIcon = Instantiate(category.buttonPrefab, itemCategoryButtonsTransform);
        Button button = buttonIcon.GetComponent<Button>();
        Action buttonOnClick = () =>
        {
            buttonIcon.GetComponent<SelectableButton>().OnPointerDown(null);
            craftItemButton.GetComponent<CraftingButton>().selectedRecipe = null;
            ClearUsedMaterialsData();
            SetCratableItems(recipes);
        };
        button.onClick.AddListener(() => buttonOnClick());

        if (isDefaultCategory)
        {
            buttonOnClick();
        }
    }

    void Start()
    {
        inventory = Inventory.instance;

        itemCategoryButtonsTransform = itemCategoryButtonPanel.GetComponent<RectTransform>();
        craftableItemsByCategory = new Dictionary<CraftingCategory, List<CraftingRecipe>>();

        /* Set up the category buttons and their callbacks. Probably do this dynamically? For now there are only a couple categories, so it's hardcoded. */
        List<CraftingRecipe> toolRecipes = new List<CraftingRecipe>()
        {
            new CraftingRecipe("StoneAxe", new List<string>() { "PinewoodStick", "PinewoodStick", "Stone", "Stone" }),
            new CraftingRecipe("SilverAxe", new List<string>() { "PinewoodLog", "PinewoodLog", "SilverBar", "SilverBar" }),
            new CraftingRecipe("MoonstoneAxe", new List<string>() { "FruitwoodLog", "MoonstoneBar", "MoonstoneBar" }),
            new CraftingRecipe("GoldPickaxe", new List<string>() { "FruitwoodLog", "GoldBar", "GoldBar" }),
            new CraftingRecipe("SunstalPickaxe", new List<string>() { "SpiritwoodLog", "SunstalBar", "SunstalBar" }),
            new CraftingRecipe("ButterflyNet", new List<string>() { "FruitwoodLog", "Flax", "Flax", "Flax", "Flax" })
        };
        InjectCraftingRecipes("ButtonTools", toolRecipes, true);

        List<CraftingRecipe> armourRecipes = new List<CraftingRecipe>()
        {
            new CraftingRecipe("RoughTunic", new List<string>() { "RoughHide", "RoughHide", "RoughHide" }),
            new CraftingRecipe("QualityTunic", new List<string>() { "QualityHide", "QualityHide", "QualityHide" }),
            new CraftingRecipe("IronArmour", new List<string>() { "IronBar", "IronBar", "IronBar", "IronBar", "RoughHide" }),
            new CraftingRecipe("SilverArmour", new List<string>() { "SilverBar", "SilverBar", "SilverBar", "SilverBar", "RoughHide" }),
            new CraftingRecipe("GoldArmour", new List<string>() { "GoldBar", "GoldBar", "GoldBar", "GoldBar", "QualityHide" }),
            new CraftingRecipe("CelestialArmour", new List<string>() { "MoonstoneBar", "MoonstoneBar", "MoonstoneBar", "SunstalBar", "SunstalBar", "SunstalBar", "QualityHide" })
        };
        InjectCraftingRecipes("ButtonArmour", armourRecipes);

        List<CraftingRecipe> weaponRecipes = new List<CraftingRecipe>()
        {
            new CraftingRecipe("Staff", new List<string>() { "FruitwoodLog" }),
            new CraftingRecipe("Wand", new List<string>() { "FruitwoodLog" }),
            new CraftingRecipe("PinewoodBow", new List<string>() { "PinewoodLog", "Flax", "IronBar" }),
            new CraftingRecipe("FruitwoodBow", new List<string>() { "FruitwoodLog", "Flax", "SilverBar" }),
            new CraftingRecipe("SpiritwoodBow", new List<string>() { "SpiritwoodLog", "Flax", "MoonstoneBar" }),
            new CraftingRecipe("IronArrow", new List<string>() { "PinewoodLog", "IronBar" /* "Feather" */ }, 3),
            new CraftingRecipe("SilverArrow", new List<string>() { "PinewoodLog", "SilverBar" /* "Feather" */ }, 3),
            new CraftingRecipe("GoldArrow", new List<string>() { "PinewoodLog", "GoldBar" /* "Feather" */ }, 3),
            new CraftingRecipe("MoonstoneArrow", new List<string>() { "FruitwoodLog", "MoonstoneBar" /* "Feather" */ }, 3),
            new CraftingRecipe("SunstalArrow", new List<string>() { "FruitwoodLog", "SunstalBar" /* "Feather" */ }, 3),
            new CraftingRecipe("IronBomb", new List<string>() { "PinewoodLog", "IronBar", "IronDust" /* "String" */ }),
            new CraftingRecipe("SilverBomb", new List<string>() { "PinewoodLog", "SilverBar", "SilverDust" /* "String" */ }),
            new CraftingRecipe("GoldBomb", new List<string>() { "PinewoodLog", "GoldBar", "GoldDust" /* "String" */ }),
            new CraftingRecipe("MoonstoneBomb", new List<string>() { "PinewoodLog", "MoonstoneBar", "MoonstoneDust" /* "String" */ }),
            new CraftingRecipe("SunstalBomb", new List<string>() { "PinewoodLog", "SunstalBar", "SunstalDust" /* "String" */ })
        };
        InjectCraftingRecipes("ButtonWeapons", weaponRecipes);

        List<CraftingRecipe> jewelryRecipes = new List<CraftingRecipe>()
        {
            new CraftingRecipe("AquamarineRing", new List<string>() { "SilverBar", "Aquamarine" }),
            new CraftingRecipe("RubyRing", new List<string>() { "GoldBar", "Ruby" }),
            new CraftingRecipe("TopazRing", new List<string>() { "SilverBar", "Topaz" }),
            new CraftingRecipe("OnyxRing", new List<string>() { "GoldBar", "Onyx" }),
            new CraftingRecipe("AquamarineNecklace", new List<string>() { "SilverBar", "Aquamarine" }),
            new CraftingRecipe("RubyNecklace", new List<string>() { "GoldBar", "Ruby" }),
            new CraftingRecipe("TopazNecklace", new List<string>() { "SilverBar", "Topaz" }),
            new CraftingRecipe("OnyxNecklace", new List<string>() { "GoldBar", "Onyx" })
        };
        InjectCraftingRecipes("ButtonJewelry", jewelryRecipes);

        List<CraftingRecipe> miscRecipes = new List<CraftingRecipe>()
        {
            new CraftingRecipe("PinewoodBirdhouse", new List<string>() { "PinewoodLog", "PinewoodLog", "PinewoodLog", "IronBar" }),
            new CraftingRecipe("FruitwoodBirdhouse", new List<string>() { "FruitwoodLog", "FruitwoodLog", "FruitwoodLog", "IronBar" }),
            new CraftingRecipe("SpiritwoodBirdhouse", new List<string>() { "SpiritwoodLog", "SpiritwoodLog", "SpiritwoodLog", "IronBar" }),
            new CraftingRecipe("Bowl", new List<string>() { "PinewoodLog" }),
            new CraftingRecipe("Lantern", new List<string>() { "IronBar", "IronBar", "PinewoodLog", "Miremoss" }),
            new CraftingRecipe("CampfireKit", new List<string>() { "PinewoodLog", "PinewoodLog", "Miremoss", "IronDust" }),
            new CraftingRecipe("Beacon", new List<string>() { "PinewoodLog", "PinewoodLog", "Miremoss", "Miremoss", "SilverDust" }),
            new CraftingRecipe("Brazier", new List<string>() { "IronBar", "IronBar", "MoonstoneBar", "PinewoodLog", "PinewoodLog", "PinewoodLog", "PinewoodLog", "SilverDust" }),
            new CraftingRecipe("Fence", new List<string>() { "PinewoodLog", "PinewoodLog", "PinewoodLog" })
        };
        InjectCraftingRecipes("ButtonWood", miscRecipes);

        /* Set up "Craft" button callback */
        craftItemButton.onClick.AddListener(() => TryCraftItem());
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
            ItemType materialItem = ItemTypeRepo.GetInstance().TryFindItemType(materialId);
            Sprite materialItemSprite = ItemTypeRepo.GetInstance().TryFindItemType(materialId)?.inventorySprite;
            materialItemImage.GetComponent<Image>().sprite = materialItemSprite;

            materialItemImage.GetComponent<MaterialCountIcon>().SetItemQuantity(materialId, materialCount);

            // Trying to implement tooltip on hover for input matieral here.
            // The idea is: Attach TooltipOnHover script to the MaterialItem sprite (done), Use the TooltipOnHover's Item property to set the tooltip backing data.
            TooltipOnHover inputMaterialTooltipScript = materialItemImage.GetComponent<TooltipOnHover>();
            inputMaterialTooltipScript.item = new Item(materialItem);

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

        for (int i = 0; i < selectedRecipe.quantityMadePerCraft; i++)
        {
            inventory.AddItemToInventory(craftedItem);
        }
    }
}
