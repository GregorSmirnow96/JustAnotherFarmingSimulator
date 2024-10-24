using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ItemMetaData;

public class RenderEquippedItem : MonoBehaviour
{
    public static RenderEquippedItem instance;

    public GameObject equippedItemContainer;

    private Toolbar playerToolbar;
    private string renderedId;
    public GameObject renderedItem;

    void Start()
    {
        playerToolbar = gameObject.GetComponent<Toolbar>();
    }

    private bool printed = false;
    void Update()
    {
        instance = this;

        Item equippedItem = playerToolbar.GetEquippedItem();
        string equippedItemId = equippedItem?.type.id;
        if (equippedItemId == null)
        {
            if (renderedItem != null)
            {
                Destroy(renderedItem);
            }
            renderedId = equippedItemId;
            return;
        }

        ItemType equippedItemType = ItemTypeRepo.GetInstance().TryFindItemType(equippedItemId);
        if (equippedItemType == null)
        {
            Debug.Log($"Equipped item ID {equippedItemId} doesn't correspond to an item");
            return;
        }
        
        if (equippedItemType.equippedPrefab == null)
        {
            Destroy(renderedItem);
            renderedId = equippedItemId;
            renderedItem = null;
            Debug.Log($"Equipped item has no equipped prefab");
        }
        
        bool itemIsAlreadyRendered = equippedItemId == renderedId;
        if (!itemIsAlreadyRendered)
        {
            Destroy(renderedItem);
            renderedItem = Instantiate(equippedItemType.equippedPrefab, equippedItemContainer.transform);
            renderedId = equippedItemId;
        }
    }
}
