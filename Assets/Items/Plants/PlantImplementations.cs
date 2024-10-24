using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Spawning;

namespace ItemMetaData
{
    public class ItemImplementations
    {
        public static List<ItemType> BUSHES = new List<ItemType>()
        {
            new BlueBerry(),
            new BlueBerrySeed(),
            new OrangeBerry(),
            new OrangeBerrySeed(),
            new RedBerry(),
            new RedBerrySeed()
        };

        public static List<ItemType> CROPS = new List<ItemType>()
        {
            new CarrotSeed(),
            new Carrot(),
            new FlaxSeed(),
            new Flax(),
            new PotatoSeed(),
            new Potato(),
            new WheatSeed(),
            new Wheat()
        };

        public static List<ItemType> FERNS = new List<ItemType>()
        {
            new ForestFernSeed(),
            new SunFernSeed()
        };

        public static List<ItemType> FLOWERS = new List<ItemType>()
        {
            new BlizzardFlowerSeed(),
            new CharmFlowerSeed(),
            new FireFlowerSeed(),
            new ZapFlowerSeed()
        };

        public static List<ItemType> FRUIT_TREES = new List<ItemType>()
        {
            new OrangeTreeSeed(),
            new Orange(),
            new PeachTreeSeed(),
            new Peach(),
            new PlumTreeSeed(),
            new Plum()
        };

        public static List<ItemType> HERBS = new List<ItemType>()
        {
            new MintSeed(),
            new Mint(),
            new RosemarySeed(),
            new Rosemary(),
            new SageSeed(),
            new Sage()
        };

        public static List<ItemType> TREES = new List<ItemType>()
        {
            new CherryBlossomSeed(),
            new FaeStaff(),
            new FaeWand(),
            new LightningTreeSeed(),
            new LightningStaff(),
            new LightningWand(),
            new RedMapleSeed(),
            new FireStaff(),
            new FireWand(),
            new WeepingWillowSeed(),
            new WaterStaff(),
            new WaterWand()
        };
    }

    // Bush Items

    public class BlueBerrySeed : ItemType
    {
        public BlueBerrySeed() : base(
            "BlueBerrySeed",
            "Plants/_Bushes/BlueBerryBush/SeedGroundItem",
            "Plants/_Bushes/BlueBerryBush/Sprite",
            "Plants/_Bushes/BlueBerryBush/SeedEquipped") {}
    }

    public class BlueBerry : ItemType
    {
        public BlueBerry() : base(
            "BlueBerry",
            "Plants/_Bushes/BlueBerryBush/Yield/GroundItem",
            "Plants/_Bushes/BlueBerryBush/Yield/Sprite",
            "Plants/_Bushes/BlueBerryBush/Yield/Equipped") {}
    }

    public class OrangeBerrySeed : ItemType
    {
        public OrangeBerrySeed() : base(
            "OrangeBerrySeed",
            "Plants/_Bushes/OrangeBerryBush/SeedGroundItem",
            "Plants/_Bushes/OrangeBerryBush/Sprite",
            "Plants/_Bushes/OrangeBerryBush/SeedEquipped") {}
    }

    public class OrangeBerry : ItemType
    {
        public OrangeBerry() : base(
            "OrangeBerry",
            "Plants/_Bushes/OrangeBerryBush/Yield/GroundItem",
            "Plants/_Bushes/OrangeBerryBush/Yield/Sprite",
            "Plants/_Bushes/OrangeBerryBush/Yield/Equipped") {}
    }

    public class RedBerrySeed : ItemType
    {
        public RedBerrySeed() : base(
            "RedBerrySeed",
            "Plants/_Bushes/RedBerryBush/SeedGroundItem",
            "Plants/_Bushes/RedBerryBush/Sprite",
            "Plants/_Bushes/RedBerryBush/SeedEquipped") {}
    }

    public class RedBerry : ItemType
    {
        public RedBerry() : base(
            "RedBerry",
            "Plants/_Bushes/RedBerryBush/Yield/GroundItem",
            "Plants/_Bushes/RedBerryBush/Yield/Sprite",
            "Plants/_Bushes/RedBerryBush/Yield/Equipped") {}
    }

    // Crop Items

