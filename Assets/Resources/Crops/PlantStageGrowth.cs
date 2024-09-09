using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GrowthStage
{
    public GameObject stagePrefab;
    public float timeToGrow;
    // Implement other growth parameters here (I.E.: water levels)
}

public class PlantStageGrowth : MonoBehaviour
{
    public GameObject initialPrefab;
    public List<GrowthStage> growthStages;
    public string plantName;

    private GameObject plantObject;
    private float growthTimer;
    private int stageIndex;
    private float lastClockTime;
    private string plantId;

    void Start()
    {
        plantObject = Instantiate(initialPrefab, transform.position, Quaternion.identity);
        plantObject.transform.parent = transform;
        growthTimer = 0;
        stageIndex = 0;
        lastClockTime = Clock.globalClock.time;
        plantId = Guid.NewGuid().ToString();

        PlantCatalog.globalPlantCatalog.AddPlant(plantId, plantName, gameObject);
    }

    void Update()
    {
        GrowthStage nextStage = GetNextStage();
        if (nextStage == null) return;

        growthTimer += Clock.globalClock.time - lastClockTime;

        if (growthTimer >= nextStage.timeToGrow)
        {
            growthTimer -= nextStage.timeToGrow;
            IncrementPhase();
        }

        lastClockTime = Clock.globalClock.time;
    }

    private GrowthStage GetNextStage() => stageIndex < growthStages.Count ? growthStages[stageIndex] : null;

    private void IncrementPhase()
    {
        Destroy(plantObject);

        GameObject nextPlantPrefab = GetNextStage().stagePrefab;
        plantObject = Instantiate(nextPlantPrefab, transform.position, Quaternion.identity);
        plantObject.transform.parent = transform;

        stageIndex++;
    }

    private void OnDestroy()
    {
        PlantCatalog.globalPlantCatalog.RemovePlant(plantId);
    }
}
