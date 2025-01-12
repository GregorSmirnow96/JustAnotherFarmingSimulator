using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using ItemMetaData;

namespace ItemMetaData
{
    public class Potion : ItemType
    {
        public Potion() : base(
            "Potion",
            "CraftedItems/Potions/Prefabs/GroundItem",
            "CraftedItems/Potions/UntintedSprite",
            "CraftedItems/Potions/Prefabs/Equipped",
            false) {}
    }
}
