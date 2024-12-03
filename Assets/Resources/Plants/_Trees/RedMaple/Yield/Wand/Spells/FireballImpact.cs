using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballImpact : MonoBehaviour
{
    public int damage = 9;

    private int scaledDamage => PlayerProperties.GetScaledPlayerDamage(damage, DamageType.Fire);

    void OnTriggerEnter(Collider other)
    {
        GameObject collidedObject = other.gameObject;

        string collidedLayerName = LayerMask.LayerToName(collidedObject.layer);
        if (collidedLayerName != "Player")
        {
            Health health = collidedObject.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(scaledDamage, DamageType.Fire);
            }
        }
    }
}
