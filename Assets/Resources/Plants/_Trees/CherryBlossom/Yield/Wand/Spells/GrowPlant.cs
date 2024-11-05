using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowPlant : MonoBehaviour
{
    public List<PlantStageGrowth> effectedGrowthScripts;

    void OnTriggerEnter(Collider other)
    {
        GameObject collidedObject = other.gameObject;

        PlantStageGrowth plantStageGrowth = collidedObject.GetComponent<PlantStageGrowth>();
        if (plantStageGrowth == null)
        {
            Transform collidedParent = collidedObject.transform.parent;
            plantStageGrowth = collidedParent?.GetComponent<PlantStageGrowth>();
        }
        if (plantStageGrowth != null)
        {
            if (!effectedGrowthScripts.Contains(plantStageGrowth))
            {
                effectedGrowthScripts.Add(plantStageGrowth);
                plantStageGrowth.IncrementPhase();
            }
        }
    }
}
