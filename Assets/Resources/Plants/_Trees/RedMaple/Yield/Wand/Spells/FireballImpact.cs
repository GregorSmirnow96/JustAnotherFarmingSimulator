using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballImpact : MonoBehaviour
{
    public int damage = 9;

    private int scaledDamage => PlayerProperties.GetScaledPlayerDamage(damage, DamageType.Fire);
    private List<GameObject> damagedObjects = new List<GameObject>();

    void OnTriggerEnter(Collider other)
    {
        GameObject collidedObject = other.gameObject;

        string collidedLayerName = LayerMask.LayerToName(collidedObject.layer);
        if (collidedLayerName != "Player" && !damagedObjects.Contains(collidedObject))
        {
            damagedObjects.Add(collidedObject);
            Health health = collidedObject.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(scaledDamage, DamageType.Fire);
            }
        }
    }
}
