using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DespawnLocations
{
    private static DespawnLocations instance;

    public static DespawnLocations GetInstance() =>
        instance == null
            ? instance = new DespawnLocations()
            : instance;

    public List<Vector3> locations;

    private DespawnLocations()
    {
        locations = new List<Vector3>();
    }

    public Vector3 GetNearestDespawnLocation(Vector3 position)
    {
        return locations
            .OrderBy(despawnLocation => Vector3.Distance(despawnLocation, position))
            .First();
    }

    public Vector3 GetFurthestDespawnLocation(Vector3 position)
    {
        return locations
            .OrderBy(despawnLocation => Vector3.Distance(despawnLocation, position))
            .Last();
    }
}
