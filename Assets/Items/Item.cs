using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ItemMetaData;

// This can be extended so that each item can have data specific to it (like potion items having properties)
public class Item
{
    public ItemType type;
    public Guid guid;
    // Pick up here tomorrow: Implement numbers over item sprites in the inventory for stacks of items.
    // This shouldn't require any refactoring in the Item class itself. The inventory modification functions WILL need refactoring though. Specifically adding/removing items.
    //  1) If an item is being added to the inventory that stacks and it already exists, it should be added to the stack.
    //  2) A remove from stack function will be needed. It will be used by the crafting button, alchemy button, and upon firing an arrow (remove 1 of the equipped arrow). 
    public int stackSize = 1;

    public bool stacks => type?.stacks ?? false;

    public Item(ItemType itemType)
    {
        type = itemType;
        guid = Guid.NewGuid();
    }

    public Item(string itemId)
    {
        type = ItemTypeRepo.GetInstance().TryFindItemType(itemId);
        guid = Guid.NewGuid();
    }

    // Like item specific data, this can be extended by item specific classes.
    // This could also come from the ItemType if the extension doesn't override?
    public virtual string GetTooltipText()
    {
        return type.id;
    }
}
