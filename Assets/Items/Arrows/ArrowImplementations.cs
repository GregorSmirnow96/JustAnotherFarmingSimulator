using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ItemMetaData
{
    public class ArrowImplementations
    {
        public static List<ItemType> ARROWS = new List<ItemType>()
        {
            new IronArrow()
        };
    }

    public class IronArrow : ArrowItemType
    {
        public IronArrow() : base(
            "IronArrow",
            "CraftedItems/Wood/Arrows/IronArrow/GroundItem",
            "CraftedItems/Wood/Arrows/IronArrow/Sprite",
            "CraftedItems/Wood/Arrows/IronArrow/Equipped") {}
        
        // Add other arrows here!
    }
}
