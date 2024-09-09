using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CatalogEntry
{
    public string name;
    public GameObject plantObject;

    public CatalogEntry(string name, GameObject plantObject)
    {
        this.name = name;
        this.plantObject = plantObject;
    }
}

public class PlantCatalog : MonoBehaviour
{
    public static PlantCatalog globalPlantCatalog;

    public Dictionary<string, CatalogEntry> catalog;

    void Awake()
    {
        globalPlantCatalog = GameObject.Find("PlantCatalog").GetComponent<PlantCatalog>();
        catalog = new Dictionary<string, CatalogEntry>();
    }

    public void AddPlant(string key, string name, GameObject plantObject)
    {
        Debug.Log($"Catalogued: {name}");
        bool plantIsCatalogued = catalog.Keys.Contains(key);
        if (!plantIsCatalogued)
        {
            CatalogEntry newEntry = new CatalogEntry(name, plantObject);
            catalog.Add(key, newEntry);
        }
    }

    public void RemovePlant(string key)
    {
        catalog.Remove(key);
    }
}
