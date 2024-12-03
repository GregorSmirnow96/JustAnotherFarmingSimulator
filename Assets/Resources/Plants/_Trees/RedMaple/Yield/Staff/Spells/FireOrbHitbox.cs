using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireOrbHitbox : MonoBehaviour
{
    public GameObject orbParticlePrefab;
    public GameObject impactPrefab;
    public int damage = 5;

    private int scaledDamage => PlayerProperties.GetScaledPlayerDamage(damage, DamageType.Fire);

    private Vector3 lastPlayerPosition;
    private Transform playerTransform;

    void Start()
    {
        Instantiate(orbParticlePrefab, transform);

        playerTransform = SceneProperties.playerTransform;
        lastPlayerPosition = playerTransform.position;
    }

    void Update()
    {
        Vector3 currentPlayerPosition = playerTransform.position;
        Vector3 playerPositionDelta = currentPlayerPosition - lastPlayerPosition;
        transform.position = transform.position + playerPositionDelta;

        lastPlayerPosition = currentPlayerPosition;
    }

    void OnTriggerEnter(Collider other)
    {
        GameObject collidedObject = other.gameObject;

        string collidedLayerName = LayerMask.LayerToName(collidedObject.layer);
        if (collidedLayerName != "Player")
        {
            Health health = collidedObject.GetComponent<Health>();
            if (health != null)
            {
                Vector3 collisionPoint = other.ClosestPoint(transform.position);
                Instantiate(impactPrefab, collisionPoint, Quaternion.identity);

                health.TakeDamage(scaledDamage, DamageType.Fire);
            }
        }
    }
}
