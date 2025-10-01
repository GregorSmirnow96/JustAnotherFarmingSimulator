using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourceContainer : MonoBehaviour
{
    public List<GameObject> resourcePrefabs;
    public bool randomizeRotation = true;
    public string spawnManagerName;

    public bool resourceExists => transform.childCount > 0;

    private ResourceSpawnManager spawnManager;

    void Awake()
    {
        spawnManager = GameObject.Find(spawnManagerName).GetComponent<ResourceSpawnManager>();
        spawnManager.AddContainer(this);
    }

    public void FillContainer()
    {
        if (!resourceExists)
        {
            int resourceIndex = Random.Range(0, resourcePrefabs.Count);
            GameObject prefab = resourcePrefabs.ElementAt(resourceIndex);

            GameObject resourceObject = Instantiate(prefab, transform);

            if (randomizeRotation)
            {
                resourceObject.transform.eulerAngles = new Vector3(0f, Random.Range(0f, 360f), 0f);
            }
        }
    }

    public void OnDestroy()
    {
        spawnManager.RemoveContainer(GetComponent<ResourceContainer>());
    }
}
