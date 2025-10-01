using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ItemMetaData
{
    public class WeaponImplementations
    {
        public static List<ItemType> SWORDS = new List<ItemType>()
        {
            new IronSword(),
            new SilverSword(),
            new GoldSword(),
            new CelestialSword()
        };

        public static List<ItemType> AXES = new List<ItemType>()
        {
            new StoneAxe(),
            new SilverAxe(),
            new MoonstoneAxe()
        };

        public static List<ItemType> PICKAXES = new List<ItemType>()
        {
            new StonePickaxe(),
            new GoldPickaxe(),
            new SunstalPickaxe()
        };

        // Swords

        public class IronSword : ItemType
        {
            public IronSword() : base(
                "IronSword",
                "Items/Swords/IronSword/GroundItem",
                "Items/Swords/IronSword/Sprite",
                "Items/Swords/IronSword/Equipped",
                false) {}
        }

        public class SilverSword : ItemType
        {
            public SilverSword() : base(
                "SilverSword",
                "Items/Swords/SilverSword/GroundItem",
                "Items/Swords/SilverSword/Sprite",
                "Items/Swords/SilverSword/Equipped",
                false) {}
        }

        public class GoldSword : ItemType
        {
            public GoldSword() : base(
                "GoldSword",
                "Items/Swords/GoldSword/GroundItem",
                "Items/Swords/GoldSword/Sprite",
                "Items/Swords/GoldSword/Equipped",
                false) {}
        }

        public class CelestialSword : ItemType
        {
            public CelestialSword() : base(
                "CelestialSword",
                "Items/Swords/CelestialSword/GroundItem",
                "Items/Swords/CelestialSword/Sprite",
                "Items/Swords/CelestialSword/Equipped",
                false) {}
        }

        // Axe

        public class StoneAxe : ItemType
        {
            public StoneAxe() : base(
                "StoneAxe",
                "Items/Axes/StoneAxe/GroundItem",
                "Items/Axes/StoneAxe/Sprite",
                "Items/Axes/StoneAxe/Equipped",
                false) {}
        }

        public class SilverAxe : ItemType
        {
            public SilverAxe() : base(
                "SilverAxe",
                "Items/Axes/SilverAxe/GroundItem",
                "Items/Axes/SilverAxe/Sprite",
                "Items/Axes/SilverAxe/Equipped",
                false) {}
        }

        public class MoonstoneAxe : ItemType
        {
            public MoonstoneAxe() : base(
                "MoonstoneAxe",
                "Items/Axes/MoonstoneAxe/GroundItem",
                "Items/Axes/MoonstoneAxe/Sprite",
                "Items/Axes/MoonstoneAxe/Equipped",
                false) {}
        }

        // Pickaxe

        public class StonePickaxe : ItemType
        {
            public StonePickaxe() : base(
                "StonePickaxe",
                "Items/Pickaxes/StonePickaxe/GroundItem",
                "Items/Pickaxes/StonePickaxe/Sprite",
                "Items/Pickaxes/StonePickaxe/Equipped",
                false) {}
        }

        public class GoldPickaxe : ItemType
        {
            public GoldPickaxe() : base(
                "GoldPickaxe",
                "Items/Pickaxes/GoldPickaxe/GroundItem",
                "Items/Pickaxes/GoldPickaxe/Sprite",
                "Items/Pickaxes/GoldPickaxe/Equipped",
                false) {}
        }

        public class SunstalPickaxe : ItemType
        {
            public SunstalPickaxe() : base(
                "SunstalPickaxe",
                "Items/Pickaxes/SunstalPickaxe/GroundItem",
                "Items/Pickaxes/SunstalPickaxe/Sprite",
                "Items/Pickaxes/SunstalPickaxe/Equipped",
                false) {}
        }
    }
}
