using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Spawning;

namespace ItemMetaData
{
    public class ItemTypeRepo
    {
        private static ItemTypeRepo Instance;
        public static ItemTypeRepo GetInstance() =>
            Instance == null
                ? Instance = new ItemTypeRepo()
                : Instance;

        private List<ItemType> items;

        private ItemTypeRepo()
        {
            items = new List<ItemType>();

            items.Add(new Beacon());
            items.Add(new CampfireKit());

            items.Add(new Staff());
            items.Add(new Wand());
            items.Add(new PinewoodBow());
            items.Add(new FruitwoodBow());
            items.Add(new SpiritwoodBow());
            items.Add(new PinewoodLog());
            items.Add(new FruitwoodLog());
            items.Add(new SpiritwoodLog());
            items.Add(new Bowl());
            items.Add(new BowlOfWater());
            items.Add(new KingBolete());
            items.Add(new OrangeFly());
            items.Add(new YellowFieldcap());

            items.Add(new ButterflyNet());
            items.Add(new BlueButterflyWing());
            items.Add(new RedButterflyWing());
            items.Add(new YellowButterflyWing());

            items.Add(new IronBomb());
            items.Add(new SilverBomb());
            items.Add(new GoldBomb());
            items.Add(new MoonstoneBomb());
            items.Add(new SunstalBomb());

            items.AddRange(MetalImplementations.ORES);
            items.AddRange(MetalImplementations.BARS);
            items.AddRange(MetalImplementations.DUSTS);

            items.AddRange(ArrowImplementations.ARROWS);
            items.AddRange(ArmourImplementations.BODY_ARMOURS);
            items.AddRange(JewelryImplementations.JEWELS);
            items.AddRange(JewelryImplementations.NECKLACES);
            items.AddRange(JewelryImplementations.RINGS);

            items.AddRange(WeaponImplementations.SWORDS);
            items.AddRange(WeaponImplementations.AXES);
            items.AddRange(WeaponImplementations.PICKAXES);

            items.AddRange(PlantImplementations.BUSHES);
            items.AddRange(PlantImplementations.CROPS);
            items.AddRange(PlantImplementations.FERNS);
            items.AddRange(PlantImplementations.FLOWERS);
            items.AddRange(PlantImplementations.HERBS);
            items.AddRange(PlantImplementations.FRUIT_TREES);
            items.AddRange(PlantImplementations.TREES);

            items.Add(new Potion());
        }

        public ItemType TryFindItemType(string itemId)
        {
            ItemType queriedType = items?.FirstOrDefault(item => item.id.Equals(itemId));
            return queriedType;
            /*
            return queriedType == null
                ? null
                : new ItemType(queriedType.id, queriedType.groundItemPrefab, queriedType.inventorySprite, queriedType.equippedPrefab, queriedType.stacks);
            */
        }
    }

    public class ItemType
    {
        public string id;
        public GameObject groundItemPrefab;
        public Sprite inventorySprite;
        public GameObject equippedPrefab;
        public bool stacks;

        public ItemType(
            string id,
            string groundItemPrefabPath,
            string inventorySpritePath,
            string equippedPrefabPath,
            bool stacks)
        {
            this.id = id;
            this.groundItemPrefab = Resources.Load<GameObject>(groundItemPrefabPath);
            this.inventorySprite = Resources.Load<Sprite>(inventorySpritePath);
            this.equippedPrefab = Resources.Load<GameObject>(equippedPrefabPath);
            this.stacks = stacks;
        }

        public ItemType(
            string id,
            GameObject groundItemPrefab,
            Sprite inventorySprite,
            GameObject equippedPrefab,
            bool stacks)
        {
            this.id = id;
            this.groundItemPrefab = groundItemPrefab;
            this.inventorySprite = inventorySprite;
            this.equippedPrefab = equippedPrefab;
            this.stacks = stacks;
        }
    }

