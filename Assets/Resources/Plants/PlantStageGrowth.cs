using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GrowthStage
{
    public GameObject stagePrefab;
    public float timeToGrow;
    public float growthAnimationTime = 52f / 24f;
}

public class PlantStageGrowth : MonoBehaviour, IWaterable
{
    public GameObject initialPrefab;
    public List<GrowthStage> growthStages;
    public string plantName;

    private GameObject plantObject;
    private float growthTimer;
    private int stageIndex;
    private float lastClockTime;
    private string plantId;
    private bool isWatered;
    private bool growing;

    void Start()
    {
        plantObject = Instantiate(initialPrefab, transform.position, initialPrefab.transform.rotation);
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
        if (nextStage == null)
        {
            Waterable waterable = GetComponent<Waterable>();
            Destroy(waterable);
            ModularInteractText interactText = GetComponent<ModularInteractText>();
            Destroy(interactText);
            BoxCollider collider = GetComponent<BoxCollider>();
            Destroy(collider);

            lastClockTime = Clock.globalClock.time;
            return;
        }

        float elapsedTime = Clock.globalClock.time - lastClockTime;

        growthTimer += isWatered
            ? elapsedTime
            : 0;

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
        if (!growing)
        {
            growing = true;
            StartCoroutine(Grow());
        }
    }

    private IEnumerator Grow()
    {
        Animator controller = plantObject.GetComponent<Animator>();
        controller.SetTrigger("Grow");

        // You should probably dynamically use the animation length...
        GrowthStage nextStage = GetNextStage();
        yield return new WaitForSeconds(nextStage.growthAnimationTime);

        Destroy(plantObject);

        GameObject nextPlantPrefab = nextStage.stagePrefab;
        plantObject = Instantiate(nextPlantPrefab, transform.position, nextPlantPrefab.transform.rotation);
        plantObject.transform.parent = transform;

        stageIndex++;

        if (GetNextStage() == null)
        {
            Debug.Log("Destroying Waterable Component");
            Destroy(GetComponent<Waterable>());
        }

        isWatered = false;
        growing = false;
    }

    private void OnDestroy()
    {
        PlantCatalog.globalPlantCatalog.RemovePlant(plantId);
    }

    public void AddWater()
    {
        isWatered = true;
    }

    public bool IsWatered()
    {
        return isWatered;
    }

    public bool IsGrowing()
    {
        return growing;
    }
}
