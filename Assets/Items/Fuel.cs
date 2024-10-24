using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Fuel
{
    private static Fuel Instance;
    public static Fuel GetInstance() =>
        Instance == null
            ? Instance = new Fuel()
            : Instance;

    private Dictionary<string, float> itemFuelStrengths;

    private Fuel()
    {
        itemFuelStrengths = new Dictionary<string, float>()
        {
            { "Log", 1f }
        };
    }

    public bool ItemIsFuel(string itemId) => itemFuelStrengths.Keys.ToList().Contains(itemId);

    public float GetItemFuelStrength(string itemId) => itemFuelStrengths[itemId];
}
