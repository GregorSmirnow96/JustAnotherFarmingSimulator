using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

public class ResourceFolliageActions : MonoBehaviour
{
    [MenuItem("Tools/Folliage/Mushrooms/GenerateMushrooms")]
    public static void GeneratePlants()
    {
        const int maxAttempts = 500;
        const int numberOfMushrooms = 50;
        const float left = 41.62077f;
        const float right = 245.2508f;
        const float front = 134.6476f;
        const float back = 333.3976f;

        List<GameObject> prefabs = new List<GameObject>()
        {
            AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Resources/Environment/ProxyGames/Mushrooms/MushroomContainer.prefab")
        };

        bool resourcesExist = prefabs.Any(prefab => prefab != null);
        Debug.Log($"Mushroom prefabs exist: {resourcesExist}");

        GameObject mushroomContainer = GameObject.Find("Mushrooms");
        Terrain terrain = GameObject.Find("Terrain").GetComponent<Terrain>();
        int mushroomsCreated = 0;
        for (int i = 0; i < maxAttempts; i++)
        {
            if (mushroomsCreated >= numberOfMushrooms)
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
            GameObject mushroom = Instantiate(prefab, position, randomRotation);
            Collider mushroomCollider = mushroom.GetComponent<Collider>();
            if (mushroomCollider != null)
            {
                Collider[] overlappingColliders = Physics.OverlapBox(
                    mushroomCollider.bounds.center,
                    mushroomCollider.bounds.extents,
                    mushroom.transform.rotation);
                overlappingColliders = overlappingColliders.Where(c => c.gameObject.name != "Terrain").ToArray();
                
                if (overlappingColliders.Length > 1)
                {
                    Debug.Log("The mushroom overlapped with another object.");
                    foreach (Collider c in overlappingColliders)
                    {
                        Debug.Log(c.gameObject.name);
                    }
                    DestroyImmediate(mushroom);
                    continue;
                }
            }

            mushroomsCreated++;
            mushroom.transform.parent = mushroomContainer.transform;
        }
    }
}
