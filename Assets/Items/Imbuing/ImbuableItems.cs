using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ImbuableItems
{
    private static ImbuableItems Instance;
    public static ImbuableItems GetInstance() =>
        Instance == null
            ? Instance = new ImbuableItems()
            : Instance;

    private Dictionary<string, string> waterImbueMap;
    private Dictionary<string, string> fireImbueMap;
    private Dictionary<string, string> lightningImbueMap;
    private Dictionary<string, string> faeImbueMap;

    private ImbuableItems()
    {
        waterImbueMap = new Dictionary<string, string>()
        {
            { "Staff", "WaterStaff" },
            { "Wand", "WaterWand" }
        };

        fireImbueMap = new Dictionary<string, string>()
        {
            { "Staff", "FireStaff" },
            { "Wand", "FireWand" }
        };

        lightningImbueMap = new Dictionary<string, string>()
        {
            { "Staff", "LightningStaff" },
            { "Wand", "LightningWand" }
        };

        faeImbueMap = new Dictionary<string, string>()
        {
            { "Staff", "FaeStaff" },
            { "Wand", "FaeWand" }
        };
    }

    public bool ItemIdIsWaterImbuable(string itemName) => waterImbueMap.ContainsKey(itemName);
    public string GetWaterImbuedItemId(string itemName) => waterImbueMap[itemName];

    public bool ItemIdIsFireImbuable(string itemName) => fireImbueMap.ContainsKey(itemName);
    public string GetFireImbuedItemId(string itemName) => fireImbueMap[itemName];

    public bool ItemIdIsLightningImbuable(string itemName) => lightningImbueMap.ContainsKey(itemName);
    public string GetLightningImbuedItemId(string itemName) => lightningImbueMap[itemName];

    public bool ItemIdIsFaeImbuable(string itemName) => faeImbueMap.ContainsKey(itemName);
    public string GetFaeImbuedItemId(string itemName) => faeImbueMap[itemName];
}