    public class CarrotSeed : ItemType
    {
        public CarrotSeed() : base(
            "CarrotSeed",
            "Plants/_Crops/Carrots/SeedGroundItem",
            "Plants/_Crops/Carrots/Sprite",
            "Plants/_Crops/Carrots/SeedEquipped") {}
    }

    public class Carrot : ItemType
    {
        public Carrot() : base(
            "Carrot",
            "Plants/_Crops/Carrots/Yield/GroundItem",
            "Plants/_Crops/Carrots/Yield/Sprite",
            "Plants/_Crops/Carrots/Yield/Equipped") {}
    }

    public class FlaxSeed : ItemType
    {
        public FlaxSeed() : base(
            "FlaxSeed",
            "Plants/_Crops/Flax/SeedGroundItem",
            "Plants/_Crops/Flax/Sprite",
            "Plants/_Crops/Flax/SeedEquipped") {}
    }

    public class Flax : ItemType
    {
        public Flax() : base(
            "Flax",
            "Plants/_Crops/Flax/Yield/GroundItem",
            "Plants/_Crops/Flax/Yield/Sprite",
            "Plants/_Crops/Flax/Yield/Equipped") {}
    }

    public class PotatoSeed : ItemType
    {
        public PotatoSeed() : base(
            "PotatoSeed",
            "Plants/_Crops/Potatoes/SeedGroundItem",
            "Plants/_Crops/Potatoes/Sprite",
            "Plants/_Crops/Potatoes/SeedEquipped") {}
    }

    public class Potato : ItemType
    {
        public Potato() : base(
            "Potato",
            "Plants/_Crops/Potatoes/Yield/GroundItem",
            "Plants/_Crops/Potatoes/Yield/Sprite",
            "Plants/_Crops/Potatoes/Yield/Equipped") {}
    }

    public class WheatSeed : ItemType
    {
        public WheatSeed() : base(
            "WheatSeed",
            "Plants/_Crops/Wheat/SeedGroundItem",
            "Plants/_Crops/Wheat/Sprite",
            "Plants/_Crops/Wheat/SeedEquipped") {}
    }

    public class Wheat : ItemType
    {
        public Wheat() : base(
            "Wheat",
            "Plants/_Crops/Wheat/Yield/GroundItem",
            "Plants/_Crops/Wheat/Yield/Sprite",
            "Plants/_Crops/Wheat/Yield/Equipped") {}
    }

    // Fern Items

    public class ForestFernSeed : ItemType
    {
        public ForestFernSeed() : base(
            "ForestFernSeed",
            "Plants/_Ferns/ForestFern/SeedGroundItem",
            "Plants/_Ferns/ForestFern/Sprite",
            "Plants/_Ferns/ForestFern/SeedEquipped") {}
    }

    public class SunFernSeed : ItemType
    {
        public SunFernSeed() : base(
            "SunFernSeed",
            "Plants/_Ferns/SunFern/SeedGroundItem",
            "Plants/_Ferns/SunFern/Sprite",
            "Plants/_Ferns/SunFern/SeedEquipped") {}
    }

    // Flower Items

    public class BlizzardFlowerSeed : ItemType
    {
        public BlizzardFlowerSeed() : base(
            "BlizzardFlowerSeed",
            "Plants/_Flowers/BlizzardFlower/SeedGroundItem",
            "Plants/_Flowers/BlizzardFlower/Sprite",
            "Plants/_Flowers/BlizzardFlower/SeedEquipped") {}
    }

    public class CharmFlowerSeed : ItemType
    {
        public CharmFlowerSeed() : base(
            "CharmFlowerSeed",
            "Plants/_Flowers/CharmFlower/SeedGroundItem",
            "Plants/_Flowers/CharmFlower/Sprite",
            "Plants/_Flowers/CharmFlower/SeedEquipped") {}
    }

    public class FireFlowerSeed : ItemType
    {
        public FireFlowerSeed() : base(
            "FireFlowerSeed",
            "Plants/_Flowers/FireFlower/SeedGroundItem",
            "Plants/_Flowers/FireFlower/Sprite",
            "Plants/_Flowers/FireFlower/SeedEquipped") {}
    }

