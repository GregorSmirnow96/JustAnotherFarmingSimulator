using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

public class PlantFolliageActions : MonoBehaviour
{
    private static int numberOfPlants = 1200;

    private static string plantAssetsRoot = "Environment/ProxyGames/Plants";

    private static List<string> assetNames = new List<string>()
        {
            "Plant 1",
            "Plant 2",
            "Plant 3"
        };

    [MenuItem("Tools/Folliage/Plants/Remove Folliage Indices")]
    public static void RemoveIndicesRoot()
    {
        foreach (string assetName in assetNames)
        {
            Transform[] children = GameObject.Find("Plants").GetComponentsInChildren<Transform>();
            List<GameObject> childObjects = children.Select(c => c.gameObject).ToList();
            childObjects.ForEach(c =>
                {
                    if (c.name.StartsWith(assetName))
                    {
                        c.name = assetName;
                    }
                });
        }
    }

    [MenuItem("Tools/Folliage/Plants/Rotate Randomly")]
    public static void SetRandomRotations()
    {
        Transform[] children = GameObject.Find("Plants").GetComponentsInChildren<Transform>();
        List<GameObject> childObjects = children.Select(c => c.gameObject).Where(c => assetNames.Contains(c.name)).ToList();
        childObjects.ForEach(c =>
            {
                float randomRotation = Random.Range(0, 360);
                Vector3 eulerAngles = new Vector3(0f, randomRotation, 0f);

                c.transform.eulerAngles = eulerAngles;
            });
    }

    [MenuItem("Tools/Folliage/Plants/Scale Verticle Randomly")]
    public static void SetRandomVerticalScales()
    {
        Transform[] children = GameObject.Find("Plants").GetComponentsInChildren<Transform>();
        List<GameObject> childObjects = children.Select(c => c.gameObject).Where(c => assetNames.Contains(c.name)).ToList();
        childObjects.ForEach(c =>
            {
                float randomVerticalScale = ((float)Random.Range(60, 160)) / 100f;
                Vector3 scale = c.transform.localScale;
                scale.y = randomVerticalScale;

                c.transform.localScale = scale;
            });
    }

    public static Vector3 GetDirectionVector(float angleInDegrees)
    {
        // Convert the angle to radians
        float angleInRadians = angleInDegrees * Mathf.Deg2Rad;

        // Calculate the x and y components
        float x = Mathf.Cos(angleInRadians);
        float z = Mathf.Sin(angleInRadians);

        // Return the vector
        Vector3 direction = new Vector3(x, 0f, z);
        return direction.normalized;
    }

    private static bool IsInCircle(Vector3 position, Vector3 center, float radius)
    {
        float distanceSquared = (position.x - center.x) * (position.x - center.x) + (position.z - center.z) * (position.z - center.z);

        return distanceSquared <= radius * radius;
    }

    [MenuItem("Tools/Folliage/Plants/GeneratePlants")]
    public static void GeneratePlants()
    {
        Vector3 center = new Vector3(200f, 0f, 200f);

        List<GameObject> plantPrefabs = assetNames.Select(asset =>
        {
            Debug.Log($"{plantAssetsRoot}/{asset}");
            return Resources.Load<GameObject>($"{plantAssetsRoot}/{asset}");
        }).ToList();
        bool resourcesExist = plantPrefabs.Any(prefab => prefab != null);
        Debug.Log($"Plant prefabs exist: {resourcesExist}");

        float minDegrees = 0f;
        float maxDegrees = 359f;
        float minDistance = 55f;
        float maxDistance = 150f;
        GameObject folliageContainer = GameObject.Find("Plants");
        Terrain terrain = GameObject.Find("Terrain").GetComponent<Terrain>();
        for (int i = 0; i < numberOfPlants; i++)
        {
            float randomDegrees = Random.Range(minDegrees, maxDegrees);
            float randomDistance = Random.Range(minDistance, maxDistance);
            Vector3 direction = GetDirectionVector(randomDegrees);
            Vector3 position = center + direction * randomDistance;
            float terrainHeight = terrain.SampleHeight(position);
            position.y = terrainHeight;
            int prefabIndex = Random.Range(0, plantPrefabs.Count);
            GameObject prefab = plantPrefabs[prefabIndex];
            GameObject plant = Instantiate(prefab, position, Quaternion.identity);
            plant.transform.parent = folliageContainer.transform;
        }
    }
}
