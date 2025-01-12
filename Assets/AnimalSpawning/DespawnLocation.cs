using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnLocation : MonoBehaviour
{
    void Start()
    {
        DespawnLocations.GetInstance().locations.Add(transform.position);
        Debug.Log(DespawnLocations.GetInstance().locations.Count);
    }

    void OnTriggerEnter(Collider other)
    {
        GameObject collidedObject = other.gameObject;
        AnimalBehaviour animalBehaviourScript = collidedObject.GetComponent<AnimalBehaviour>();
        if (animalBehaviourScript.shouldDespawn)
        {
            Destroy(other.gameObject);
        }
    }
}
