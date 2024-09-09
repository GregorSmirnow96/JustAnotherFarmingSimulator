using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AnimalSpawnData
{
    public string name;
    public float spawnTime; // This should be a range eventually.
    public float retreatTime; // ^ same
    public GameObject animalPrefab;
    public List<string> targetPlants;

    public AnimalSpawnData(
        string name,
        float spawnTime,
        float retreatTime,
        string animalPrefabPath,
        List<string> targetPlants)
    {
        this.name = name;
        this.spawnTime = spawnTime;
        this.retreatTime = retreatTime;
        this.animalPrefab = Resources.Load<GameObject>(animalPrefabPath);
        this.targetPlants = targetPlants;
    }
}

public class AnimalSpawnDataRepo
{
    private static AnimalSpawnDataRepo instance;

    public static AnimalSpawnDataRepo GetInstance() =>
        instance == null
            ? instance = new AnimalSpawnDataRepo()
            : instance;

    public Dictionary<string, AnimalSpawnData> animalSpawnDataMap;
    public List<AnimalSpawnData> animalSpawnDataList => animalSpawnDataMap.Values.ToList();

    private AnimalSpawnDataRepo()
    {
        animalSpawnDataMap = new Dictionary<string, AnimalSpawnData>();

        AddSpawnData(new RabbitSpawnData());
    }

    public AnimalSpawnData TryGetSpawnData(string animalName) => animalSpawnDataMap[animalName];
    
    private void AddSpawnData(AnimalSpawnData spawnData)
    {
        animalSpawnDataMap.Add(spawnData.name, spawnData);
    }
}
