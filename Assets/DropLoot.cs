using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using ItemMetaData;

[Serializable]
public class ItemDrop
{
    public string itemName;
    public float dropRate;
}

[Serializable]
public class DropQuantityWeight
{
    public int quantity;
    public float weight;
}

public class DropLoot : MonoBehaviour
{
    public ItemDrop[] itemDrops;
    public DropQuantityWeight[] dropQuantityWeights;

    private Dictionary<string, float> dropRatesDictionary;
    private Dictionary<int, float> dropQuantityWeightsDictionary;

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

        // Normalize the weights in both dictionaries so the values in each add up to 1.
        float dropRateSum = itemDrops.Sum(itemDrop => itemDrop.dropRate);
        foreach (ItemDrop itemDrop in itemDrops)
        {
            itemDrop.dropRate = itemDrop.dropRate / dropRateSum;
        }

        dropQuantityWeightsDictionary = new Dictionary<int, float>();
        float dropQuantityWeightSum = dropQuantityWeights.Sum(weightKvp => weightKvp.weight);
        foreach (DropQuantityWeight kvp in dropQuantityWeights)
        {
            dropQuantityWeightsDictionary.Add(kvp.quantity, kvp.weight / dropQuantityWeightSum);
        }

        Debug.Log(dropQuantityWeightSum);
        dropQuantityWeightsDictionary.Values.ToList().ForEach(v => Debug.Log(v));
    }

    void OnDestroy()
    {
        float roll = UnityEngine.Random.Range(0.0f, 1.0f);
        Debug.Log(roll);
        foreach (float dropQuantityWeight in dropQuantityWeightsDictionary.Values)
        {
            DropItem();
            roll -= dropQuantityWeight;
            if (roll <= 0)
            {
                break;
            }
        }
    }

    void DropItem()
    {
        float roll = UnityEngine.Random.Range(0.0f, 1.0f);
        // Debug.Log($"Start roll: {roll}");
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
                // Debug.Log($"Item to drop: {itemToDrop}");
                ObjectSpawnController.GetController().MakeRequest(itemTypeToDrop.groundItemPrefab, spawnPosition, Quaternion.identity);
            }
            else
            {
                // Debug.Log($"Ground item prefab is null for: {itemToDrop}");
                // Debug.Log($"ItemType is null: {itemTypeToDrop == null}");
            }
        }
    }
}
