using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

public class CreatePlantPrefabs : MonoBehaviour
{
    // Variables
    const string plantName = "WeepingWillow";
    const string seedObjName = "WeepingWillowSeed";
    const string plantCategory = "_Trees";
    const float growthTimePerStage = 24f;

    const string harvestableItemId = "WeepingWillow";
    const float regrowTime = 24f;

    // Don't change
    const string plantsRoot = "Assets/Resources/Plants";
    const string plantFolderPath = plantsRoot + "/" + plantCategory + "/" + plantName + "/";
    const string seedObjPath = plantFolderPath + "/" + seedObjName + ".obj";

    [MenuItem("Tools/Plants/3S_Harv_Reg")]
    public static void ThreeStages_Harvestable_Regrows()
    {
        CreatePrefabs();
        AddAdultPrefabScripts();
        AddHarvestedPrefabScripts();
    }

    [MenuItem("Tools/Plants/3S_Harv")]
    public static void ThreeStages_Harvestable()
    {
        CreatePrefabs();
        AddAdultPrefabScripts();
    }


    [MenuItem("Tools/Plants/Create Plant Prefabs")]
    public static void CreatePrefabs()
    {
        // Container
        string containerPrefabName = $"{plantName}Container.prefab";
        GameObject containerObject = new GameObject(containerPrefabName);

        PlantStageGrowth growthScript = containerObject.AddComponent<PlantStageGrowth>();
        GameObject sproutPrefab = AssetDatabase.LoadAssetAtPath<GameObject>($"{plantFolderPath}/Sprout/Sprout.prefab");
        growthScript.initialPrefab = sproutPrefab;
        GameObject sapplingPrefab = AssetDatabase.LoadAssetAtPath<GameObject>($"{plantFolderPath}/Sappling/Sappling.prefab");
        growthScript.growthStages = new List<GrowthStage>();
        GrowthStage sapplingStage = new GrowthStage { stagePrefab = sapplingPrefab, timeToGrow = growthTimePerStage };
        growthScript.growthStages.Add(sapplingStage);
        GameObject youngAdultPrefab = AssetDatabase.LoadAssetAtPath<GameObject>($"{plantFolderPath}/YoungAdult/YoungAdult.prefab");
        if (youngAdultPrefab != null)
        {
            GrowthStage youngAdultStage = new GrowthStage { stagePrefab = youngAdultPrefab, timeToGrow = growthTimePerStage };
            growthScript.growthStages.Add(youngAdultStage);
        }
        else
        {
            Debug.Log("No YA prefab exists, so it wasn't added to the container.");
        }
        GameObject adultPrefab = AssetDatabase.LoadAssetAtPath<GameObject>($"{plantFolderPath}/Adult/Adult.prefab");
        GrowthStage adultStage = new GrowthStage { stagePrefab = adultPrefab, timeToGrow = growthTimePerStage };
        growthScript.growthStages.Add(adultStage);
        growthScript.plantName = plantName;

        containerObject.AddComponent<Waterable>();

        ModularInteractText interactText = containerObject.AddComponent<ModularInteractText>();
        interactText.textPrefab =  AssetDatabase.LoadAssetAtPath<GameObject>("Assets/CustomUIElements/InteractText.prefab");

        CapsuleCollider collider = containerObject.AddComponent<CapsuleCollider>();
        collider.radius = 1;
        collider.height = 4;

        string containerPrefabPath = $"{plantFolderPath}/{containerPrefabName}";

        PrefabUtility.SaveAsPrefabAsset(containerObject, containerPrefabPath);
    
        // GroundItem
        GameObject seedObject = AssetDatabase.LoadAssetAtPath<GameObject>(seedObjPath);
        GameObject groundItemObject = GameObject.Instantiate(seedObject);
        GroundItem groundItemScript = groundItemObject.AddComponent<GroundItem>();

        groundItemScript.itemId = seedObjName;
        groundItemScript.setTagDelay = 0.5f;
        string seedGroundItemPrefabPath = $"{plantFolderPath}SeedGroundItem.prefab";
        Debug.Log(seedGroundItemPrefabPath);
        PrefabUtility.SaveAsPrefabAsset(groundItemObject, seedGroundItemPrefabPath);

        // EquippedItem
        /* --- No need to reinstantiate the seedObject --- */
        GameObject equippedItemObject = GameObject.Instantiate(seedObject);
        UseSeed useSeedScript = equippedItemObject.AddComponent<UseSeed>();

        useSeedScript.plantPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(containerPrefabPath);
        equippedItemObject.transform.position = new Vector3(0.497999996f,-0.569000006f,1.051f);

        string seedEquippiedPrefabPath = $"{plantFolderPath}SeedEquipped.prefab";
        PrefabUtility.SaveAsPrefabAsset(equippedItemObject, seedEquippiedPrefabPath);
    }

    [MenuItem("Tools/Plants/Create Adult Prefab Script")]
    public static void AddAdultPrefabScripts()
    {
        string adultPrefabPath = $"{plantFolderPath}/Adult/Adult.prefab";
        GameObject adultPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(adultPrefabPath);

        Harvestable harvestableScript = adultPrefab.AddComponent<Harvestable>();
        harvestableScript.itemId = harvestableItemId;
        string harvestedPrefabPath = $"{plantFolderPath}/Harvested/Harvested.prefab";
        GameObject harvestedPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(harvestedPrefabPath);
        if (harvestedPrefab != null)
        {
            harvestableScript.regrows = true;
            harvestableScript.harvestedPrefab = harvestedPrefab;
        }
        else
        {
            Debug.Log("No harvested prefab exists.");
        }

        ModularInteractText interactText = adultPrefab.AddComponent<ModularInteractText>();
        interactText.textPrefab =  AssetDatabase.LoadAssetAtPath<GameObject>("Assets/CustomUIElements/InteractText.prefab");

        CapsuleCollider collider = adultPrefab.AddComponent<CapsuleCollider>();
        collider.radius = 1;
        collider.height = 4;
    }

    [MenuItem("Tools/Plants/Create Harvested Prefab Script")]
    public static void AddHarvestedPrefabScripts()
    {
        string harvestedPrefabPath = $"{plantFolderPath}/Harvested/Harvested.prefab";
        GameObject harvestedPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(harvestedPrefabPath);

        string adultPrefabPath = $"{plantFolderPath}/Adult/Adult.prefab";
        GameObject adultPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(adultPrefabPath);

        Regrow regrowScript = harvestedPrefab.AddComponent<Regrow>();
        regrowScript.harvestablePrefab = adultPrefab;
        regrowScript.regrowTime = regrowTime;

        CapsuleCollider collider = harvestedPrefab.AddComponent<CapsuleCollider>();
        collider.radius = 1;
        collider.height = 4;
    }
}
