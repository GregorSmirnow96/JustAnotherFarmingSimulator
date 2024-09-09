using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Spawning;

namespace ItemMetaData
{
    public abstract class Plant : ItemType
    {
        public static readonly string RED_YELLER = "RedYeller";

        public Plant(
            string id,
            string groundItemPrefabPath,
            string inventorySpritePath,
            string equippedPrefabPath) : base(
                id,
                groundItemPrefabPath,
                inventorySpritePath,
                equippedPrefabPath) {}
    }
}
