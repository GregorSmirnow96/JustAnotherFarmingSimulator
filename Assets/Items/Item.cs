using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ItemMetaData;

// This can be extended so that each item can have data specific to it (like potion items having properties)
public class Item
{
    public ItemType type;

    public Item(ItemType itemType)
    {
        type = itemType;
    }

    public Item(string itemId)
    {
        type = ItemTypeRepo.GetInstance().TryFindItemType(itemId);
    }

    // Like item specific data, this can be extended by item specific classes.
    // This could also come from the ItemType if the extension doesn't override?
    public virtual string GetTooltipText()
    {
        return type.id;
    }
}
