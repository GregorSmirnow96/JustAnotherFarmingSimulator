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
            items.Add(new WaterStaff());
            items.Add(new IronOre());
            items.Add(new CopperOre());
            items.Add(new Log());
            items.Add(new Bowl());
            items.Add(new BowlOfWater());
            items.Add(new Mushroom2());

            items.AddRange(ItemImplementations.BUSHES);
            items.AddRange(ItemImplementations.CROPS);
            items.AddRange(ItemImplementations.FERNS);
            items.AddRange(ItemImplementations.FLOWERS);
            items.AddRange(ItemImplementations.HERBS);
            items.AddRange(ItemImplementations.FRUIT_TREES);
            items.AddRange(ItemImplementations.TREES);
        }

        public ItemType TryFindItemType(string itemId) //=> items?.FirstOrDefault(item => item.id.Equals(itemId));
        {
            if (itemId == "OrangeBerry")
            {
                Debug.Log("----------------------------");
                items.ForEach(i => Debug.Log(i.id));
                Debug.Log("----------------------------");
            }
            return items?.FirstOrDefault(item => item.id.Equals(itemId));
        }
    }

    public abstract class ItemType
    {
        public string id;
        public GameObject groundItemPrefab;
        public Sprite inventorySprite;
        public GameObject equippedPrefab;

        public ItemType(
            string id,
            string groundItemPrefabPath,
            string inventorySpritePath,
            string equippedPrefabPath)
        {
            this.id = id;
            this.groundItemPrefab = Resources.Load<GameObject>(groundItemPrefabPath);
            this.inventorySprite = Resources.Load<Sprite>(inventorySpritePath);
            this.equippedPrefab = Resources.Load<GameObject>(equippedPrefabPath);
        }
    }

    public class Shovel : ItemType
    {
        public Shovel() : base(
            "Shovel",
            "Tools/Shovel/GroundItem",
            "Tools/Shovel/Sprite",
            "EquippedItems/Shovel/Shovel") {}
    }

    public class Axe : ItemType
    {
        public Axe() : base(
            "Axe",
            "Tools/Axe/GroundItem",
            "Tools/Axe/Sprite",
            "EquippedItems/Axe/Axe") {}
    }

    public class Pickaxe : ItemType
    {
        public Pickaxe() : base(
            "Pickaxe",
            "Tools/Pickaxe/GroundItem",
            "Tools/Pickaxe/Sprite",
            "EquippedItems/Pickaxe/Equipped") {}
    }

    public class Staff : ItemType
    {
        public Staff() : base(
            "Staff",
            "CraftedItems/Wood/Staff/GroundItem",
            "CraftedItems/Wood/Staff/Sprite",
            "CraftedItems/Wood/Staff/Equipped") {}
    }

    public class Log : ItemType
    {
        public Log() : base(
            "Log",
            "Environment/ProxyGames/Logs/Log",
            "Environment/ProxyGames/Logs/Sprite",
            "EquippedItems/Logs/Log") {}
    }

    public class Bowl : ItemType
    {
        public Bowl() : base(
            "Bowl",
            "CraftedItems/Wood/Bowl/GroundItem",
            "CraftedItems/Wood/Bowl/Sprite",
            "CraftedItems/Wood/Bowl/EquippedItem") {}
    }

    public class BowlOfWater : ItemType
    {
        public BowlOfWater() : base(
            "BowlOfWater",
            "CraftedItems/Wood/BowlOfWater/GroundItem",
            "CraftedItems/Wood/BowlOfWater/Sprite",
            "CraftedItems/Wood/BowlOfWater/EquippedItem") {}
    }

    public class Mushroom2 : ItemType
    {
        public Mushroom2() : base(
            "Mushroom2",
            "Environment/ProxyGames/Mushrooms/Mushroom2/Mushroom 2",
            "Environment/ProxyGames/Mushrooms/Mushroom2/Sprite",
            "EquippedItems/Mushrooms/INVALIDPATH") {}
    }
}
