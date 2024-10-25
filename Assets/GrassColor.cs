using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassColor : MonoBehaviour
{
    Material grassMaterial;

    void Start()
    {
    }

    void Update()
    {
        if (grassMaterial == null)
        {
            InitializeShader();
        }

        Debug.Log(grassMaterial.name);
    }

    private void InitializeShader()
    {
        foreach (Transform child in transform)
        {
            if (child.name.Contains("VisibilityMesh"))
            {
                if (grassMaterial == null)
                {
                    Renderer renderer = child.gameObject.GetComponent<Renderer>();
                    grassMaterial = renderer.material;
                }
                else
                {
                    Renderer renderer = child.gameObject.GetComponent<Renderer>();
                    renderer.material = grassMaterial;
                }
            }
        }
    }
}
