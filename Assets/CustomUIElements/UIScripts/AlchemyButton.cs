using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AlchemyButton : MonoBehaviour
{
    public List<InventoryTile> ingredientTiles;
    public InventoryTile outputTile;

    private CanvasGroup canvasGroup;
    private Button createButton;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        createButton = GetComponent<Button>();
        createButton.onClick.AddListener(() => CreatePotion());
    }

    void Update()
    {
        bool canMakePotion = GetCanMakePotion();
        canvasGroup.interactable = canMakePotion;
        canvasGroup.alpha = canMakePotion ? 1f : 0.4f;
    }

    private void CreatePotion()
    {
        IEnumerable<Item> inputItems = ingredientTiles
            .Select(tile => tile.backingItem)
            .Where(item => item != null);
        PotionItem createdPotion = PotionCreation.Create(inputItems);

        outputTile.backingItem = createdPotion;
        ingredientTiles.ForEach(tile => tile.backingItem = null);
    }

    private bool GetCanMakePotion()
    {
        IEnumerable<Item> inputItems = ingredientTiles
            .Select(tile => tile.backingItem)
            .Where(item => item != null);

        PotionIngredientRepo ingredientRepo = PotionIngredientRepo.GetInstance();
        bool itemsAreAllIngredients = inputItems.All(item => ingredientRepo.ItemIsPotionIngredient(item.type.id));
        bool atLeastTwoIngredientsAreUsed = inputItems.Count() >= 2;

        return itemsAreAllIngredients && atLeastTwoIngredientsAreUsed && outputTile.backingItem == null;
    }
}
