using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AnimalSpawner : MonoBehaviour
{
    public GameObject forestSpiritPrefab;

    private float previousTime;
    private List<Vector3> spawnLocations = new List<Vector3>();
    private List<float> spawnTimes;
    private AnimalSpawnDataRepo spawnDataRepo;

    void Start()
    {
        spawnDataRepo = AnimalSpawnDataRepo.GetInstance();

        spawnTimes = spawnDataRepo.animalSpawnDataList.Select(animalSpawnData => animalSpawnData.spawnTime).ToList();

        foreach (Transform child in transform)
        {
            spawnLocations.Add(child.position);
        }
    }

    void Update()
    {
        float currentTime = Clock.globalClock.time % Clock.globalClock.dayDuration;

        bool isSpawnTime = spawnTimes.Any(spawnTime => currentTime >= spawnTime && previousTime < spawnTime);
        if (isSpawnTime)
        {
            SpawnAnimals();
        }

        previousTime = currentTime;
    }

    void SpawnAnimals()
    {
        /*
            Spawning Algorithm:

                // Animals
                - First determine how many animals to spawn
                    - Spawn 1 animal per plant, up to 3
                    - For every plant beyond 3, have a 50% chance to spawn another animals
                    - Add 1 animal for every 2 (1?) days played
                - Then choose which animals to spawn
                    - Animals will have a difficulty tier (1 is weakest)
                    - Define a map of weights for each difficulty tier. Something like {1: .3, 2: .2, 3: .18, 4: .15, 5: .1, 6: .7}
                    - For each plant, roll for difficulty with advantage per 2 (3+?) days played
                        - A roll of .73 would result in T3 because
                            - T3 weight = T1 + T2 + T3 = .68
                            - T4 weight = T1 + T2 + T3 + T4 = .83
                            - The roll of .73 is more than T3, but less than T4
                    - Spawn the hardest animal that is interested in the plant, excluding animals that are easier than the rolled tier
                
                // Cryptids
                - After all animals are spawned, spawn a random cryptid
                - Spawn 2 after 6 (7+?) days
        */

        // Calculate # of animals
        int daysPassed = (int) (Clock.globalClock.time / Clock.globalClock.dayDuration);
        Dictionary<string, CatalogEntry> catalog = PlantCatalog.globalPlantCatalog.catalog;
        int numberOfPlants = catalog.Keys.Count();

        int animalCount;
        if (numberOfPlants <= 3)
        {
            animalCount = numberOfPlants;
        }
        else
        {
            animalCount = 3;
            for (int i = 0; i < numberOfPlants - 3; i++)
            {
                if (Random.Range(0f, 1f) >= 0.5)
                {
                    animalCount++;
                }
            }
        }

        animalCount += (int) (daysPassed / 2);

        Debug.Log(numberOfPlants);
        Debug.Log(daysPassed);
        Debug.Log($"Animals to Spawn: {animalCount}");

        // Choose which animals to spawn
        int difficultyRollAdvantage = (int) (daysPassed / 2);

        // 1. Create a list of difficulty rolls
        List<int> animalDifficultyTiers = new List<int>();
        for (int i = 0; i < animalCount; i++)
        {
            float difficultyRoll = Random.Range(0f, 1f);
            for (int j = 0; j < difficultyRollAdvantage; j++)
            {
                float advantageRoll = Random.Range(0f, 1f);
                difficultyRoll = Mathf.Max(difficultyRoll, advantageRoll);
            }
            int nextAnimalTier = GetAnimalTierFromRoll(difficultyRoll);
            animalDifficultyTiers.Add(nextAnimalTier);
        }

        // 2. Iterate over them, selecting a random plant in the catalog
        animalDifficultyTiers.ForEach(requestedTier =>
        {
            int tier = Mathf.Min(requestedTier, daysPassed + 2); // Cap the tier so a random roll doesn't destroy the player on day 1.
            Debug.Log($"Tier = {tier}      (Requested Tier = {requestedTier})");

            List<CatalogEntry> catalogEntries = catalog.Values.ToList();
            // Filter down the catalogEntries to exclude plants that have no animals interested in them. Use the AnimalSpawnDataRepo to do this. <----- TODO
            int numberOfPlants = catalogEntries.Count();
            int selectedPlantIndex = Random.Range(0, numberOfPlants);

            CatalogEntry selectedPlant = catalogEntries.ElementAt(selectedPlantIndex);
            string plantName = selectedPlant.name;
            GameObject plantObject = selectedPlant.plantObject;

            List<AnimalSpawnData> spawnDataForPlant = spawnDataRepo.GetAnimalsInterestedInPlant(plantName);

            // 3. For each plant, use the roll to pick an animal
            List<AnimalSpawnData> filteredSpawnData = spawnDataForPlant.Where(spawnData => spawnData.difficulty <= tier).ToList();
            AnimalSpawnData selectedSpawnData = null;
            if (filteredSpawnData.Count() == 0 && spawnDataForPlant.Count() > 0)
            {
                selectedSpawnData = spawnDataForPlant.First();
            }
            else if (filteredSpawnData.Count() > 0)
            {
                List<AnimalSpawnData> sortedSpawnData = filteredSpawnData.OrderBy(data => data.difficulty).ToList();
                selectedSpawnData = sortedSpawnData.Where(spawnData => spawnData.difficulty <= tier).Last();
            }

            // 4. Spawn the animal, and set their target to the iterated plant
            if (selectedSpawnData != null)
            {
                Debug.Log($"Spawned a '{selectedSpawnData.name}' to target plant '{plantName}'");
                
                int spawnLocationIndex = UnityEngine.Random.Range(0, spawnLocations.Count);
                Vector3 spawnLocation = spawnLocations[spawnLocationIndex];
                spawnLocation.y = SceneProperties.TerrainHeightAtPosition(spawnLocation);
                GameObject animalObject = Instantiate(selectedSpawnData.animalPrefab, spawnLocation, Quaternion.identity);

                IAnimalBehaviour behaviour = animalObject.GetComponent<IAnimalBehaviour>();
                behaviour.SetTarget(plantObject.transform);
            }
            else
            {
                Debug.Log($"No animal was interested in '{plantName}' for the difficulty tier: {tier}");
            }
        });

        // Choose which (and how many?) cryptids to spawn
        int spawnLocationIndex = UnityEngine.Random.Range(0, spawnLocations.Count);
        Vector3 spawnLocation = spawnLocations[spawnLocationIndex];
        GameObject cryptidPrefabToSpawn = forestSpiritPrefab; // Choose either 1 random cryptid (not random??) or multiple if the player has survived long enough.
        Instantiate(forestSpiritPrefab, spawnLocation, Quaternion.identity);
    }

    int GetAnimalTierFromRoll(float difficultyRoll)
    {
        /*
            State above: - Define a map of weights for each difficulty tier. Something like {1: .3, 2: .2, 3: .18, 4: .15, 5: .1, 6: .7}
                - Trying these weightings instead since higher tier animals are too common to start:
                    .5  .75 .85 .91 .96

            I'm hard coding the roll ranges since this is NOT dynamic. The weights could change, but never at run time, so just update them here.
        */
        
        if (difficultyRoll <= 0.3f)
        {
            return 1;
        }
        else if (difficultyRoll <= 0.75f)
        {
            return 2;
        }
        else if (difficultyRoll <= 0.85f)
        {
            return 3;
        }
        else if (difficultyRoll <= 0.91f)
        {
            return 4;
        }
        else if (difficultyRoll <= 0.96f)
        {
            return 5;
        }
        else
        {
            return 6;
        }
    }

    void _SpawnAnimals()
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
        
        // Spawn animals
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

        // Spawn cryptid(s)
        int spawnLocationIndex = UnityEngine.Random.Range(0, spawnLocations.Count);
        Vector3 spawnLocation = spawnLocations[spawnLocationIndex];
        GameObject cryptidPrefabToSpawn = forestSpiritPrefab; // Choose either 1 random cryptid (not random??) or multiple if the player has survived long enough.
        Instantiate(forestSpiritPrefab, spawnLocation, Quaternion.identity);
    }
}
