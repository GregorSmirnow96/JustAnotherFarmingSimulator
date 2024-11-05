using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowHitbox : MonoBehaviour
{
    public GameObject explosionPrefab;
    public GameObject damageHitboxPrefab;
    public GameObject arrowParticleSystem;

    void OnTriggerEnter(Collider other)
    {
        string collidedLayerName = LayerMask.LayerToName(other.gameObject.layer);
        if (collidedLayerName != "Player")
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Instantiate(damageHitboxPrefab, transform.position, Quaternion.identity);

            Destroy(arrowParticleSystem);
            Destroy(gameObject);
        }
    }
}
