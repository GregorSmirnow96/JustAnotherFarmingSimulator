using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

public class CreatePlantAssetTemplates : MonoBehaviour
{
    private static Vector3 plantScale = new Vector3(0.075f, 0.25f, 0.075f);
    private static Color plantColor = new Color(80f/255f, 80f/255f, 200f/255f, 1f);
    private static string plantName = "Plant11";

    [MenuItem("Tools/Create Plant Asset Templates")]
    public static void CreateAssets()
    {
        CreateInitialPrefab();
        CreateStagePrefab("S1", 1);
        CreateStagePrefab("S2", 2);
        CreateStagePrefab("S3", 3);
        CreateStagePrefab("S4", 4);
        AddScriptToS1();
        CreateIndicatorPrefab();
        CreateSeedPrefab();

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("Assets created successfully.");
    }

    private static void CreateInitialPrefab()
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = new Vector3(0f, plantScale.y / 2, 0f);
        cube.transform.localScale = plantScale;

        Material newMaterial = new Material(Shader.Find("Standard"));
        newMaterial.color = plantColor;

        Renderer renderer = cube.GetComponent<Renderer>();
        renderer.material = newMaterial;

        string materialPath = $"Assets/Resources/Crops/{plantName}/PlantMaterial.mat";
        AssetDatabase.CreateAsset(newMaterial, materialPath);

        string prefabPath = $"Assets/Resources/Crops/{plantName}/Plant.prefab";
        PrefabUtility.SaveAsPrefabAsset(cube, prefabPath);

        DestroyImmediate(cube);
    }

    private static void CreateStagePrefab(string name, float childScale)
    {
        GameObject parentObject = new GameObject(name);

        GameObject childPrefab = Resources.Load<GameObject>($"Crops/{plantName}/Plant");
        GameObject childObject = Instantiate(childPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity);
        childObject.transform.position = new Vector3(0f, plantScale.y / 2, 0f) * childScale;
        childObject.transform.localScale = plantScale * childScale;
        childObject.transform.parent = parentObject.transform;

        Material plantMaterial = Resources.Load<Material>($"Crops/{plantName}/PlantMaterial");

        Renderer renderer = childObject.GetComponent<Renderer>();
        renderer.material = plantMaterial;

        string prefabPath = $"Assets/Resources/Crops/{plantName}/{name}.prefab";
        PrefabUtility.SaveAsPrefabAsset(parentObject, prefabPath);

        DestroyImmediate(parentObject);
    }

    private static void AddScriptToS1()
    {
        GameObject s1Prefab = Resources.Load<GameObject>($"Crops/{plantName}/S1");
        GameObject s1Instance = Instantiate(s1Prefab, new Vector3(0f, 0f, 0f), Quaternion.identity);

        PlantSizeGrowth growthScript = s1Instance.AddComponent<PlantSizeGrowth>();
        GameObject s2Prefab = Resources.Load<GameObject>($"Crops/{plantName}/S2");
        GameObject s3Prefab = Resources.Load<GameObject>($"Crops/{plantName}/S3");
        GameObject s4Prefab = Resources.Load<GameObject>($"Crops/{plantName}/S4");
        //growthScript.growthStagePrefabs = new List<GameObject>()
        //    {
        //        s2Prefab,
        //        s3Prefab,
        //        s4Prefab
        //    };
        growthScript.growthTimes = new List<float>()
            {
                3f,
                6f,
                9f
            };
        PrefabUtility.SaveAsPrefabAsset(s1Instance, $"Assets/Resources/Crops/{plantName}/S1.prefab");
        DestroyImmediate(s1Instance);
    }

    private static void CreateIndicatorPrefab()
    {
        GameObject parentObject = new GameObject("SeedIndicator");

        GameObject childObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        childObject.transform.position = new Vector3(0f, plantScale.y / 2, 0f);
        childObject.transform.localScale = plantScale;
        childObject.transform.parent = parentObject.transform;

        Material transparentMaterial = new Material(Shader.Find("Standard"));
        Color transparentColor = plantColor;
        plantColor.a = 114f/255f;
        transparentMaterial.color = plantColor;
        transparentMaterial.SetFloat("_Mode", 3);

        string materialPath = $"Assets/Resources/Crops/{plantName}/SeedIndicatorMaterial.mat";
        AssetDatabase.CreateAsset(transparentMaterial, materialPath);

        Renderer renderer = childObject.GetComponent<Renderer>();
        renderer.material = transparentMaterial;

        string prefabPath = $"Assets/Resources/Crops/{plantName}/SeedIndicator.prefab";
        PrefabUtility.SaveAsPrefabAsset(parentObject, prefabPath);

        DestroyImmediate(parentObject);
    }

    private static void CreateSeedPrefab()
    {
        GameObject parentObject = new GameObject("SeedIndicator");
        parentObject.tag = "GroundItem";
        GroundItem groundItemScript = parentObject.AddComponent<GroundItem>();
        groundItemScript.itemId = plantName;

        GameObject childObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        childObject.transform.position = new Vector3(0f, plantScale.y / 2, 0f);
        childObject.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        childObject.transform.parent = parentObject.transform;

        Material seedMaterial = new Material(Shader.Find("Standard"));
        seedMaterial.color = plantColor;
        seedMaterial.SetFloat("_Metallic", 1f);
        seedMaterial.SetFloat("_Glossiness", 1f);

        string materialPath = $"Assets/Resources/Crops/{plantName}/SeedMaterial.mat";
        AssetDatabase.CreateAsset(seedMaterial, materialPath);

        Renderer renderer = childObject.GetComponent<Renderer>();
        renderer.material = seedMaterial;

        string prefabPath = $"Assets/Resources/Crops/{plantName}/SeedGroundItem.prefab";
        PrefabUtility.SaveAsPrefabAsset(parentObject, prefabPath);

        DestroyImmediate(parentObject);
    }
}
