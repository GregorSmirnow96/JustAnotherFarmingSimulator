using StixGames.GrassShader;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class TerrainModifier : MonoBehaviour
{
    public float brushSize = 5f;
    public float brushStrength = 0.01f;
    public Texture2D brushTexture;
    public int paintingIterations = 100;
    public float minBrushSize = 15f;
    public float maxBrushSize = 30f;
    public float minBrushStrength = 0.001f;
    public float maxBrushStrength = 0.002f;

    public Material grassMaterial;

    private Terrain terrain;
    private float[,] originalHeights;

    void Awake()
    {
        terrain = GetComponent<Terrain>();

        int width = terrain.terrainData.heightmapResolution;
        int height = terrain.terrainData.heightmapResolution;

        // Store the original heightmap
        originalHeights = terrain.terrainData.GetHeights(0, 0, width, height);

        // Randomize at runtime
        //RaiseTerrain(new Vector3(190f, 0f, 190f), brushSize, brushStrength, brushTexture);

        // Raise the terrain a bunch of times. The methodology? Experiment!
        // Start with:
        //  1) Iterate N times
        //  2) Select a random coordinate
        //  3) Skip if the coordinate is in the area of the player's cabin. We don't want the terrain raising into it.
        //  4) Use the selected (or select a random?) brush, select a value within a range for size and strength, and paint.
        //  *Potentially required feature: Log the coordinates you've painted. If a coordinate is too close to a previously painted coord, maybe skip it?
        Vector3 unaffectedArea = new Vector3(200f, 0f, 200f);
        float unaffectedRadius = 30f;
        for (int n = 0; n <= paintingIterations; n++)
        {
            float xCoord = Random.Range(50f, 350f);
            float zCoord = Random.Range(50f, 350f);
            Vector3 raiseCoord = new Vector3(xCoord, 0f, zCoord);

            int hillsInCluster = Random.Range(2, 8);
            float clusterRadius = Random.Range(5f, 25f);
            for (int nn = 0; nn <= hillsInCluster; nn++)
            {
                if ((unaffectedArea - raiseCoord).magnitude >= unaffectedRadius)
                {
                    float brushSize = Random.Range(minBrushSize, maxBrushSize);
                    float brushStrength = Random.Range(minBrushStrength, maxBrushStrength);

                    float currentHeight = terrain.SampleHeight(raiseCoord);
                    if (currentHeight < 1.8f)
                    {
                        float xOffset = Random.Range(0, clusterRadius) - (clusterRadius / 2);
                        float zOffset = Random.Range(0, clusterRadius) - (clusterRadius / 2);

                        Vector3 hillOffset = new Vector3(xOffset, 0f, zOffset);
                        RaiseTerrain(raiseCoord + hillOffset, brushSize, brushStrength, brushTexture);
                    }
                }
            }
        }

        // Paint random textures all over the terain... Did it manually. Can come back to this if I want to.

        // Add the GrassRenderer, then attach the grass shader to that script.
        GrassRenderer grassRenderer = gameObject.AddComponent<GrassRenderer>();
        grassRenderer.Material = grassMaterial;

        // Generate boulders.
        GenerateLargeRocks();
        GenerateSmallRocks();

        // Generate resource nodes.

        // Add the trees using the new terrain heights.
        GenerateTrees();
        SetRandomRotations();
        SetRandomVerticalScales();

        // Add smaller folliage / rocks. Some of these may be gatherable resources? Like herbs or bushes from bushes? Idk. Consider this.
        GenerateBushes();
        GenerateBranches();

        // This doesn't need to be generated here... BUT ADD THE BUTTERFLIES BACK! That'd be pretty :)
    }

    public void GenerateLargeRocks()
    {
        const int maxAttempts = 300;
        const int maxNumberOfRocks = 100;

        List<GameObject> rockPrefabs = new List<GameObject>()
        {
            AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Resources/ProceduralAssets/Boulder1.prefab"),
            AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Resources/ProceduralAssets/Boulder2.prefab"),
            AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Resources/ProceduralAssets/Boulder3.prefab"),
            AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Resources/ProceduralAssets/Cliff1.prefab"),
            AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Resources/ProceduralAssets/Cliff2.prefab"),
            AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Resources/ProceduralAssets/Cliff3.prefab"),
            AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Resources/ProceduralAssets/Cliff4.prefab"),
            AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Resources/ProceduralAssets/Cliff5.prefab"),
            AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Resources/ProceduralAssets/Cliff6.prefab")
        };
        System.Func<Vector3> calculateScale = () => {
            float scale = Random.Range(0.85f, 1.12f);
            return new Vector3(scale, scale, scale);
        };
        GameObject rockContainer = GameObject.Find("Rocks");
        float maxDistanceFromCenter = 100f;

        GenerateAssetsFromPool(maxAttempts, maxNumberOfRocks, SceneProperties.clearingRadius, maxDistanceFromCenter, calculateScale, rockPrefabs, rockContainer, 0f);
    }

    public void GenerateSmallRocks()
    {
        const int maxAttempts = 1000;
        const int maxNumberOfRocks = 800;

        List<GameObject> rockPrefabs = new List<GameObject>()
        {
            AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Resources/ProceduralAssets/SmallRock1.prefab"),
            AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Resources/ProceduralAssets/SmallRock2.prefab"),
            AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Resources/ProceduralAssets/SmallRock3.prefab"),
            AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Resources/ProceduralAssets/SmallRock4.prefab"),
            AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Resources/ProceduralAssets/SmallRock5.prefab"),
            AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Resources/ProceduralAssets/SmallRock6.prefab")
        };
        System.Func<Vector3> calculateScale = () => new Vector3(Random.Range(0.2f, 0.45f), Random.Range(0.2f, 0.45f), Random.Range(0.2f, 0.45f));
        GameObject rockContainer = GameObject.Find("Rocks");

        GenerateAssetsFromPool(maxAttempts, maxNumberOfRocks, SceneProperties.clearingRadius, calculateScale, rockPrefabs, rockContainer, 0f);
    }

    /* This function successfully generates trees randomly :D
        Create a function that places an object given (x, z, rotation, verticalScale, clearingRadius, and prefab)
        Refactor this function to use that placement function.
        Break this into 2 functions where:
            - one performs all the placements (deciding the x, z, rotation, and prefab) given a list of prefabs, max attempts, and goal placements.
            - another that defines the list of tree prefabs as done below, then passes them into the first function with the hardcoded values.
        Now that second function can be called repeatedly! Once for boulders, again for trees, then for bushes, mushrooms, etc.
     */
    public void GenerateTrees()
    {
        const int maxAttempts = 5000;
        const int numberOfTrees = 3000;

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
        System.Func<Vector3> calculateScale = () => new Vector3(1f, (Random.Range(0.6f, 1.6f) + Random.Range(0.6f, 1.6f)) / 2f, 1f);
        GameObject folliageContainer = GameObject.Find("Trees");

        GenerateAssetsFromPool(maxAttempts, numberOfTrees, SceneProperties.clearingRadius, calculateScale, treefabs, folliageContainer, 0f);
    }

    public void GenerateBushes()
    {
        const int maxAttempts = 500;
        const int numberOfBushes = 300;

        List<GameObject> bushPrefabs = new List<GameObject>()
        {
            AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Resources/ProceduralAssets/Bushes/Bush1.prefab"),
            AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Resources/ProceduralAssets/Bushes/Bush2.prefab")
        };

        System.Func<Vector3> calculateScale = () =>
        {
            float scale = Random.Range(0.6f, 1f);
            return new Vector3(scale, scale, scale);
        };
        GameObject bushContainer = GameObject.Find("Bushes");

        GenerateAssetsFromPool(maxAttempts, numberOfBushes, SceneProperties.clearingRadius - 4f, calculateScale, bushPrefabs, bushContainer, 0f);
    }

    public void GenerateBranches()
    {
        const int maxAttempts = 600;
        const int numberOfBranches = 400;

        List<GameObject> branchPrefabs = new List<GameObject>()
        {
            AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Resources/ProceduralAssets/FallenBranches/FallenBranch1.prefab"),
            AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Resources/ProceduralAssets/FallenBranches/FallenBranch2.prefab"),
            AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Resources/ProceduralAssets/FallenBranches/FallenBranch3.prefab"),
            AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Resources/ProceduralAssets/FallenBranches/FallenBranch4.prefab")
        };

        System.Func<Vector3> calculateScale = () =>
        {
            float scale = Random.Range(0.9f, 1.3f);
            return new Vector3(scale, scale, scale);
        };
        GameObject branchContainer = GameObject.Find("Branches");

        GenerateAssetsFromPool(maxAttempts, numberOfBranches, SceneProperties.clearingRadius + 4f, calculateScale, branchPrefabs, branchContainer, 1f);
    }

    private void GenerateAssetsFromPool(
        int maxAttempts,
        int maxObjectCreations,
        float minDistanceFromCenter,
        System.Func<Vector3> calculateScale,
        List<GameObject> prefabs,
        GameObject container,
        float verticalOffset)
    {
        GenerateAssetsFromPool(
            maxAttempts,
            maxObjectCreations,
            minDistanceFromCenter,
            SceneProperties.maxSceneX / 2,
            calculateScale,
            prefabs,
            container,
            verticalOffset);
    }

    private void GenerateAssetsFromPool(
        int maxAttempts,
        int maxObjectCreations,
        float minDistanceFromCenter,
        float maxDistanceFromCenter,
        System.Func<Vector3> calculateScale,
        List<GameObject> prefabs,
        GameObject container,
        float verticalOffset)
    {
        bool resourcesAreNotNull = prefabs.Any(prefab => prefab != null);
        Debug.Log($"Prefabs to generate assets from exist: {resourcesAreNotNull}");

        Terrain terrain = gameObject.GetComponent<Terrain>();
        int objectsCreated = 0;
        for (int i = 0; i < maxAttempts; i++)
        {
            if (objectsCreated >= maxObjectCreations)
            {
                break;
            }

            float x = Random.Range(SceneProperties.minSceneX, SceneProperties.maxSceneX);
            float z = Random.Range(SceneProperties.minSceneY, SceneProperties.maxSceneY);

            float distanceFromCenter = (new Vector2(x, z) - new Vector2(200f, 200f)).magnitude;
            if (distanceFromCenter < minDistanceFromCenter || distanceFromCenter > maxDistanceFromCenter)
            {
                continue;
            }

            int prefabIndex = Random.Range(0, prefabs.Count);
            GameObject prefab = prefabs[prefabIndex];

            Quaternion randomRotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
            Vector3 generatedScale = calculateScale();
            GameObject spawnedObject = AttemptToSpawnObject(x, z, randomRotation, generatedScale, prefab, verticalOffset);

            if (spawnedObject != null)
            {
                objectsCreated++;
                spawnedObject.transform.parent = container.transform;
            }
        }
    }

    private GameObject AttemptToPlaceObject(
        float x,
        float z,
        Quaternion rotation,
        GameObject prefab,
        float verticalOffset)
    {
        return AttemptToSpawnObject(
            x,
            z,
            rotation,
            new Vector3(1f, 1f, 1f),
            prefab,
            verticalOffset);
    }

    private GameObject AttemptToSpawnObject(
        float x,
        float z,
        Quaternion rotation,
        Vector3 scale,
        GameObject prefab,
        float verticalOffset)
    {
        float terrainHeight = SceneProperties.TerrainHeightAtPosition(new Vector2(x, z));
        float y = terrainHeight + verticalOffset;
        Vector3 position = new Vector3(x, y, z);

        GameObject newObject = Instantiate(prefab, position, rotation);

        newObject.transform.localScale = scale;
        ProceduralGenCollider pgc = newObject.GetComponent<ProceduralGenCollider>();
        Collider objectCollider = pgc?.GetCollider();
        if (objectCollider != null)
        {
            Collider[] overlappingColliders = Physics.OverlapBox(
                objectCollider.bounds.center,
                objectCollider.bounds.extents,
                newObject.transform.rotation);
            overlappingColliders = overlappingColliders.Where(c => c.gameObject.name != "Terrain").ToArray();

            if (overlappingColliders.Length > 1)
            {
                Debug.Log("The new object overlapped with another object.");
                foreach (Collider c in overlappingColliders)
                {
                    Debug.Log(c.gameObject.name);
                }
                DestroyImmediate(newObject);
                return null;
            }
        }

        if (pgc != null && pgc.deleteColliderAfterPlacement)
        {
            Destroy(objectCollider);
        }

        return newObject;
    }

    private void SetRandomRotations()
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

    private void SetRandomVerticalScales()
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

#if UNITY_EDITOR
    void OnApplicationQuit()
    {
        RestoreTerrain();
    }
#endif

    public void RaiseTerrain(
        Vector3 worldPosition,
        float brushSize,
        float brushStrength,
        Texture2D brushTexture)
    {
        TerrainData terrainData = terrain.terrainData;

        int heightmapWidth = terrainData.heightmapResolution;
        int heightmapHeight = terrainData.heightmapResolution;

        // Convert world position to normalized terrain coordinates (0-1 range)
        Vector3 terrainPos = worldPosition - terrain.transform.position;
        float normX = terrainPos.x / terrainData.size.x;
        float normZ = terrainPos.z / terrainData.size.z;

        // Convert normalized coords to heightmap coords
        int centerX = Mathf.RoundToInt(normX * heightmapWidth);
        int centerZ = Mathf.RoundToInt(normZ * heightmapHeight);

        // Brush size in heightmap pixels
        int brushSizePixels = Mathf.RoundToInt((brushSize / terrainData.size.x) * heightmapWidth);

        // Get current heights in area of the brush
        int offsetX = centerX - brushSizePixels / 2;
        int offsetZ = centerZ - brushSizePixels / 2;
        float[,] heights = terrainData.GetHeights(offsetX, offsetZ, brushSizePixels, brushSizePixels);

        for (int x = 0; x < brushSizePixels; x++)
        {
            for (int z = 0; z < brushSizePixels; z++)
            {
                // Distance from brush center (in pixels)
                float dist = Vector2.Distance(new Vector2(x, z), new Vector2(brushSizePixels / 2, brushSizePixels / 2));
                float strength = 1f - Mathf.Clamp01(dist / (brushSizePixels / 2f));

                // Apply brush texture if given
                if (brushTexture != null)
                {
                    float u = (float)x / brushSizePixels;
                    float v = (float)z / brushSizePixels;
                    strength *= brushTexture.GetPixelBilinear(u, v).a; // using alpha as mask
                }

                heights[z, x] += brushStrength * strength;
            }
        }

        // Apply the modified heights
        terrainData.SetHeights(offsetX, offsetZ, heights);
    }

    private void RandomizeTerrain()
    {
        int width = terrain.terrainData.heightmapResolution;
        int height = terrain.terrainData.heightmapResolution;
        float[,] heights = new float[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                heights[x, y] = Mathf.PerlinNoise(x * 0.05f, y * 0.05f) * 0.1f;
            }
        }

        terrain.terrainData.SetHeights(0, 0, heights);
    }

    private void RestoreTerrain()
    {
        if (originalHeights != null)
        {
            terrain.terrainData.SetHeights(0, 0, originalHeights);
        }
    }
}
