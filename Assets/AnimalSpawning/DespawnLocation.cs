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
        Destroy(other.gameObject);
    }
}