    public class Shovel : ItemType
    {
        public Shovel() : base(
            "Shovel",
            "Tools/Shovel/GroundItem",
            "Tools/Shovel/Sprite",
            "EquippedItems/Shovel/Shovel",
            false) {}
    }

    public class Staff : ItemType
    {
        public Staff() : base(
            "Staff",
            "CraftedItems/Wood/Staff/GroundItem",
            "CraftedItems/Wood/Staff/Sprite",
            "CraftedItems/Wood/Staff/Equipped",
            false) {}
    }

    public class Wand : ItemType
    {
        public Wand() : base(
            "Wand",
            "CraftedItems/Wood/Wand/GroundItem",
            "CraftedItems/Wood/Wand/Sprite",
            "CraftedItems/Wood/Wand/Equipped",
            false) {}
    }

    public class PinewoodBow : ItemType
    {
        public PinewoodBow() : base(
            "PinewoodBow",
            "Items/Bows/PinewoodBow/GroundItem",
            "Items/Bows/PinewoodBow/Sprite",
            "Items/Bows/PinewoodBow/Equipped",
            false) {}
    }

    public class FruitwoodBow : ItemType
    {
        public FruitwoodBow() : base(
            "FruitwoodBow",
            "Items/Bows/FruitwoodBow/GroundItem",
            "Items/Bows/FruitwoodBow/Sprite",
            "Items/Bows/FruitwoodBow/Equipped",
            false) {}
    }

    public class SpiritwoodBow : ItemType
    {
        public SpiritwoodBow() : base(
            "SpiritwoodBow",
            "Items/Bows/SpiritwoodBow/GroundItem",
            "Items/Bows/SpiritwoodBow/Sprite",
            "Items/Bows/SpiritwoodBow/Equipped",
            false) {}
    }

    public class PinewoodLog : ItemType
    {
        public PinewoodLog() : base(
            "PinewoodLog",
            "Items/Logs/PinewoodLog/GroundItem",
            "Items/Logs/PinewoodLog/Sprite",
            "Items/Logs/PinewoodLog/Equipped",
            false) {}
    }

    public class FruitwoodLog : ItemType
    {
        public FruitwoodLog() : base(
            "FruitwoodLog",
            "Items/Logs/FruitwoodLog/GroundItem",
            "Items/Logs/FruitwoodLog/Sprite",
            "Items/Logs/FruitwoodLog/Equipped",
            false) {}
    }

    public class SpiritwoodLog : ItemType
    {
        public SpiritwoodLog() : base(
            "SpiritwoodLog",
            "Items/Logs/SpiritwoodLog/GroundItem",
            "Items/Logs/SpiritwoodLog/Sprite",
            "Items/Logs/SpiritwoodLog/Equipped",
            false) {}
    }

    public class Bowl : ItemType
    {
        public Bowl() : base(
            "Bowl",
            "CraftedItems/Wood/Bowl/GroundItem",
            "CraftedItems/Wood/Bowl/Sprite",
            "CraftedItems/Wood/Bowl/EquippedItem",
            false) {}
    }

    public class BowlOfWater : ItemType
    {
        public BowlOfWater() : base(
            "BowlOfWater",
            "CraftedItems/Wood/BowlOfWater/GroundItem",
            "CraftedItems/Wood/BowlOfWater/Sprite",
            "CraftedItems/Wood/BowlOfWater/EquippedItem",
            false) {}
    }

    public class KingBolete : ItemType
    {
        public KingBolete() : base(
            "KingBolete",
            "Items/Mushrooms/KingBolete/GroundItem",
            "Items/Mushrooms/KingBolete/Sprite",
            "Items/Mushrooms/KingBolete/Equipped",
            false) {}
    }

    public class OrangeFly : ItemType
    {
        public OrangeFly() : base(
            "OrangeFly",
            "Items/Mushrooms/OrangeFly/GroundItem",
            "Items/Mushrooms/OrangeFly/Sprite",
            "Items/Mushrooms/OrangeFly/Equipped",
            false) {}
    }

