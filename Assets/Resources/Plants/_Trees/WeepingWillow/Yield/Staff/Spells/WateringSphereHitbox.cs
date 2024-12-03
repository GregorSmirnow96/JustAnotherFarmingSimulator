using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WateringSphereHitbox : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        IWaterable waterableScript = other.gameObject.GetComponent<IWaterable>();
        if (waterableScript != null)
        {
            if (waterableScript != null)
            {
                bool needsWater = !(waterableScript.IsGrowing() || waterableScript.IsWatered());
                if (needsWater)
                {
                    waterableScript.AddWater();
                }
            }
        }
    }
}
