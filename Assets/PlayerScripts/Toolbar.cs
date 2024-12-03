using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

using ItemMetaData;

public class Toolbar : MonoBehaviour
{
    public static Toolbar instance;

    public const int inventorySize = 5;
    public int equippedItemIndex = 0;
    public float autoScrollEnableTime = 3f;

    private bool scrollingEnabled;
    private float scrollDisabledTime;

    public bool IsFull => Inventory.instance.toolbarItems.All(id => id != null);

    void Awake()
    {
        instance = this;
        scrollingEnabled = true;
    }

    void Update()
    {
        if (!scrollingEnabled)
        {
            if (Time.time - scrollDisabledTime >= autoScrollEnableTime)
            scrollingEnabled = true;
        }

        if (scrollingEnabled)
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll > 0.0f)
            {
                equippedItemIndex = (equippedItemIndex + 1) % inventorySize;
            }
            else if (scroll < 0.0f)
            {
                equippedItemIndex = (equippedItemIndex + inventorySize - 1) % inventorySize;
            }
        }
    }

    public void SetScrollingEnabled(bool scrollingEnabled)
    {
        this.scrollingEnabled = scrollingEnabled;
        scrollDisabledTime = Time.time;
    }

    public Item GetItem(int i) => Inventory.instance.toolbarItems[i];

    public bool AddItem(Item newItem)
    {
        int nextOpenIndex = -1;
        int currentIndex = 0;
        foreach (Item item in Inventory.instance.toolbarItems)
        {
            if (item == null)
            {
                nextOpenIndex = currentIndex;
                break;
            }

            currentIndex++;
        }

        if (nextOpenIndex == -1)
        {
            Debug.Log("Your inventory is full");
            return false;
        }

        SetInventorySlot(nextOpenIndex, newItem);
        return true;
    }

    public void SetInventorySlot(int index, Item item)
    {
        if (index < 0 && index >= Inventory.instance.toolbarItems.Length)
        {
            Debug.Log($"Cannot set item in inventory at index {index}. This is out of bounds");
            return;
        }

        Item currentItem = Inventory.instance.toolbarItems[index];
        if (currentItem != null && item != null)
        {
            Debug.Log($"Overwrote an item in the inventory at index: {index}. Should the item be dropped? Should this operation fail?");
        }

        Inventory.instance.SetToolbarItem(index, item);
    }

    public void DeleteItemInSlot(int index)
    {
        SetInventorySlot(index, null);
    }

    public Item GetEquippedItem()
    {
        return Inventory.instance.toolbarItems[equippedItemIndex];
    }

    public void DeleteEquippedItem() => DeleteItemInSlot(equippedItemIndex);
}