    public class YellowFieldcap : ItemType
    {
        public YellowFieldcap() : base(
            "YellowFieldcap",
            "Items/Mushrooms/YellowFieldcap/GroundItem",
            "Items/Mushrooms/YellowFieldcap/Sprite",
            "Items/Mushrooms/YellowFieldcap/Equipped",
            false) {}
    }

    public class ButterflyNet : ItemType
    {
        public ButterflyNet() : base(
            "ButterflyNet",
            "Items/Butterflies/ButterflyNet/GroundItem",
            "Items/Butterflies/ButterflyNet/Sprite",
            "Items/Butterflies/ButterflyNet/Equipped",
            false) {}
    }

    public class BlueButterflyWing : ItemType
    {
        public BlueButterflyWing() : base(
            "BlueButterflyWing",
            "Items/Butterflies/BlueButterflyWing/GroundItem",
            "Items/Butterflies/BlueButterflyWing/Sprite",
            "Items/Butterflies/BlueButterflyWing/Equipped",
            false) {}
    }

    public class RedButterflyWing : ItemType
    {
        public RedButterflyWing() : base(
            "RedButterflyWing",
            "Items/Butterflies/RedButterflyWing/GroundItem",
            "Items/Butterflies/RedButterflyWing/Sprite",
            "Items/Butterflies/RedButterflyWing/Equipped",
            false) {}
    }

    public class YellowButterflyWing : ItemType
    {
        public YellowButterflyWing() : base(
            "YellowButterflyWing",
            "Items/Butterflies/YellowButterflyWing/GroundItem",
            "Items/Butterflies/YellowButterflyWing/Sprite",
            "Items/Butterflies/YellowButterflyWing/Equipped",
            false) {}
    }

    public class IronBomb : ItemType
    {
        public IronBomb() : base(
            "IronBomb",
            "Items/Bombs/IronBomb/GroundItem",
            "Items/Bombs/IronBomb/Sprite",
            "Items/Bombs/IronBomb/Equipped",
            false) {}
    }

    public class SilverBomb : ItemType
    {
        public SilverBomb() : base(
            "SilverBomb",
            "Items/Bombs/SilverBomb/GroundItem",
            "Items/Bombs/SilverBomb/Sprite",
            "Items/Bombs/SilverBomb/Equipped",
            false) {}
    }

    public class GoldBomb : ItemType
    {
        public GoldBomb() : base(
            "GoldBomb",
            "Items/Bombs/GoldBomb/GroundItem",
            "Items/Bombs/GoldBomb/Sprite",
            "Items/Bombs/GoldBomb/Equipped",
            false) {}
    }

    public class MoonstoneBomb : ItemType
    {
        public MoonstoneBomb() : base(
            "MoonstoneBomb",
            "Items/Bombs/MoonstoneBomb/GroundItem",
            "Items/Bombs/MoonstoneBomb/Sprite",
            "Items/Bombs/MoonstoneBomb/Equipped",
            false) {}
    }

    public class SunstalBomb : ItemType
    {
        public SunstalBomb() : base(
            "SunstalBomb",
            "Items/Bombs/SunstalBomb/GroundItem",
            "Items/Bombs/SunstalBomb/Sprite",
            "Items/Bombs/SunstalBomb/Equipped",
            false) {}
    }

    public class Beacon : ItemType
    {
        public Beacon() : base(
            "Beacon",
            "Items/LightSources/Beacon/GroundItem",
            "Items/LightSources/Beacon/Sprite",
            "Items/LightSources/Beacon/Equipped",
            false) {}
    }

    public class CampfireKit : ItemType
    {
        public CampfireKit() : base(
            "CampfireKit",
            "Items/LightSources/CampfireKit/GroundItem",
            "Items/LightSources/CampfireKit/Sprite",
            "Items/LightSources/CampfireKit/Equipped",
            false) {}
    }
}
