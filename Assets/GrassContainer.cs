using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GrassContainer : MonoBehaviour
{
    public GameObject grassTilePrefab;
    public int minX;
    public int maxX;
    public int minZ;
    public int maxZ;

    public float scale;

    private int cellSize = 2;
    private Dictionary<int, Dictionary<int, List<GameObject>>> grassGrid;
    private Dictionary<GameObject, List<GameObject>> grassCells;

    // Start is called before the first frame update
    void Start()
    {
        InitializeGrid();
        GenerateGrass();
    }

    private void InitializeGrid()
    {
        grassCells = new Dictionary<GameObject, List<GameObject>>();
        grassGrid = new Dictionary<int, Dictionary<int, List<GameObject>>>();
        for (int z = minZ; z <= maxZ; z = z + cellSize)
        {
            Dictionary<int, List<GameObject>> row = new Dictionary<int, List<GameObject>>();

            for (int x = minX; x <= maxX; x = x + cellSize)
            {
                List<GameObject> cell = new List<GameObject>();
                row.Add(x, cell);
            }
            grassGrid.Add(z, row);
        }
    }

    private void GenerateGrass()
    {
        Quaternion rotation1 = Quaternion.identity;
        Quaternion rotation2 = Quaternion.Euler(0, 90, 0);
        Quaternion rotation3 = Quaternion.Euler(0, 180, 0);
        Quaternion rotation4 = Quaternion.Euler(0, 270, 0);
        Quaternion[] rotations = new Quaternion[]
        {
            rotation1,
            rotation2,
            rotation3,
            rotation4
        };
        int rotationIndex = 0;
        for (int z = (int)(minZ*scale); z <= (int)(maxZ*scale); z++)
        {
            for (int x = (int)(minX*scale); x <= (int)(maxX*scale); x++)
            {
                Vector3 position = new Vector3(
                    (float)x/scale,
                    0.0f,
                    (float)z/scale
                );
                GameObject grass = Instantiate(grassTilePrefab, position, rotations[UnityEngine.Random.Range(0, 4)]);
                grass.tag = "Grass";

                int cellX = ((int)(x/scale/cellSize)) * 2;
                int cellZ = ((int)(z/scale/cellSize)) * 2;
                List<GameObject> cell = grassGrid[cellZ][cellX];
                cell.Add(grass);
                grassCells.Add(grass, cell);
            }
        }
    }

    public List<GameObject> GetGrassObjects(Vector3 digPosition, float digRadius)
    {
        float minDigX = digPosition.x - digRadius;
        float maxDigX = digPosition.x + digRadius;
        float minDigZ = digPosition.z - digRadius;
        float maxDigZ = digPosition.z + digRadius;

        int minDigXIndex = Mathf.RoundToInt(minDigX / cellSize) * cellSize;
        int maxDigXIndex = Mathf.RoundToInt(maxDigX / cellSize) * cellSize;
        int minDigZIndex = Mathf.RoundToInt(minDigZ / cellSize) * cellSize;
        int maxDigZIndex = Mathf.RoundToInt(maxDigZ / cellSize) * cellSize;

        List<GameObject> grassObjects = new List<GameObject>();
        for (int zIndex = minDigZIndex; zIndex <= maxDigZIndex; zIndex = zIndex + cellSize)
        {
            for (int xIndex = minDigXIndex; xIndex <= maxDigXIndex; xIndex = xIndex + cellSize)
            {
                List<GameObject> grassObjectsInCell = grassGrid[zIndex][xIndex];
                grassObjects.AddRange(grassObjectsInCell);
            }
        }

        return grassObjects.Where(grass => (grass.transform.position - digPosition).magnitude <= digRadius).ToList();
    }

    public void DestroyGrass(GameObject grass)
    {
        grassCells[grass].Remove(grass);
        Destroy(grass);
    }
}
