using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ItemMetaData
{
    public class MetalImplementations
    {
        public static List<ItemType> ORES = new List<ItemType>()
        {
            new IronOre(),
            new SilverOre(),
            new GoldOre(),
            new MoonstoneOre(),
            new SunstalOre()
        };

        public static List<ItemType> BARS = new List<ItemType>()
        {
            new IronBar(),
            new SilverBar(),
            new GoldBar(),
            new MoonstoneBar(),
            new SunstalBar()
        };

        public static List<ItemType> DUSTS = new List<ItemType>()
        {
            new IronDust(),
            new SilverDust(),
            new GoldDust(),
            new MoonstoneDust(),
            new SunstalDust()
        };

        // Ores
        
        public class IronOre : ItemType
        {
            public IronOre() : base(
                "IronOre",
                "Items/Ores/IronOre/GroundItem",
                "Items/Ores/IronOre/Sprite",
                "Items/Ores/IronOre/Equipped",
                false) {}
        }

        public class SilverOre : ItemType
        {
            public SilverOre() : base(
                "SilverOre",
                "Items/Ores/SilverOre/GroundItem",
                "Items/Ores/SilverOre/Sprite",
                "Items/Ores/SilverOre/Equipped",
                false) {}
        }

        public class GoldOre : ItemType
        {
            public GoldOre() : base(
                "GoldOre",
                "Items/Ores/GoldOre/GroundItem",
                "Items/Ores/GoldOre/Sprite",
                "Items/Ores/GoldOre/Equipped",
                false) {}
        }

        public class MoonstoneOre : ItemType
        {
            public MoonstoneOre() : base(
                "MoonstoneOre",
                "Items/Ores/MoonstoneOre/GroundItem",
                "Items/Ores/MoonstoneOre/Sprite",
                "Items/Ores/MoonstoneOre/Equipped",
                false) {}
        }

        public class SunstalOre : ItemType
        {
            public SunstalOre() : base(
                "SunstalOre",
                "Items/Ores/SunstalOre/GroundItem",
                "Items/Ores/SunstalOre/Sprite",
                "Items/Ores/SunstalOre/Equipped",
                false) {}
        }

        // Bars

        public class IronBar : ItemType
        {
            public IronBar() : base(
                "IronBar",
                "Items/Bars/IronBar/GroundItem",
                "Items/Bars/IronBar/Sprite",
                "Items/Bars/IronBar/Equipped",
                false) {}
        }

        public class SilverBar : ItemType
        {
            public SilverBar() : base(
                "SilverBar",
                "Items/Bars/SilverBar/GroundItem",
                "Items/Bars/SilverBar/Sprite",
                "Items/Bars/SilverBar/Equipped",
                false) {}
        }

        public class GoldBar : ItemType
        {
            public GoldBar() : base(
                "GoldBar",
                "Items/Bars/GoldBar/GroundItem",
                "Items/Bars/GoldBar/Sprite",
                "Items/Bars/GoldBar/Equipped",
                false) {}
        }

        public class MoonstoneBar : ItemType
        {
            public MoonstoneBar() : base(
                "MoonstoneBar",
                "Items/Bars/MoonstoneBar/GroundItem",
                "Items/Bars/MoonstoneBar/Sprite",
                "Items/Bars/MoonstoneBar/Equipped",
                false) {}
        }

        public class SunstalBar : ItemType
        {
            public SunstalBar() : base(
                "SunstalBar",
                "Items/Bars/SunstalBar/GroundItem",
                "Items/Bars/SunstalBar/Sprite",
                "Items/Bars/SunstalBar/Equipped",
                false) {}
        }

        // Dusts

        public class IronDust : ItemType
        {
            public IronDust() : base(
                "IronDust",
                "Items/Dusts/IronDust/GroundItem",
                "Items/Dusts/IronDust/Sprite",
                "Items/Dusts/IronDust/Equipped",
                false) {}
        }

        public class SilverDust : ItemType
        {
            public SilverDust() : base(
                "SilverDust",
                "Items/Dusts/SilverDust/GroundItem",
                "Items/Dusts/SilverDust/Sprite",
                "Items/Dusts/SilverDust/Equipped",
                false) {}
        }

        public class GoldDust : ItemType
        {
            public GoldDust() : base(
                "GoldDust",
                "Items/Dusts/GoldDust/GroundItem",
                "Items/Dusts/GoldDust/Sprite",
                "Items/Dusts/GoldDust/Equipped",
                false) {}
        }

        public class MoonstoneDust : ItemType
        {
            public MoonstoneDust() : base(
                "MoonstoneDust",
                "Items/Dusts/MoonstoneDust/GroundItem",
                "Items/Dusts/MoonstoneDust/Sprite",
                "Items/Dusts/MoonstoneDust/Equipped",
                false) {}
        }

        public class SunstalDust : ItemType
        {
            public SunstalDust() : base(
                "SunstalDust",
                "Items/Dusts/SunstalDust/GroundItem",
                "Items/Dusts/SunstalDust/Sprite",
                "Items/Dusts/SunstalDust/Equipped",
                false) {}
        }
    }
}
