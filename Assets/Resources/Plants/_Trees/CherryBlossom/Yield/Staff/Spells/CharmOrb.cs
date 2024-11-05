using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharmOrb : MonoBehaviour
{
    private float orbDuration;
    private float spawnTime;

    void Start()
    {
        spawnTime = Time.time;
        ParticleSystem orbParticleSystem = GetComponent<ParticleSystem>();
        orbDuration = orbParticleSystem.main.startLifetime.constant;
    }

    void OnTriggerEnter(Collider other)
    {
        GameObject collidedObject = other.gameObject;

        ICCable ccable = collidedObject.GetComponent<ICCable>();
        if (ccable != null)
        {
            float elapsedTime = Time.time - spawnTime;
            float remainingOrbTime = orbDuration - elapsedTime;
            ccable.Charm(remainingOrbTime, transform);
        }
    }
}
