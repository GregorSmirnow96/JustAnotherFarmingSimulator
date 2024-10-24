using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestCurse : MonoBehaviour
{
    public Vector2 sceneCenter = new Vector2(200f, 200f);
    public float maxDistance = 150f;

    private Vector2 xzPlayerPosition => new Vector2(gameObject.transform.position.x, gameObject.transform.position.z);
    private float distanceFromCenter => (xzPlayerPosition - sceneCenter).magnitude;

    void Update()
    {
        var obit = ItemMetaData.ItemTypeRepo.GetInstance().TryFindItemType("OrangeBerryBush");
        Debug.Log(obit.id);
        // THIS WON'T WORK!
        // The player will be teleported infinitely because the new position will also trigger this condition.
        // If you teleport them to the opposite side, but a little closer to the center, this could work, but consider camera orientation.
        // Probably just face their camera towards the center. :) Good job, good solution
        if (distanceFromCenter > maxDistance)
        {
            Vector2 playerSceneOffset = xzPlayerPosition - sceneCenter;
            gameObject.transform.position = sceneCenter - playerSceneOffset;
        }
    }
}
