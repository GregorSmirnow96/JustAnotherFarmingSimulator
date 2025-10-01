using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class AnimalSpawnData
{
    public string name;
    public float spawnTime; // This should be a range eventually.
    public float retreatTime; // ^ same
    public GameObject animalPrefab;
    public List<string> targetPlants;
    public int difficulty;

    public AnimalSpawnData(
        string name,
        float spawnTime,
        float retreatTime,
        string animalPrefabPath,
        List<string> targetPlants,
        int difficulty)
    {
        this.name = name;
        this.spawnTime = spawnTime;
        this.retreatTime = retreatTime;
        this.animalPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(animalPrefabPath);
        this.targetPlants = targetPlants;
        this.difficulty = difficulty;
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
        AddSpawnData(new RaccoonSpawnData());
        AddSpawnData(new BoarSpawnData());
        AddSpawnData(new FoxSpawnData());
        AddSpawnData(new DeerSpawnData());
        AddSpawnData(new CougarSpawnData());
        AddSpawnData(new WolfSpawnData());
        AddSpawnData(new BearSpawnData());
        AddSpawnData(new MooseSpawnData());
        AddSpawnData(new TigerSpawnData());
    }

    public AnimalSpawnData TryGetSpawnData(string animalName) => animalSpawnDataMap[animalName];
    
    private void AddSpawnData(AnimalSpawnData spawnData)
    {
        animalSpawnDataMap.Add(spawnData.name, spawnData);
    }

    public List<AnimalSpawnData> GetAnimalsInterestedInPlant(string plantName)
    {
        return animalSpawnDataList
            .Where(spawnData => spawnData.targetPlants.Contains(plantName))
            .ToList();
    }
}
