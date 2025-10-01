using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralGenCollider : MonoBehaviour
{
    public GameObject objectWithCollider;
    public bool deleteColliderAfterPlacement = false;

    public Collider GetCollider()
    {
        if (objectWithCollider == null)
        {
            return gameObject.GetComponent<Collider>();
        }
        else
        {
            return objectWithCollider.GetComponent<Collider>();
        }
    }
}