    public class ZapFlowerSeed : ItemType
    {
        public ZapFlowerSeed() : base(
            "ZapFlowerSeed",
            "Plants/_Flowers/ZapFlower/SeedGroundItem",
            "Plants/_Flowers/ZapFlower/Sprite",
            "Plants/_Flowers/ZapFlower/SeedEquipped") {}
    }

    // Fruit Tree Items

    public class OrangeTreeSeed : ItemType
    {
        public OrangeTreeSeed() : base(
            "OrangeTreeSeed",
            "Plants/_FruitTrees/OrangeTree/SeedGroundItem",
            "Plants/_FruitTrees/OrangeTree/Sprite",
            "Plants/_FruitTrees/OrangeTree/SeedEquipped") {}
    }

    public class Orange : ItemType
    {
        public Orange() : base(
            "Orange",
            "Plants/_FruitTrees/OrangeTree/Yield/GroundItem",
            "Plants/_FruitTrees/OrangeTree/Yield/Sprite",
            "Plants/_FruitTrees/OrangeTree/Yield/Equipped") {}
    }

    public class PeachTreeSeed : ItemType
    {
        public PeachTreeSeed() : base(
            "PeachTreeSeed",
            "Plants/_FruitTrees/PeachTree/SeedGroundItem",
            "Plants/_FruitTrees/PeachTree/Sprite",
            "Plants/_FruitTrees/PeachTree/SeedEquipped") {}
    }

    public class Peach : ItemType
    {
        public Peach() : base(
            "Peach",
            "Plants/_FruitTrees/PeachTree/Yield/GroundItem",
            "Plants/_FruitTrees/PeachTree/Yield/Sprite",
            "Plants/_FruitTrees/PeachTree/Yield/Equipped") {}
    }

    public class PlumTreeSeed : ItemType
    {
        public PlumTreeSeed() : base(
            "PlumTreeSeed",
            "Plants/_FruitTrees/PlumTree/SeedGroundItem",
            "Plants/_FruitTrees/PlumTree/Sprite",
            "Plants/_FruitTrees/PlumTree/SeedEquipped") {}
    }

    public class Plum : ItemType
    {
        public Plum() : base(
            "Plum",
            "Plants/_FruitTrees/PlumTree/Yield/GroundItem",
            "Plants/_FruitTrees/PlumTree/Yield/Sprite",
            "Plants/_FruitTrees/PlumTree/Yield/Equipped") {}
    }

    // Herb Items

    public class MintSeed : ItemType
    {
        public MintSeed() : base(
            "MintSeed",
            "Plants/_Herbs/Mint/SeedGroundItem",
            "Plants/_Herbs/Mint/Sprite",
            "Plants/_Herbs/Mint/SeedEquipped") {}
    }

    public class Mint : ItemType
    {
        public Mint() : base(
            "Mint",
            "Plants/_Herbs/Mint/Yield/GroundItem",
            "Plants/_Herbs/Mint/Yield/Sprite",
            "Plants/_Herbs/Mint/Yield/Equipped") {}
    }

    public class RosemarySeed : ItemType
    {
        public RosemarySeed() : base(
            "RosemarySeed",
            "Plants/_Herbs/Rosemary/SeedGroundItem",
            "Plants/_Herbs/Rosemary/Sprite",
            "Plants/_Herbs/Rosemary/SeedEquipped") {}
    }

    public class Rosemary : ItemType
    {
        public Rosemary() : base(
            "Rosemary",
            "Plants/_Herbs/Rosemary/Yield/GroundItem",
            "Plants/_Herbs/Rosemary/Yield/Sprite",
            "Plants/_Herbs/Rosemary/Yield/Equipped") {}
    }

    public class SageSeed : ItemType
    {
        public SageSeed() : base(
            "SageSeed",
            "Plants/_Herbs/Sage/SeedGroundItem",
            "Plants/_Herbs/Sage/Sprite",
            "Plants/_Herbs/Sage/SeedEquipped") {}
    }

    public class Sage : ItemType
    {
        public Sage() : base(
            "Sage",
            "Plants/_Herbs/Sage/Yield/GroundItem",
            "Plants/_Herbs/Sage/Yield/Sprite",
            "Plants/_Herbs/Sage/Yield/Equipped") {}
    }

    // Trees Items

