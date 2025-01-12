using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AnimalSpawner : MonoBehaviour
{
    private float previousTime;
    private List<Vector3> spawnLocations = new List<Vector3>();
    private List<float> spawnTimes;
    private AnimalSpawnDataRepo spawnDataRepo;

    void Start()
    {
        spawnDataRepo = AnimalSpawnDataRepo.GetInstance();

        spawnTimes = spawnDataRepo.animalSpawnDataList.Select(animalSpawnData => animalSpawnData.spawnTime).ToList();

        //spawnLocations.Add(new Vector3(146.42f, 0f, 330.82f));
        spawnLocations.Add(new Vector3(102f, 0f, 128.24f));
    }

    void Update()
    {
        float currentTime = Clock.globalClock.time % Clock.globalClock.dayDuration;

        bool isSpawnTime = spawnTimes.Any(spawnTime => currentTime >= spawnTime && previousTime < spawnTime);
        if (isSpawnTime)
        {
            string spawningReport = "SPAWNING REPORT\n";
            var catalog = PlantCatalog.globalPlantCatalog.catalog;
            var catalogKeys = catalog.Keys;
            spawningReport +=
                catalogKeys.Count == 0
                    ? "NO KEYS"
                    : catalogKeys.Count == 1
                        ? catalogKeys.ToList()[0]
                        : catalogKeys.Select(k => catalog[k].name).Aggregate((s1, s2) => $"{s1},{s2}");
            
            catalogKeys.ToList().ForEach(k => Debug.Log($"{k} -> {catalog[k]}"));
            catalogKeys
                .Select(key => catalog[key])
                .ToList()
                .ForEach(catalogEntry =>
                    {
                        List<AnimalSpawnData> interestedAnimals = spawnDataRepo.animalSpawnDataList
                            .Where(animalSpawnData => animalSpawnData.targetPlants.Contains(catalogEntry.name))
                            .ToList();
                        
                        if (interestedAnimals.Count == 0)
                        {
                            Debug.Log($"No animals are interested in {catalogEntry.name}");
                            return;
                        }

                        int selectedAnimalIndex = UnityEngine.Random.Range(0, interestedAnimals.Count);
                        AnimalSpawnData selectedAnimalSpawnData = interestedAnimals[selectedAnimalIndex];

                        int spawnLocationIndex = UnityEngine.Random.Range(0, spawnLocations.Count);
                        Vector3 spawnLocation = spawnLocations[spawnLocationIndex];
                        spawnLocation.y = SceneProperties.TerrainHeightAtPosition(spawnLocation);
                        GameObject animalObject = Instantiate(selectedAnimalSpawnData.animalPrefab, spawnLocation, Quaternion.identity);

                        Debug.Log("Getting behaviour script...");
                        IAnimalBehaviour behaviour = animalObject.GetComponent<IAnimalBehaviour>();
                        Debug.Log($"Setting the target: {catalogEntry.plantObject.transform.position}");
                        Debug.Log(catalogEntry != null);
                        Debug.Log(catalogEntry?.plantObject != null);
                        Debug.Log(catalogEntry?.plantObject?.transform != null);
                        behaviour.SetTarget(catalogEntry.plantObject.transform);
                    });

            Debug.Log(spawningReport);
        }

        previousTime = currentTime;
    }
}
