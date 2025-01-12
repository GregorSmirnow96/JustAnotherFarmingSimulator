using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using ItemMetaData;

public class ArrowItemType : ItemType
{
    public ArrowItemType(
        string id,
        string groundItemPrefabPath,
        string inventorySpritePath,
        string equippedPrefabPath) : base(
            id,
            groundItemPrefabPath,
            inventorySpritePath,
            equippedPrefabPath,
            true)
    {
    }
}
