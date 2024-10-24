using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ContainerMap
{
    public string emptyItemId;
    public string fullItemId;
    public float volume;

    public ContainerMap(string emptyItemId, string fullItemId, float volume)
    {
        this.emptyItemId = emptyItemId;
        this.fullItemId = fullItemId;
        this.volume = volume;
    }
}

public class WaterContainers
{
    private static WaterContainers Instance;
    public static WaterContainers GetInstance() =>
        Instance == null
            ? Instance = new WaterContainers()
            : Instance;

    private List<ContainerMap> containerMaps;

    private WaterContainers()
    {
        containerMaps = new List<ContainerMap>()
        {
            new ContainerMap("Bowl", "BowlOfWater", 1f)
        };
    }

    public bool ItemIsWaterContainer(string itemId) =>
        containerMaps.Any(map => map.emptyItemId.Equals(itemId) || map.fullItemId.Equals(itemId));

    public bool ItemIsEmptyContainer(string itemId) => containerMaps.Any(map => map.emptyItemId.Equals(itemId));

    public bool ItemIsFullContainer(string itemId) => containerMaps.Any(map => map.fullItemId.Equals(itemId));

    public ContainerMap GetContainerMap(string itemId) =>
        containerMaps
            .FirstOrDefault(map => map.emptyItemId.Equals(itemId) || map.fullItemId.Equals(itemId));

    public float? GetContainerVolume(string itemId) =>
        containerMaps
            .FirstOrDefault(map => map.emptyItemId == itemId || map.fullItemId == itemId)
            ?.volume;
}
