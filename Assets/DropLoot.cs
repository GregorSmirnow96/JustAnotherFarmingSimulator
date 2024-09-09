using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ItemMetaData;

[System.Serializable]
public class ItemDrop
{
    public string itemName;
    public float dropRate;
}

public class DropLoot : MonoBehaviour
{
    public ItemDrop[] itemDrops;
    public bool multipleItemsCanDrop;

    private Dictionary<string, float> dropRatesDictionary;

    void Awake()
    {
        dropRatesDictionary = new Dictionary<string, float>();
        foreach (var itemDrop in itemDrops)
        {
            if (!dropRatesDictionary.ContainsKey(itemDrop.itemName))
            {
                dropRatesDictionary.Add(itemDrop.itemName, itemDrop.dropRate);
            }
            else
            {
                Debug.Log($"The drop table already contains the item: {itemDrop.itemName}");
            }
        }
    }

    void OnDestroy()
    {
        if (multipleItemsCanDrop)
        {
            HandleMultiDrop();
        }
        else
        {
            HandleSingleDrop();
        }
    }

    void HandleSingleDrop()
    {
        float roll = Random.Range(0.0f, 1.0f);
        Debug.Log($"Start roll: {roll}");
        string itemToDrop = null;
        foreach (ItemDrop itemDrop in itemDrops)
        {
            roll -= itemDrop.dropRate;
            if (roll <= 0)
            {
                itemToDrop = itemDrop.itemName;
                break;
            }
        }

        if (itemToDrop != null)
        {
            float terrainHeight = SceneProperties.TerrainHeightAtPosition(transform.position);
            Vector3 spawnPosition = transform.position;
            spawnPosition.y = terrainHeight;
            ItemType itemTypeToDrop = ItemTypeRepo.GetInstance().TryFindItemType(itemToDrop);
            if (itemTypeToDrop?.groundItemPrefab != null)
            {
                Debug.Log($"Item to drop: {itemToDrop}");
                ObjectSpawnController.GetController().MakeRequest(itemTypeToDrop.groundItemPrefab, spawnPosition, Quaternion.identity);
            }
            else
            {
                Debug.Log($"Ground item prefab is null for: {itemToDrop}");
                Debug.Log($"ItemType is null: {itemTypeToDrop == null}");
            }
        }
    }

    void HandleMultiDrop()
    {
    }
}
