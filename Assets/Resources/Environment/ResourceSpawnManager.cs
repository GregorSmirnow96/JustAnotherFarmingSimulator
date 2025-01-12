using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourceSpawnManager : MonoBehaviour
{
    public int maxFilledContainers;
    public float respawnInterval = 120f;

    private float respawnTimer;
    private List<ResourceContainer> resourceContainers;

    void Start()
    {
        resourceContainers = new List<ResourceContainer>();
        foreach (Transform child in transform)
        {
            ResourceContainer childContainer = child.GetComponent<ResourceContainer>();
            if (childContainer == null)
            {
                Debug.Log($"Child of {gameObject.name} ({child.gameObject.name}) has no ResourceContainer component.");
            }
            else
            {
                resourceContainers.Add(childContainer);
            }
        }

        FillContainers();
        respawnTimer = 0f;
    }

    void Update()
    {
        respawnTimer += Time.deltaTime;
        if (respawnTimer >= respawnInterval)
        {
            FillContainers();
            respawnTimer = 0f;
        }
    }

    private void FillContainers()
    {
        int filledContainerCount = resourceContainers.Count(container => container.resourceExists);
        int numberOfContainersToFill = System.Math.Max(maxFilledContainers - filledContainerCount, 0);

        for (int i = 0; i < numberOfContainersToFill; i++)
        {
            List<ResourceContainer> emptyContainers = GetEmptyContainers();
            int emptyContainerCount = emptyContainers.Count();
            if (emptyContainerCount > 0)
            {
                int emptyContainerIndex = Random.Range(0, emptyContainers.Count());
                ResourceContainer emptyContainer = emptyContainers.ElementAt(emptyContainerIndex);
                emptyContainer.FillContainer();
            }
        }
    }

    private List<ResourceContainer> GetEmptyContainers() => resourceContainers
        .Where(container => !container.resourceExists)
        .ToList();
}
