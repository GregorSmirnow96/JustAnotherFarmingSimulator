using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EquipmentItems
{
    private static EquipmentItems Instance;
    public static EquipmentItems GetInstance() =>
        Instance == null
            ? Instance = new EquipmentItems()
            : Instance;

    private List<string> quiverItemIds;
    private List<string> necklaceItemIds;
    private List<string> armourItemIds;
    private List<string> ringItemIds;

    private EquipmentItems()
    {
        quiverItemIds = new List<string>()
        {
            "IronArrow",
            "SilverArrow",
            "GoldArrow",
            "MoonstoneArrow",
            "SunstalArrow",
            "WaterArrow",
            "LightningArrow",
            "FireArrow",
            "LunarArrow",
            "SolarArrow",
        };
        necklaceItemIds = new List<string>()
        {
            "AquamarineNecklace",
            "OnyxNecklace",
            "RubyNecklace",
            "TopazNecklace"
        };
        armourItemIds = new List<string>()
        {
            "RoughTunic",
            "QualityTunic",
            "FireTunic",
            "WaterTunic",
            "LightningTunic",
            "FaeTunic",
            "IronArmour",
            "SilverArmour",
            "GoldArmour",
            "CelestialArmour",
            "FireArmour",
            "WaterArmour",
            "LightningArmour",
            "FaeArmour"
        };
        ringItemIds = new List<string>()
        {
            "AquamarineRing",
            "OnyxRing",
            "RubyRing",
            "TopazRing"
        };
    }

    public bool ItemIdIsQuiverItem(string itemId) => itemId == null || quiverItemIds.Contains(itemId);
    public bool ItemIdIsNecklaceItem(string itemId) => itemId == null || necklaceItemIds.Contains(itemId);
    public bool ItemIdIsArmourItem(string itemId) => itemId == null || armourItemIds.Contains(itemId);
    public bool ItemIdIsRingItem(string itemId) => itemId == null || ringItemIds.Contains(itemId);
}
