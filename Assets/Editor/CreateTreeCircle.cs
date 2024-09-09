using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

public class CreateTreeCircle : MonoBehaviour
{
    public static Vector3 CENTER = new Vector3(200, 0, 200);

    [MenuItem("Tools/Create Tree Circles")]
    public static void CreateTrees()
    {
        GeneratePointsAroundCircle(CENTER, 55, 6);

        Debug.Log("Trees created successfully.");
    }

    static void GeneratePointsAroundCircle(Vector3 center, float radius, float angleStep)
    {
        GameObject tree3Prefab = Resources.Load<GameObject>($"Tree 3");

        for (float angle = 0; angle < 360; angle += angleStep)
        {
            // Convert angle to radians
            float rad = angle * Mathf.Deg2Rad;

            // Calculate X and Z position using cosine and sine
            float x = center.x + Mathf.Cos(rad) * radius;
            float z = center.z + Mathf.Sin(rad) * radius;

            // Create the point (on the X-Z plane)
            Vector3 point = new Vector3(x, center.y, z);

            // Optionally, log the points
            Instantiate(tree3Prefab, point, Quaternion.identity);
        }
    }
}
