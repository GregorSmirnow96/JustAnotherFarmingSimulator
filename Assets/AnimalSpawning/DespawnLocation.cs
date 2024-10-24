using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnLocation : MonoBehaviour
{
    void Start()
    {
        DespawnLocations.GetInstance().locations.Add(transform.position);
    }

    void OnTriggerEnter(Collider other)
    {
        // TODO: Refactor this. For some reason the animal isn't being destroyed, just one piece of them.
        Destroy(other.gameObject);
    }
}
