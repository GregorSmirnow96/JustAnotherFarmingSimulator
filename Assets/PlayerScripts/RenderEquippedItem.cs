using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ItemMetaData;

public class RenderEquippedItem : MonoBehaviour
{
    public static RenderEquippedItem instance;

    public GameObject equippedItemContainer;

    public GameObject renderedItem;

    private Toolbar playerToolbar;
    private string renderedId;
    private string previousRenderedId;
    private Guid? renderedGuid;
    private Guid? previousRenderedGuid;
    
    private List<Action<GameObject>> onItemChangeCallbacks = new List<Action<GameObject>>();

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        playerToolbar = gameObject.GetComponent<Toolbar>();
    }

    private bool printed = false;
    void Update()
    {
        Item equippedItem = playerToolbar.GetEquippedItem();
        string equippedItemId = equippedItem?.type.id;
        Guid? equippedGuid = equippedItem?.guid;
        if (equippedItemId == null)
        {
            if (renderedItem != null)
            {
                Destroy(renderedItem);
            }
            renderedId = null;
            renderedGuid = null;
            renderedItem = null;
        }
        else
        {
            ItemType equippedItemType = ItemTypeRepo.GetInstance().TryFindItemType(equippedItemId);
            if (equippedItemType == null)
            {
                Debug.Log($"Equipped item ID {equippedItemId} doesn't correspond to an item");
            }
            else
            {
                if (equippedItemType.equippedPrefab == null)
                {
                    Destroy(renderedItem);
                    renderedId = equippedItemId;
                    renderedGuid = equippedItem?.guid;
                    renderedItem = null;
                    Debug.Log($"Equipped item has no equipped prefab");
                }

                bool itemIsAlreadyRendered = equippedGuid == renderedGuid;
                if (!itemIsAlreadyRendered)
                {
                    Destroy(renderedItem);
                    renderedItem = Instantiate(equippedItemType.equippedPrefab, equippedItemContainer.transform);
                    EquippedItem equippedItemScript = renderedItem.AddComponent<EquippedItem>();
                    equippedItemScript.item = equippedItem;
                    renderedId = equippedItemId;
                    renderedGuid = equippedItemScript?.item?.guid;
                }
            }
        }

        if (previousRenderedGuid != renderedGuid)
        {
            NotifyItemChanged();
        }

        previousRenderedId = renderedId;
        previousRenderedGuid = renderedGuid;
    }

    public void RegisterCallback(Action<GameObject> callback)
    {
        onItemChangeCallbacks.Add(callback);
    }

    private void NotifyItemChanged()
    {
        onItemChangeCallbacks.ForEach(callback => callback(renderedItem));
    }
}
