using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateMaterial : MonoBehaviour
{
    public Material baseMaterial;

    private Material instancedMaterial;
    private Color potionColor;

    void Start()
    {
        instancedMaterial = new Material(baseMaterial);
        EquippedItem equippedItem = GetComponent<EquippedItem>();
        PotionItem equippedPotion = (PotionItem) equippedItem.item;
        potionColor = equippedPotion.potionColor;

        Transform childTransform = transform.GetChild(0);
        if (childTransform != null)
        {
            Renderer childRenderer = childTransform.GetComponent<Renderer>();
            if (childRenderer != null)
            {
                Material[] materials = childRenderer.materials;
                int index = Array.FindIndex(materials, material => material.name.Contains("Liquid"));
                if (index != -1)
                {
                    materials[index] = instancedMaterial;
                    childRenderer.materials = materials;
                }
            }
        }
        else
        {
            Debug.LogWarning("Child object not found.");
        }

        instancedMaterial.SetColor("_AlbedoColor", potionColor);
        instancedMaterial.SetColor("_EmissionColor", potionColor * 0.65f);
    }
}
