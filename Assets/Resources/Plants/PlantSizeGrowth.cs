using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlantSizeGrowth : MonoBehaviour
{
    // This needs to be used. It doesn't work with the template assets, though. For now, just scale up the meshes with growthStageScales.
    //public List<GameObject> growthStagePrefabs;
    public List<float> growthStageScales;
    public List<float> growthTimes;
    public string plantName;

    public float growthTimer = 0;
    public float growthScaler = 1f;
    public int nextStageIndex = 0;

    private bool addedToCatalog = false;
    public string plantId;
    public bool fullyGrown => nextStageIndex == growthStageScales.Count;

    void Start()
    {
        if (growthTimes.Count != growthStageScales.Count)
        {
            Debug.Log("Growth stage count must equal growth timer count.");
        }

        plantId = Guid.NewGuid().ToString();
        Debug.Log(plantId);
    }

    void Update()
    {
        if (!addedToCatalog && plantName != null)
        {
            PlantCatalog.globalPlantCatalog.AddPlant(plantId, plantName, gameObject);
            addedToCatalog = true;
        }

        growthTimer += Time.deltaTime * growthScaler;
        if (nextStageIndex >= growthTimes.Count)
        {
            return;
        }

        float nextStageTime = growthTimes[nextStageIndex];
        if (growthTimer >= growthTimes[nextStageIndex])
        {
            float nextStageScale = growthStageScales[nextStageIndex];
            gameObject.transform.localScale = gameObject.transform.localScale * (1 + nextStageScale);

            nextStageIndex++;
        }
    }

    private void OnDestroy()
    {
        PlantCatalog.globalPlantCatalog.RemovePlant(plantId);
    }
}