    public class CherryBlossomSeed : ItemType
    {
        public CherryBlossomSeed() : base(
            "CherryBlossomSeed",
            "Plants/_Trees/CherryBlossom/SeedGroundItem",
            "Plants/_Trees/CherryBlossom/Sprite",
            "Plants/_Trees/CherryBlossom/SeedEquipped") {}
    }

    public class FaeStaff : ItemType
    {
        public FaeStaff() : base(
            "FaeStaff",
            "Plants/_Trees/CherryBlossom/Yield/Staff/GroundItem",
            "Plants/_Trees/CherryBlossom/Yield/Staff/Sprite",
            "Plants/_Trees/CherryBlossom/Yield/Staff/Equipped") {}
    }

    public class FaeWand : ItemType
    {
        public FaeWand() : base(
            "FaeWand",
            "Plants/_Trees/CherryBlossom/Yield/Wand/GroundItem",
            "Plants/_Trees/CherryBlossom/Yield/Wand/Sprite",
            "Plants/_Trees/CherryBlossom/Yield/Wand/Equipped") {}
    }

    public class LightningTreeSeed : ItemType
    {
        public LightningTreeSeed() : base(
            "LightningTreeSeed",
            "Plants/_Trees/LightningTree/SeedGroundItem",
            "Plants/_Trees/LightningTree/Sprite",
            "Plants/_Trees/LightningTree/SeedEquipped") {}
    }

    public class LightningStaff : ItemType
    {
        public LightningStaff() : base(
            "LightningStaff",
            "Plants/_Trees/LightningTree/Yield/Staff/GroundItem",
            "Plants/_Trees/LightningTree/Yield/Staff/Sprite",
            "Plants/_Trees/LightningTree/Yield/Staff/Equipped") {}
    }

    public class LightningWand : ItemType
    {
        public LightningWand() : base(
            "LightningWand",
            "Plants/_Trees/LightningTree/Yield/Wand/GroundItem",
            "Plants/_Trees/LightningTree/Yield/Wand/Sprite",
            "Plants/_Trees/LightningTree/Yield/Wand/Equipped") {}
    }

    public class RedMapleSeed : ItemType
    {
        public RedMapleSeed() : base(
            "RedMapleSeed",
            "Plants/_Trees/RedMaple/SeedGroundItem",
            "Plants/_Trees/RedMaple/Sprite",
            "Plants/_Trees/RedMaple/SeedEquipped") {}
    }

    public class FireStaff : ItemType
    {
        public FireStaff() : base(
            "FireStaff",
            "Plants/_Trees/RedMaple/Yield/Staff/GroundItem",
            "Plants/_Trees/RedMaple/Yield/Staff/Sprite",
            "Plants/_Trees/RedMaple/Yield/Staff/Equipped") {}
    }

    public class FireWand : ItemType
    {
        public FireWand() : base(
            "FireWand",
            "Plants/_Trees/RedMaple/Yield/Wand/GroundItem",
            "Plants/_Trees/RedMaple/Yield/Wand/Sprite",
            "Plants/_Trees/RedMaple/Yield/Wand/Equipped") {}
    }

    public class WeepingWillowSeed : ItemType
    {
        public WeepingWillowSeed() : base(
            "WeepingWillowSeed",
            "Plants/_Trees/WeepingWillow/SeedGroundItem",
            "Plants/_Trees/WeepingWillow/Sprite",
            "Plants/_Trees/WeepingWillow/SeedEquipped") {}
    }

    public class WaterStaff : ItemType
    {
        public WaterStaff() : base(
            "WaterStaff",
            "Plants/_Trees/WeepingWillow/Yield/Staff/GroundItem",
            "Plants/_Trees/WeepingWillow/Yield/Staff/Sprite",
            "Plants/_Trees/WeepingWillow/Yield/Staff/Equipped") {}
    }

    public class WaterWand : ItemType
    {
        public WaterWand() : base(
            "WaterWand",
            "Plants/_Trees/WeepingWillow/Yield/Wand/GroundItem",
            "Plants/_Trees/WeepingWillow/Yield/Wand/Sprite",
            "Plants/_Trees/WeepingWillow/Yield/Wand/Equipped") {}
    }
}
