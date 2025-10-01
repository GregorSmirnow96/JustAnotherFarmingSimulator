using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestructWhenFarFromCenter : MonoBehaviour
{
    public float maxDistance = 90f;

    void Start()
    {
        float distanceFromCenter = (SceneProperties.sceneCenter - transform.position).magnitude;
        if (distanceFromCenter > maxDistance)
        {
            Destroy(gameObject);
        }
    }
}
