using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ItemMetaData
{
    public class JewelryImplementations
    {
        public static List<ItemType> JEWELS = new List<ItemType>()
        {
            new Aquamarine(),
            new Onyx(),
            new Ruby(),
            new Topaz()
        };

        public static List<ItemType> NECKLACES = new List<ItemType>()
        {
            new AquamarineNecklace(),
            new OnyxNecklace(),
            new RubyNecklace(),
            new TopazNecklace()
        };

        public static List<ItemType> RINGS = new List<ItemType>()
        {
            new AquamarineRing(),
            new OnyxRing(),
            new RubyRing(),
            new TopazRing()
        };

        // Jewels

        public class Aquamarine : ItemType
        {
            public Aquamarine() : base(
                "Aquamarine",
                "Items/Jewelry/Aquamarine/GroundItem",
                "Items/Jewelry/Aquamarine/Sprite",
                "Items/Jewelry/Aquamarine/Equipped",
                false) {}
        }

        public class Onyx : ItemType
        {
            public Onyx() : base(
                "Onyx",
                "Items/Jewelry/Onyx/GroundItem",
                "Items/Jewelry/Onyx/Sprite",
                "Items/Jewelry/Onyx/Equipped",
                false) {}
        }

        public class Ruby : ItemType
        {
            public Ruby() : base(
                "Ruby",
                "Items/Jewelry/Ruby/GroundItem",
                "Items/Jewelry/Ruby/Sprite",
                "Items/Jewelry/Ruby/Equipped",
                false) {}
        }

        public class Topaz : ItemType
        {
            public Topaz() : base(
                "Topaz",
                "Items/Jewelry/Topaz/GroundItem",
                "Items/Jewelry/Topaz/Sprite",
                "Items/Jewelry/Topaz/Equipped",
                false) {}
        }

        // Necklaces

        public class AquamarineNecklace : ItemType
        {
            public AquamarineNecklace() : base(
                "AquamarineNecklace",
                "Items/Jewelry/AquamarineNecklace/GroundItem",
                "Items/Jewelry/AquamarineNecklace/Sprite",
                "Items/Jewelry/AquamarineNecklace/Equipped",
                false) {}
        }

        public class OnyxNecklace : ItemType
        {
            public OnyxNecklace() : base(
                "OnyxNecklace",
                "Items/Jewelry/OnyxNecklace/GroundItem",
                "Items/Jewelry/OnyxNecklace/Sprite",
                "Items/Jewelry/OnyxNecklace/Equipped",
                false) {}
        }

        public class RubyNecklace : ItemType
        {
            public RubyNecklace() : base(
                "RubyNecklace",
                "Items/Jewelry/RubyNecklace/GroundItem",
                "Items/Jewelry/RubyNecklace/Sprite",
                "Items/Jewelry/RubyNecklace/Equipped",
                false) {}
        }

        public class TopazNecklace : ItemType
        {
            public TopazNecklace() : base(
                "TopazNecklace",
                "Items/Jewelry/TopazNecklace/GroundItem",
                "Items/Jewelry/TopazNecklace/Sprite",
                "Items/Jewelry/TopazNecklace/Equipped",
                false) {}
        }

        // Rings

        public class AquamarineRing : ItemType
        {
            public AquamarineRing() : base(
                "AquamarineRing",
                "Items/Jewelry/AquamarineRing/GroundItem",
                "Items/Jewelry/AquamarineRing/Sprite",
                "Items/Jewelry/AquamarineRing/Equipped",
                false) {}
        }

        public class OnyxRing : ItemType
        {
            public OnyxRing() : base(
                "OnyxRing",
                "Items/Jewelry/OnyxRing/GroundItem",
                "Items/Jewelry/OnyxRing/Sprite",
                "Items/Jewelry/OnyxRing/Equipped",
                false) {}
        }

        public class RubyRing : ItemType
        {
            public RubyRing() : base(
                "RubyRing",
                "Items/Jewelry/RubyRing/GroundItem",
                "Items/Jewelry/RubyRing/Sprite",
                "Items/Jewelry/RubyRing/Equipped",
                false) {}
        }

        public class TopazRing : ItemType
        {
            public TopazRing() : base(
                "TopazRing",
                "Items/Jewelry/TopazRing/GroundItem",
                "Items/Jewelry/TopazRing/Sprite",
                "Items/Jewelry/TopazRing/Equipped",
                false) {}
        }
    }
}
