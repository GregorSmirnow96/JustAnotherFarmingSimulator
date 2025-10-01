using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityComponent : MonoBehaviour
{
    public GameObject proximityScript;
    public float maximumProximity = 5f;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if ((SceneProperties.playerTransform.position - transform.position).magnitude <= maximumProximity)
        {
            proximityScript.active = true;
        }
        else
        {
            proximityScript.active = false;
        }
    }
}
