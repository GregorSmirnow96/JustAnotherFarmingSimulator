using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spawning
{
    public static class PlantSpawner
    {
        public static Dictionary<int, GameObject> plantPrefabById = new Dictionary<int, GameObject>()
        {
            { 1, Resources.Load<GameObject>("Crops/RedYeller/S1") },
            { 2, Resources.Load<GameObject>("Crops/Plant2/S1") },
            { 3, Resources.Load<GameObject>("Crops/Plant3/S1") },
            { 4, Resources.Load<GameObject>("Crops/Plant4/S1") },
            { 5, Resources.Load<GameObject>("Crops/Plant5/S1") },
            { 6, Resources.Load<GameObject>("Crops/Plant6/S1") },
            { 7, Resources.Load<GameObject>("Crops/Plant7/S1") },
            { 8, Resources.Load<GameObject>("Crops/Plant8/S1") },
            { 9, Resources.Load<GameObject>("Crops/Plant9/S1") },
            { 10, Resources.Load<GameObject>("Crops/Plant10/S1") }
        };

        public static void SpawnPlant(Vector3 position, int id)
        {
            GameObject prefab = plantPrefabById[id];
            GameObject newObject = MonoBehaviour.Instantiate(prefab, position, Quaternion.identity);
        }
    }
}
