using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using PP = PotionProperty;

public class PotionIngredientRepo
{
    private static PotionIngredientRepo Instance;
    public static PotionIngredientRepo GetInstance() =>
        Instance == null
            ? Instance = new PotionIngredientRepo()
            : Instance;

    private Dictionary<string, IngredientInfo> ingredientInfos;

    private PotionIngredientRepo()
    {
        ingredientInfos = new Dictionary<string, IngredientInfo>()
        {
            { "Carrot", new IngredientInfo( new []{ PP.HealOverTime, PP.CooldownReduction }, PP.None ) },
            { "BlueBerry", new IngredientInfo( new []{ PP.HealOverTime, PP.WaterDamage, PP.MovementSpeed }, PP.CooldownReduction ) },
            { "Wheat", new IngredientInfo( new []{ PP.HealImmediately, PP.CooldownReduction }, PP.HealOverTime ) },
            { "WaterStaff", new IngredientInfo( new []{ PP.WaterDamage, PP.MovementSpeed }, PP.CooldownReduction ) },
        };
    }

    public bool ItemIsPotionIngredient(string itemId) => ingredientInfos.Keys.ToList().Contains(itemId);

    public IngredientInfo GetItemIngredientInfo(string itemId)
    {
        IngredientInfo ingredientInfo;
        ingredientInfos.TryGetValue(itemId, out ingredientInfo);
        return ingredientInfo;
    }
}
