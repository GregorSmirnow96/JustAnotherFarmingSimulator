using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RemovesGrass : MonoBehaviour
{
    public float radius = 2f;

    void Start()
    {
        GameObject grassRemoverPrefab = AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/GrassRemovalSphere.prefab");
        GameObject grassRemover = Instantiate(grassRemoverPrefab, transform);
        grassRemover.transform.localScale = new Vector3(radius, radius, radius);
    }
}
