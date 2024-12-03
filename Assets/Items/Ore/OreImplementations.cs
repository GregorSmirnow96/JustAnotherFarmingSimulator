using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Spawning;

namespace ItemMetaData
{
    public class IronOre : ItemType
    {
        public IronOre() : base(
            "IronOre",
            "Environment/MinableRocks/Rocks/Iron/Ore/GroundItem",
            "Environment/MinableRocks/Rocks/Iron/Ore/Sprite",
            "Environment/MinableRocks/Rocks/Iron/Ore/Equipped") {}
    }

    public class IronBar : ItemType
    {
        public IronBar() : base(
            "IronBar",
            "Environment/MinableRocks/Rocks/Iron/Bar/GroundItem",
            "Environment/MinableRocks/Rocks/Iron/Bar/Sprite",
            "Environment/MinableRocks/Rocks/Iron/Bar/Equipped") {}
    }

    public class CopperOre : ItemType
    {
        public CopperOre() : base(
            "CopperOre",
            "Environment/MinableRocks/Veins/Copper/Ore/GroundItem",
            "Environment/MinableRocks/Veins/Copper/Ore/Sprite",
            "Environment/MinableRocks/Veins/Copper/Ore/Equipped") {}
    }
}
