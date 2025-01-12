using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

public class RockFolliageActions : MonoBehaviour
{
    private static int numberOfRocks = 1200;

    private static string rockAssetsRoot = "Environment/ProxyGames/Rocks";

    private static List<string> assetNames = new List<string>()
        {
            "Small Rock 1",
            "Small Rock 2",
            "Small Rock 3",
            "Small Rock 4",
            "Small Rock 5"
        };

    [MenuItem("Tools/Folliage/Rocks/Remove Folliage Indices")]
    public static void RemoveIndicesRoot()
    {
        foreach (string assetName in assetNames)
        {
            Transform[] children = GameObject.Find("Rocks").GetComponentsInChildren<Transform>();
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

    [MenuItem("Tools/Folliage/Rocks/Rotate Randomly")]
    public static void SetRandomRotations()
    {
        Transform[] children = GameObject.Find("Rocks").GetComponentsInChildren<Transform>();
        List<GameObject> childObjects = children.Select(c => c.gameObject).Where(c => assetNames.Contains(c.name)).ToList();
        childObjects.ForEach(c =>
            {
                float randomRotation = Random.Range(0, 360);
                Vector3 eulerAngles = new Vector3(0f, randomRotation, 0f);

                c.transform.eulerAngles = eulerAngles;
            });
    }

    [MenuItem("Tools/Folliage/Rocks/Scale Verticle Randomly")]
    public static void SetRandomVerticalScales()
    {
        Transform[] children = GameObject.Find("Rocks").GetComponentsInChildren<Transform>();
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

    [MenuItem("Tools/Folliage/Rocks/GenerateRocks")]
    public static void GenerateRocks()
    {
        const int maxAttempts = 2000;
        const int numberOfRocks = 750;
        const float left = 41.62077f;
        const float right = 245.2508f;
        const float front = 134.6476f;
        const float back = 333.3976f;

        List<GameObject> prefabs = new List<GameObject>()
        {
            AssetDatabase.LoadAssetAtPath<GameObject>("Assets/_OldCliffs/Prefabs/Splinter1.prefab"),
            AssetDatabase.LoadAssetAtPath<GameObject>("Assets/_OldCliffs/Prefabs/Splinter2.prefab"),
            AssetDatabase.LoadAssetAtPath<GameObject>("Assets/_OldCliffs/Prefabs/SplinterS1.prefab"),
            AssetDatabase.LoadAssetAtPath<GameObject>("Assets/_OldCliffs/Prefabs/SplinterS2.prefab"),
            AssetDatabase.LoadAssetAtPath<GameObject>("Assets/_OldCliffs/Prefabs/SplinterS3.prefab"),
            AssetDatabase.LoadAssetAtPath<GameObject>("Assets/_OldCliffs/Prefabs/SplinterS4.prefab")
        };

        bool resourcesExist = prefabs.Any(prefab => prefab != null);
        Debug.Log($"Rock prefabs exist: {resourcesExist}");

        GameObject rockContainer = GameObject.Find("Rocks");
        Terrain terrain = GameObject.Find("Terrain").GetComponent<Terrain>();
        int rocksCreated = 0;
        for (int i = 0; i < maxAttempts; i++)
        {
            if (rocksCreated >= numberOfRocks)
            {
                continue;
            }

            float x = Random.Range(left, right);
            float z = Random.Range(front, back);
            Vector3 position = new Vector3(x, 0f, z);
            position = new Vector3(x, terrain.SampleHeight(position), z);

            int prefabIndex = Random.Range(0, prefabs.Count);
            GameObject prefab = prefabs[prefabIndex];
            Quaternion randomRotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
            GameObject rock = Instantiate(prefab, position, randomRotation);
            float randomVerticalScale = ((float)Random.Range(60, 160)) / 100f;
            Vector3 scale = rock.transform.localScale;
            scale.y = randomVerticalScale;
            rock.transform.localScale = scale;
            Collider rockCollider = rock.GetComponent<Collider>();
            if (rockCollider != null)
            {
                Collider[] overlappingColliders = Physics.OverlapBox(
                    rockCollider.bounds.center,
                    rockCollider.bounds.extents,
                    rock.transform.rotation);
                overlappingColliders = overlappingColliders.Where(c => c.gameObject.name != "Terrain").ToArray();
                
                if (overlappingColliders.Length > 1)
                {
                    Debug.Log("The rock overlapped with another object.");
                    foreach (Collider c in overlappingColliders)
                    {
                        Debug.Log(c.gameObject.name);
                    }
                    DestroyImmediate(rock);
                    continue;
                }
            }

            rocksCreated++;
            rock.transform.parent = rockContainer.transform;
        }
    }
}
