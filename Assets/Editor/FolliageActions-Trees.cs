using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

public class RemoveFolliageIndices : MonoBehaviour
{
    private static string treeAssetsRoot = "Environment/ProxyGames/Trees";

    [MenuItem("Tools/Folliage/Trees/Remove Folliage Indices")]
    public static void RemoveIndicesRoot()
    {
        List<string> assetNames = new List<string>()
        {
            "Tree 1",
            "Tree 2",
            "Tree 3",
            "Tree 4",
            "Tree 5"
        };
        foreach (string assetName in assetNames)
        {
            Transform[] children = GameObject.Find("Trees").GetComponentsInChildren<Transform>();
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

    [MenuItem("Tools/Folliage/Trees/Rotate Randomly")]
    public static void SetRandomRotations()
    {
        List<string> assetNames = new List<string>()
        {
            "Tree 1",
            "Tree 2",
            "Tree 3",
            "Tree 4",
            "Tree 5"
        };
        Transform[] children = GameObject.Find("Trees").GetComponentsInChildren<Transform>();
        List<GameObject> childObjects = children.Select(c => c.gameObject).Where(c => assetNames.Contains(c.name)).ToList();
        childObjects.ForEach(c =>
            {
                float randomRotation = Random.Range(0, 360);
                Vector3 eulerAngles = new Vector3(0f, randomRotation, 0f);

                c.transform.eulerAngles = eulerAngles;
            });
    }

    [MenuItem("Tools/Folliage/Trees/Scale Verticle Randomly")]
    public static void SetRandomVerticalScales()
    {
        List<string> assetNames = new List<string>()
        {
            "Tree 1",
            "Tree 2",
            "Tree 3",
            "Tree 4",
            "Tree 5"
        };
        Transform[] children = GameObject.Find("Trees").GetComponentsInChildren<Transform>();
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

    [MenuItem("Tools/Folliage/GenerateTrees")]
    public static void GenerateTrees()
    {
        const int numberOfTrees = 800;
        Vector3 center = new Vector3(200f, 0f, 200f);

        List<GameObject> treefabs = new List<GameObject>()
        {
            Resources.Load<GameObject>($"{treeAssetsRoot}/Tree 1"),
            Resources.Load<GameObject>($"{treeAssetsRoot}/Tree 2"),
            Resources.Load<GameObject>($"{treeAssetsRoot}/Tree 3"),
            Resources.Load<GameObject>($"{treeAssetsRoot}/Tree 4"),
            Resources.Load<GameObject>($"{treeAssetsRoot}/Tree 5")
        };
        bool resourcesExist = treefabs.Any(prefab => prefab != null);
        Debug.Log($"Tree prefabs exist: {resourcesExist}");

        float minDegrees = 0f;
        float maxDegrees = 359f;
        float minDistance = 55f;
        float maxDistance = 150f;
        GameObject folliageContainer = GameObject.Find("Trees");
        Terrain terrain = GameObject.Find("Terrain").GetComponent<Terrain>();
        for (int i = 0; i < numberOfTrees; i++)
        {
            float randomDegrees = Random.Range(minDegrees, maxDegrees);
            float randomDistance = Random.Range(minDistance, maxDistance);
            Vector3 direction = GetDirectionVector(randomDegrees);
            Vector3 position = center + direction * randomDistance;
            float terrainHeight = terrain.SampleHeight(position);
            position.y = terrainHeight;
            int treefabIndex = Random.Range(0, treefabs.Count);
            GameObject prefab = treefabs[treefabIndex];
            GameObject tree = Instantiate(prefab, position, Quaternion.identity);
            tree.transform.parent = folliageContainer.transform;
        }
    }

    [MenuItem("Tools/Folliage/GenerateGrass")]
    public static void GenerateGrass()
    {
        GameObject grassPrefab = Resources.Load<GameObject>("PT_Grass_02_Stretched");

        float minCoordValue = 55f;
        float maxCoordValue = 345f;
        int rows = 5;
        int columns = rows;

        float delta = maxCoordValue - minCoordValue;
        GameObject grassContainer = GameObject.Find("Grass");
        Terrain terrain = GameObject.Find("Terrain").GetComponent<Terrain>();
        for (int z = 0; z < rows; z++)
        {
            for (int x = 0; x < columns; x++)
            {
                float xPosition = minCoordValue + delta * (((float)x) / ((float)columns));
                float zPosition = minCoordValue + delta * (((float)x) / ((float)rows));
                Vector3 position = new Vector3(xPosition, 0f, zPosition);
                float terrainHeight = terrain.SampleHeight(position);
                position.y = terrainHeight;
                GameObject grass = Instantiate(grassPrefab, position, Quaternion.identity);
                grass.transform.parent = grassContainer.transform;
            }
        }
    }

    
    [MenuItem("Tools/Folliage/DeleteGrass")]
    public static void DeleteGrass()
    {
        var objects = GameObject.FindObjectsOfType<GameObject>();
        Debug.Log(objects.Count());

        int deleteCount = 0;
        foreach (GameObject obj in objects)
        {
            try
            {
                if (deleteCount > 10000)
                {
                    return;
                }
                if (obj.name == "PT_Grass_02_Stretched(Clone)")
                {
                    DestroyImmediate(obj);
                    deleteCount++;
                }
            }
            catch
            {
            }
        }
    }
}
