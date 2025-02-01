using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ItemMetaData
{
    public class ArmourImplementations
    {
        public static List<ItemType> BODY_ARMOURS = new List<ItemType>()
        {
            new RoughTunic(),
            new QualityTunic(),
            new FireTunic(),
            new LightningTunic(),
            new WaterTunic(),
            new FaeTunic(),

            new IronArmour(),
            new SilverArmour(),
            new GoldArmour(),
            new CelestialArmour(),
            new FireArmour(),
            new LightningArmour(),
            new WaterArmour(),
            new FaeArmour()
        };

        // Tunics
        
        public class RoughTunic : ItemType
        {
            public RoughTunic() : base(
                "RoughTunic",
                "Items/Armour/RoughTunic/GroundItem",
                "Items/Armour/RoughTunic/Sprite",
                "Items/Armour/RoughTunic/Equipped",
                false) {}
        }

        public class QualityTunic : ItemType
        {
            public QualityTunic() : base(
                "QualityTunic",
                "Items/Armour/QualityTunic/GroundItem",
                "Items/Armour/QualityTunic/Sprite",
                "Items/Armour/QualityTunic/Equipped",
                false) {}
        }

        public class FireTunic : ItemType
        {
            public FireTunic() : base(
                "FireTunic",
                "Items/Armour/FireTunic/GroundItem",
                "Items/Armour/FireTunic/Sprite",
                "Items/Armour/FireTunic/Equipped",
                false) {}
        }

        public class LightningTunic : ItemType
        {
            public LightningTunic() : base(
                "LightningTunic",
                "Items/Armour/LightningTunic/GroundItem",
                "Items/Armour/LightningTunic/Sprite",
                "Items/Armour/LightningTunic/Equipped",
                false) {}
        }

        public class WaterTunic : ItemType
        {
            public WaterTunic() : base(
                "WaterTunic",
                "Items/Armour/WaterTunic/GroundItem",
                "Items/Armour/WaterTunic/Sprite",
                "Items/Armour/WaterTunic/Equipped",
                false) {}
        }

        public class FaeTunic : ItemType
        {
            public FaeTunic() : base(
                "FaeTunic",
                "Items/Armour/FaeTunic/GroundItem",
                "Items/Armour/FaeTunic/Sprite",
                "Items/Armour/FaeTunic/Equipped",
                false) {}
        }

        // Armours

        public class IronArmour : ItemType
        {
            public IronArmour() : base(
                "IronArmour",
                "Items/Armour/IronArmour/GroundItem",
                "Items/Armour/IronArmour/Sprite",
                "Items/Armour/IronArmour/Equipped",
                false) {}
        }

        public class SilverArmour : ItemType
        {
            public SilverArmour() : base(
                "SilverArmour",
                "Items/Armour/SilverArmour/GroundItem",
                "Items/Armour/SilverArmour/Sprite",
                "Items/Armour/SilverArmour/Equipped",
                false) {}
        }

        public class GoldArmour : ItemType
        {
            public GoldArmour() : base(
                "GoldArmour",
                "Items/Armour/GoldArmour/GroundItem",
                "Items/Armour/GoldArmour/Sprite",
                "Items/Armour/GoldArmour/Equipped",
                false) {}
        }

        public class CelestialArmour : ItemType
        {
            public CelestialArmour() : base(
                "CelestialArmour",
                "Items/Armour/CelestialArmour/GroundItem",
                "Items/Armour/CelestialArmour/Sprite",
                "Items/Armour/CelestialArmour/Equipped",
                false) {}
        }

        public class FireArmour : ItemType
        {
            public FireArmour() : base(
                "FireArmour",
                "Items/Armour/FireArmour/GroundItem",
                "Items/Armour/FireArmour/Sprite",
                "Items/Armour/FireArmour/Equipped",
                false) {}
        }

        public class LightningArmour : ItemType
        {
            public LightningArmour() : base(
                "LightningArmour",
                "Items/Armour/LightningArmour/GroundItem",
                "Items/Armour/LightningArmour/Sprite",
                "Items/Armour/LightningArmour/Equipped",
                false) {}
        }

        public class WaterArmour : ItemType
        {
            public WaterArmour() : base(
                "WaterArmour",
                "Items/Armour/WaterArmour/GroundItem",
                "Items/Armour/WaterArmour/Sprite",
                "Items/Armour/WaterArmour/Equipped",
                false) {}
        }

        public class FaeArmour : ItemType
        {
            public FaeArmour() : base(
                "FaeArmour",
                "Items/Armour/FaeArmour/GroundItem",
                "Items/Armour/FaeArmour/Sprite",
                "Items/Armour/FaeArmour/Equipped",
                false) {}
        }
    }
}
