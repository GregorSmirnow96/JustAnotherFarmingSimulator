using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ItemMetaData
{
    public class ArrowImplementations
    {
        public static List<ItemType> ARROWS = new List<ItemType>()
        {
            new IronArrow(),
            new SilverArrow(),
            new GoldArrow(),
            new MoonstoneArrow(),
            new SunstalArrow(),
            
            new FireArrow(),
            new LightningArrow(),
            new WaterArrow(),
            new LunarArrow(),
            new SolarArrow()
        };
    }

    public class IronArrow : ArrowItemType
    {
        public IronArrow() : base(
            "IronArrow",
            "Items/Arrows/IronArrow/GroundItem",
            "Items/Arrows/IronArrow/Sprite",
            "Items/Arrows/IronArrow/Equipped") {}
    }

    public class SilverArrow : ArrowItemType
    {
        public SilverArrow() : base(
            "SilverArrow",
            "Items/Arrows/SilverArrow/GroundItem",
            "Items/Arrows/SilverArrow/Sprite",
            "Items/Arrows/SilverArrow/Equipped") {}
    }

    public class GoldArrow : ArrowItemType
    {
        public GoldArrow() : base(
            "GoldArrow",
            "Items/Arrows/GoldArrow/GroundItem",
            "Items/Arrows/GoldArrow/Sprite",
            "Items/Arrows/GoldArrow/Equipped") {}
    }

    public class MoonstoneArrow : ArrowItemType
    {
        public MoonstoneArrow() : base(
            "MoonstoneArrow",
            "Items/Arrows/MoonstoneArrow/GroundItem",
            "Items/Arrows/MoonstoneArrow/Sprite",
            "Items/Arrows/MoonstoneArrow/Equipped") {}
    }

    public class SunstalArrow : ArrowItemType
    {
        public SunstalArrow() : base(
            "SunstalArrow",
            "Items/Arrows/SunstalArrow/GroundItem",
            "Items/Arrows/SunstalArrow/Sprite",
            "Items/Arrows/SunstalArrow/Equipped") {}
    }

    public class FireArrow : ArrowItemType
    {
        public FireArrow() : base(
            "FireArrow",
            "Items/Arrows/FireArrow/GroundItem",
            "Items/Arrows/FireArrow/Sprite",
            "Items/Arrows/FireArrow/Equipped") {}
    }

    public class LightningArrow : ArrowItemType
    {
        public LightningArrow() : base(
            "LightningArrow",
            "Items/Arrows/LightningArrow/GroundItem",
            "Items/Arrows/LightningArrow/Sprite",
            "Items/Arrows/LightningArrow/Equipped") {}
    }

    public class WaterArrow : ArrowItemType
    {
        public WaterArrow() : base(
            "WaterArrow",
            "Items/Arrows/WaterArrow/GroundItem",
            "Items/Arrows/WaterArrow/Sprite",
            "Items/Arrows/WaterArrow/Equipped") {}
    }

    public class LunarArrow : ArrowItemType
    {
        public LunarArrow() : base(
            "LunarArrow",
            "Items/Arrows/LunarArrow/GroundItem",
            "Items/Arrows/LunarArrow/Sprite",
            "Items/Arrows/LunarArrow/Equipped") {}
    }

    public class SolarArrow : ArrowItemType
    {
        public SolarArrow() : base(
            "SolarArrow",
            "Items/Arrows/SolarArrow/GroundItem",
            "Items/Arrows/SolarArrow/Sprite",
            "Items/Arrows/SolarArrow/Equipped") {}
    }
}
