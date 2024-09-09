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

        Debug.Log(spawnDataRepo != null);
        spawnTimes = spawnDataRepo.animalSpawnDataList.Select(animalSpawnData => animalSpawnData.spawnTime).ToList();

        spawnLocations.Add(new Vector3(240, 0, 240));
        //spawnLocations.Add(new Vector3(30f, 0f, 70f));
        //spawnLocations.Add(new Vector3(70f, 0f, 30f));
        //spawnLocations.Add(new Vector3(70f, 0f, 70f));
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
                        behaviour.SetTarget(catalogEntry.plantObject.transform);
                    });

            Debug.Log(spawningReport);
        }

        /*
        if (currentTime >= rabbitSpawning.spawnTime && previousTime < rabbitSpawning.spawnTime)
        {
            int spawnLocationIndex = UnityEngine.Random.Range(0, spawnLocations.Count);
            Vector3 spawnLocation = spawnLocations[spawnLocationIndex];
            // Use the animal's prefab. For now, I'm going to use a primitive.
            GameObject animalObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            animalObject.transform.position = spawnLocation;

            Health health = animalObject.AddComponent<Health>();
            DropLoot dropLoot = animalObject.AddComponent<DropLoot>();
            AttackPlant behaviour = animalObject.AddComponent<AttackPlant>();

            health.health = 10;
            health.initialHealth = 10;
        }
        */

        previousTime = currentTime;
    }
}
