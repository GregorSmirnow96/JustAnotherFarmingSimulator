using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using ItemMetaData;

public class CraftingMenuModel : MonoBehaviour
{
    public Canvas menuCanvas;
    public TMP_InputField searchInput;
    public GameObject scrollView;
    public GameObject viewPort;
    public GameObject content;
    public GameObject cardPrefab;

    private List<Recipe> recipes;
    private List<Recipe> filteredRecipes;
    private string lastUsedFilter = "A Random String";
    private string filter;
    private string defaultSearchText;
    private List<GameObject> cards = new List<GameObject>();

    private float contentHeight;
    public float scrollViewVerticalMargin = 3f;
    public float scrollViewHorizontalMargin = 3f;
    private const float recipeCardHeight = 36f;
    private const float recipeCardWidth = 182f;

    void Start()
    {
        recipes = new List<Recipe>()
        {
            new Recipe("Axe", new Dictionary<string, int>() { { "Log", 2 }, { "Mushroom2", 1 } }, new List<string>(){ "Wood" }),
            new Recipe("Mushroom2", new Dictionary<string, int>() { { "Log", 2 } }, new List<string>(){ "Wood" })
        };

        defaultSearchText = searchInput.text;
    }

    void Update()
    {
        HandleFilter();
    }

    void HandleFilter()
    {
        filter = searchInput.text;
        if (filter == defaultSearchText)
        {
            filter = string.Empty;
        }

        if (filter != lastUsedFilter)
        {
            filteredRecipes = recipes
                .Where(recipe => filter == null || recipe.tags.Contains(filter) || recipe.itemName.Contains(filter))
                .ToList();

            ClearCards();
            UpdateContentHeight();

            filteredRecipes.ForEach(AddRecipeCard);
        }

        lastUsedFilter = filter;
    }

    void ClearCards()
    {
        cards.ForEach(card => Destroy(card));
        cards = new List<GameObject>();
    }

    void UpdateContentHeight()
    {
        RectTransform contentRectTransform = content.GetComponent<RectTransform>();
        contentRectTransform.sizeDelta = new Vector2(
            contentRectTransform.sizeDelta.x,
            filteredRecipes.Count() * recipeCardHeight + scrollViewVerticalMargin * 2);
        contentHeight = contentRectTransform.rect.height;
    }

    void AddRecipeCard(Recipe recipe)
    {
        int newCardIndex = cards.Count();
        float nextCardY = contentHeight / 2 - recipeCardHeight / 2 - recipeCardHeight * newCardIndex - scrollViewVerticalMargin;
        float nextCardWidth = recipeCardWidth; // - scrollViewHorizontalMargin; This margin was causing weird behavior. Revisit this if you want a horizontal margin!

        GameObject newCard = Instantiate(cardPrefab, content.transform);
        RectTransform cardTransform = newCard.GetComponent<RectTransform>();
        cardTransform.anchoredPosition = new Vector2(0, nextCardY);
        cardTransform.sizeDelta = new Vector2(nextCardWidth, cardTransform.rect.height);

        RecipeCard recipeCardScript = newCard.GetComponent<RecipeCard>();
        recipeCardScript.InitializeUI(recipe.itemName, recipe.componentQuantities);

        cards.Add(newCard);
    }

    public void SetFilter(string newFilter)
    {
        filter = newFilter;
    }
}

public class Recipe
{
    private string _itemId;
    private Dictionary<string, int> _requiredItemQuantities;
    private List<string> _tags;
    private ItemType _craftedItemType;

    public Recipe(
        string itemId,
        Dictionary<string, int> requiredItemQuantities,
        List<string> tags)
    {
        _itemId = itemId;
        _requiredItemQuantities = requiredItemQuantities;
        _tags = tags;
        _craftedItemType = ItemTypeRepo.GetInstance().TryFindItemType(itemId);
    }

    public string itemName => _itemId;
    public Dictionary<string, int> componentQuantities => _requiredItemQuantities;
    public List<string> tags => _tags;
    public Sprite craftableImageSprite => _craftedItemType?.inventorySprite;
}
