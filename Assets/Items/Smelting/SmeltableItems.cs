using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SmeltableItems
{
    private static SmeltableItems Instance;
    public static SmeltableItems GetInstance() =>
        Instance == null
            ? Instance = new SmeltableItems()
            : Instance;

    private Dictionary<string, string> smeltingMap;

    private SmeltableItems()
    {
        smeltingMap = new Dictionary<string, string>()
        {
            { "IronOre", "IronBar" }
        };
    }

    public bool ItemIdIsSmeltable(string itemName) => smeltingMap.ContainsKey(itemName ?? "Invalid");

    public string GetSmeltedItemId(string itemName) => smeltingMap[itemName];
}
