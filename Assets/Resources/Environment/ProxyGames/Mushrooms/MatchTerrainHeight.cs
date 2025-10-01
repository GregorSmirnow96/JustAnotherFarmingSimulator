using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchTerrainHeight : MonoBehaviour
{
    public string reparentingTargetName;

    void Start()
    {
        transform.parent = GameObject.Find(reparentingTargetName)?.transform;

        float xPosition = transform.position.x;
        float zPosition = transform.position.z;
        
        float localTerrainHeight = SceneProperties.TerrainHeightAtPosition(new Vector2(xPosition, zPosition));

        Vector3 adjustedPosition = new Vector3(xPosition, localTerrainHeight, zPosition);
        transform.position = adjustedPosition;
    }
}
