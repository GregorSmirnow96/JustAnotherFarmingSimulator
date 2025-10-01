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
        List<Transform> children = GameObject.Find("Trees").GetComponentsInChildren<Transform>().ToList();
        children.ForEach(c =>
            {
                if (c.gameObject.name.Contains("(Clone)"))
                {
                    float randomRotation = Random.Range(0, 360);
                    Vector3 eulerAngles = new Vector3(0f, randomRotation, 0f);

                    c.transform.eulerAngles = eulerAngles;
                }
            });
    }

    [MenuItem("Tools/Folliage/Trees/Scale Verticle Randomly")]
    public static void SetRandomVerticalScales()
    {
        List<Transform> children = GameObject.Find("Trees").GetComponentsInChildren<Transform>().ToList();
        children.ForEach(c =>
            {
                if (c.gameObject.name.Contains("(Clone)"))
                {
                    float randomVerticalScale = ((float)Random.Range(60, 160)) / 100f;
                    Vector3 scale = c.transform.localScale;
                    scale.y = randomVerticalScale;

                    c.transform.localScale = scale;
                }
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
        const int maxAttempts = 2000;
        const int numberOfTrees = 750;
        const float left = 0f;
        const float right = 400f;
        const float front = 0f;
        const float back = 400f;

        List<GameObject> treefabs = new List<GameObject>()
        {
            AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/Trees (Low Poly)/Prefab/Pine and fir trees/PineTree01color1.prefab"),
            AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/Trees (Low Poly)/Prefab/Pine and fir trees/PineTree01color1.prefab"),
            AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/Trees (Low Poly)/Prefab/Pine and fir trees/PineTree01color1.prefab"),
            AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/Trees (Low Poly)/Prefab/Pine and fir trees/PineTree01color2.prefab"),
            AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/Trees (Low Poly)/Prefab/Pine and fir trees/PineTree01color6.prefab"),
            AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/Trees (Low Poly)/Prefab/Pine and fir trees/PineTree01color6.prefab"),
            AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/Trees (Low Poly)/Prefab/Pine and fir trees/PineTree01color6.prefab"),
            AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/Trees (Low Poly)/Prefab/Pine and fir trees/PineTree01color7.prefab"),
            AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/Trees (Low Poly)/Prefab/Pine and fir trees/PineTree01color7.prefab"),
            
            AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/Trees (Low Poly)/Prefab/Pine and fir trees/PineTree2color1.prefab"),
            AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/Trees (Low Poly)/Prefab/Pine and fir trees/PineTree2color1.prefab"),
            AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/Trees (Low Poly)/Prefab/Pine and fir trees/PineTree2color1.prefab"),
            AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/Trees (Low Poly)/Prefab/Pine and fir trees/PineTree2color2.prefab"),
            AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/Trees (Low Poly)/Prefab/Pine and fir trees/PineTree2color6.prefab"),
            AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/Trees (Low Poly)/Prefab/Pine and fir trees/PineTree2color6.prefab"),
            AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/Trees (Low Poly)/Prefab/Pine and fir trees/PineTree2color6.prefab"),
            AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/Trees (Low Poly)/Prefab/Pine and fir trees/PineTree2color7.prefab"),
            AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/Trees (Low Poly)/Prefab/Pine and fir trees/PineTree2color7.prefab"),
            
            AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/Trees (Low Poly)/Prefab/Pine and fir trees/PineTree03color1.prefab"),
            AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/Trees (Low Poly)/Prefab/Pine and fir trees/PineTree03color1.prefab"),
            AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/Trees (Low Poly)/Prefab/Pine and fir trees/PineTree03color1.prefab"),
            AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/Trees (Low Poly)/Prefab/Pine and fir trees/PineTree03color2.prefab"),
            AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/Trees (Low Poly)/Prefab/Pine and fir trees/PineTree03color6.prefab"),
            AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/Trees (Low Poly)/Prefab/Pine and fir trees/PineTree03color6.prefab"),
            AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/Trees (Low Poly)/Prefab/Pine and fir trees/PineTree03color6.prefab"),
            AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/Trees (Low Poly)/Prefab/Pine and fir trees/PineTree03color7.prefab"),
            AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/Trees (Low Poly)/Prefab/Pine and fir trees/PineTree03color7.prefab"),
            
            AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/Trees (Low Poly)/Prefab/Pine and fir trees/PineTree04color1.prefab"),
            AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/Trees (Low Poly)/Prefab/Pine and fir trees/PineTree04color1.prefab"),
            AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/Trees (Low Poly)/Prefab/Pine and fir trees/PineTree04color1.prefab"),
            AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/Trees (Low Poly)/Prefab/Pine and fir trees/PineTree04color2.prefab"),
            AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/Trees (Low Poly)/Prefab/Pine and fir trees/PineTree04color6.prefab"),
            AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/Trees (Low Poly)/Prefab/Pine and fir trees/PineTree04color6.prefab"),
            AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/Trees (Low Poly)/Prefab/Pine and fir trees/PineTree04color6.prefab"),
            AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/Trees (Low Poly)/Prefab/Pine and fir trees/PineTree04color7.prefab"),
            AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/Trees (Low Poly)/Prefab/Pine and fir trees/PineTree04color7.prefab"),

            AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/Trees (Low Poly)/Prefab/Pine and fir trees/PineTree05.prefab"),
            AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/Trees (Low Poly)/Prefab/Pine and fir trees/PineTree05.prefab"),
            AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/Trees (Low Poly)/Prefab/Pine and fir trees/PineTree05.prefab"),
            AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/Trees (Low Poly)/Prefab/Pine and fir trees/PineTree05.prefab"),
            
            AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/Trees (Low Poly)/Prefab/Pine and fir trees/PineTree06color1.prefab"),
            AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/Trees (Low Poly)/Prefab/Pine and fir trees/PineTree06color1.prefab"),
            AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/Trees (Low Poly)/Prefab/Pine and fir trees/PineTree06color1.prefab"),
            AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/Trees (Low Poly)/Prefab/Pine and fir trees/PineTree06color2.prefab"),
            AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/Trees (Low Poly)/Prefab/Pine and fir trees/PineTree06color6.prefab"),
            AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/Trees (Low Poly)/Prefab/Pine and fir trees/PineTree06color6.prefab"),
            AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/Trees (Low Poly)/Prefab/Pine and fir trees/PineTree06color6.prefab"),
            AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/Trees (Low Poly)/Prefab/Pine and fir trees/PineTree06color7.prefab"),
            AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/Trees (Low Poly)/Prefab/Pine and fir trees/PineTree06color7.prefab")
        };
        bool resourcesExist = treefabs.Any(prefab => prefab != null);
        Debug.Log($"Tree prefabs exist: {resourcesExist}");

        GameObject folliageContainer = GameObject.Find("Trees");
        Terrain terrain = GameObject.Find("Terrain").GetComponent<Terrain>();
        int treesCreated = 0;
        for (int i = 0; i < maxAttempts; i++)
        {
            if (treesCreated >= numberOfTrees)
            {
                continue;
            }

            float x = Random.Range(left, right);
            float z = Random.Range(front, back);
            Vector3 position = new Vector3(x, 0f, z);

            float clearingRadius = 50f;
            if ((position - new Vector3(200f, 0f, 200f)).magnitude < clearingRadius)
            {
                continue;
            }

            position = new Vector3(x, terrain.SampleHeight(position), z);

            int treefabIndex = Random.Range(0, treefabs.Count);
            GameObject prefab = treefabs[treefabIndex];
            Quaternion randomRotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
            GameObject tree = Instantiate(prefab, position, randomRotation);
            float randomVerticalScale = ((float)Random.Range(60, 160)) / 100f;
            Vector3 scale = tree.transform.localScale;
            scale.y = randomVerticalScale;
            tree.transform.localScale = scale;
            Collider treeCollider = tree.GetComponent<Collider>();
            if (treeCollider != null)
            {
                Collider[] overlappingColliders = Physics.OverlapBox(
                    treeCollider.bounds.center,
                    treeCollider.bounds.extents,
                    tree.transform.rotation);
                overlappingColliders = overlappingColliders.Where(c => c.gameObject.name != "Terrain").ToArray();
                
                if (overlappingColliders.Length > 1)
                {
                    Debug.Log("The tree overlapped with another object.");
                    foreach (Collider c in overlappingColliders)
                    {
                        Debug.Log(c.gameObject.name);
                    }
                    DestroyImmediate(tree);
                    continue;
                }
            }

            treesCreated++;
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
