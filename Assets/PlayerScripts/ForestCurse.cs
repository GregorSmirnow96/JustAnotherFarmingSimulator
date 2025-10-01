using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestCurse : MonoBehaviour
{
    public float maxDistance = 150f;

    private Vector2 xzPlayerPosition => new Vector2(gameObject.transform.position.x, gameObject.transform.position.z);

    void Update()
    {
        float distanceFromCenter = SceneProperties.playerDistanceFromCenter;
        if (distanceFromCenter > maxDistance)
        {
            Vector2 sceneCenter = SceneProperties.sceneCenter.ToXZ();
            Vector2 playerSceneOffset = xzPlayerPosition - sceneCenter;
            Vector2 playerSceneOffsetNormal = playerSceneOffset.normalized;
            Vector2 targetPosition = sceneCenter - playerSceneOffsetNormal * maxDistance * 0.9f;
            float height = SceneProperties.TerrainHeightAtPosition(targetPosition) + 0.2f;
            gameObject.transform.position = new Vector3(targetPosition.x, height, targetPosition.y);

        }
    }
}
