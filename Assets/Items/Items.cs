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
            items.Add(new Shovel());
            items.Add(new Axe());
            items.Add(new Pickaxe());
            items.Add(new Staff());
            items.Add(new Wand());
            items.Add(new Bow());
            items.Add(new Log());
            items.Add(new Bowl());
            items.Add(new BowlOfWater());
            items.Add(new Mushroom1());
            items.Add(new Mushroom2());
            items.Add(new Mushroom3());

            items.AddRange(MetalImplementations.ORES);
            items.AddRange(MetalImplementations.BARS);
            items.AddRange(MetalImplementations.DUSTS);
            items.AddRange(ArrowImplementations.ARROWS);
            items.AddRange(ArmourImplementations.BODY_ARMOURS);

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

    public class Axe : ItemType
    {
        public Axe() : base(
            "Axe",
            "Tools/Axe/GroundItem",
            "Tools/Axe/Sprite",
            "EquippedItems/Axe/Axe",
            false) {}
    }

    public class Pickaxe : ItemType
    {
        public Pickaxe() : base(
            "Pickaxe",
            "Tools/Pickaxe/GroundItem",
            "Tools/Pickaxe/Sprite",
            "EquippedItems/Pickaxe/Equipped",
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

    public class Bow : ItemType
    {
        public Bow() : base(
            "Bow",
            "CraftedItems/Wood/Bow/GroundItem",
            "CraftedItems/Wood/Bow/Sprite",
            "CraftedItems/Wood/Bow/Equipped",
            false) {}
    }

    public class Log : ItemType
    {
        public Log() : base(
            "Log",
            "Environment/ProxyGames/Logs/Log",
            "Environment/ProxyGames/Logs/Sprite",
            "EquippedItems/Logs/Log",
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

    public class Mushroom1 : ItemType
    {
        public Mushroom1() : base(
            "Mushroom1",
            "Environment/ProxyGames/Mushrooms/Mushroom1/Mushroom 1",
            "Environment/ProxyGames/Mushrooms/Mushroom1/Sprite",
            "EquippedItems/Mushrooms/INVALIDPATH",
            false) {}
    }

    public class Mushroom2 : ItemType
    {
        public Mushroom2() : base(
            "Mushroom2",
            "Environment/ProxyGames/Mushrooms/Mushroom2/Mushroom 2",
            "Environment/ProxyGames/Mushrooms/Mushroom2/Sprite",
            "EquippedItems/Mushrooms/INVALIDPATH",
            false) {}
    }

    public class Mushroom3 : ItemType
    {
        public Mushroom3() : base(
            "Mushroom3",
            "Environment/ProxyGames/Mushrooms/Mushroom3/Mushroom 3",
            "Environment/ProxyGames/Mushrooms/Mushroom3/Sprite",
            "EquippedItems/Mushrooms/INVALIDPATH",
            false) {}
    }
}
