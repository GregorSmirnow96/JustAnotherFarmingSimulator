using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class PotionCreation
{
    public static Dictionary<string, int> CalculatePropertyStrengths(IEnumerable<Item> ingredients)
    {
        PotionIngredientRepo ingredientRepo = PotionIngredientRepo.GetInstance();

        List<IngredientInfo> ingredientInfos = ingredients
            .Select(input => ingredientRepo.GetItemIngredientInfo(input.type.id))
            .ToList();

        List<string> properties = ingredientInfos
            .SelectMany(ingredientInfo => ingredientInfo.properties)
            .ToList();

        List<string> selectedProperties = properties
            .GroupBy(property => property)
            .Where(group => group.Count() > 1)
            .Select(group => group.Key)
            .ToList();

        if (selectedProperties.Count() > 0)
        {
            List<string> propertyBoosts = ingredientInfos
                .Select(ingredientInfo => ingredientInfo.boostedProperty)
                .ToList();

            Dictionary<string, int> propertyBoostCounts = propertyBoosts
                .Where(property => !property.Equals(PotionProperty.None))
                .GroupBy(property => property)
                .ToDictionary(group => group.Key, group => group.Count());

            return selectedProperties
                .ToDictionary(
                    property => property,
                    property => {
                        int boost = 0;
                        propertyBoostCounts.TryGetValue(property, out boost);
                        return boost;
                    });
        }

        return null;
    }

    public static PotionItem Create(IEnumerable<Item> inputs)
    {
        PotionIngredientRepo ingredientRepo = PotionIngredientRepo.GetInstance();

        bool inputsAreAllIngredients = inputs.All(input => ingredientRepo.ItemIsPotionIngredient(input.type.id));
        if (inputsAreAllIngredients && inputs.Count() >= 2)
        {
            List<IngredientInfo> ingredientInfos = inputs
                .Select(input => ingredientRepo.GetItemIngredientInfo(input.type.id))
                .ToList();

            List<string> properties = ingredientInfos
                .SelectMany(ingredientInfo => ingredientInfo.properties)
                .ToList();

            List<string> selectedProperties = properties
                .GroupBy(property => property)
                .Where(group => group.Count() > 1)
                .Select(group => group.Key)
                .ToList();

            // if (selectedProperties.Count() > 0)
            // {
                List<string> propertyBoosts = ingredientInfos
                    .Select(ingredientInfo => ingredientInfo.boostedProperty)
                    .ToList();

                Dictionary<string, int> propertyBoostCounts = propertyBoosts
                    .Where(property => !property.Equals(PotionProperty.None))
                    .GroupBy(property => property)
                    .ToDictionary(group => group.Key, group => group.Count());

                Dictionary<string, int> propertyStrengths = selectedProperties
                    .ToDictionary(
                        property => property,
                        property => {
                            int boost = 0;
                            propertyBoostCounts.TryGetValue(property, out boost);
                            return boost;
                        });

                PotionItem createdPotion = new PotionItem("Potion", propertyStrengths);

                return createdPotion;
            // }
        }
        else
        {
            Debug.Log($"Potion inputs are not all ingredients, or there aren't enough (2+).");
        }

        return null;
    }
}
