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
        resourceContainers ??= new List<ResourceContainer>();

        List<ResourceContainer> destroyedContainers = new List<ResourceContainer>();
        for (int i = 0; i < resourceContainers.Count(); i++)
        {
            ResourceContainer container = resourceContainers.ElementAt(i);
            if (container == null)
            {
                destroyedContainers.Add(container);
            }
        }

        foreach (ResourceContainer destroyedContainer in destroyedContainers)
        {
            resourceContainers.Remove(destroyedContainer);
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

    public void AddContainer(ResourceContainer newContainer)
    {
        resourceContainers ??= new List<ResourceContainer>();
        resourceContainers.Add(newContainer);
    }

    public void RemoveContainer(ResourceContainer newContainer)
    {
        resourceContainers ??= new List<ResourceContainer>();
        resourceContainers.Remove(newContainer);
    }

    private void FillContainers()
    {
        Debug.Log(gameObject.name);

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
