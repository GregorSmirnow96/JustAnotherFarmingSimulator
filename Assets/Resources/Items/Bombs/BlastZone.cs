using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DamageTypePair
{
    public int damage;
    public string damageType;
}

public class BlastZone : MonoBehaviour
{
    public List<DamageTypePair> damageTypePairs;

    private List<GameObject> damagedObjects = new List<GameObject>();

    private int ScaledDamage(int damage, string damageType) => PlayerProperties.GetScaledPlayerDamage(damage, damageType);

    void OnTriggerEnter(Collider other)
    {
        GameObject collidedObject = other.gameObject;

        if (!damagedObjects.Contains(collidedObject))
        {
            damagedObjects.Add(collidedObject);
            Health health = collidedObject.GetComponent<Health>();
            if (health != null)
            {
                foreach (DamageTypePair dtp in damageTypePairs)
                {
                    int scaledDamage = ScaledDamage(dtp.damage, dtp.damageType);
                    health.TakeDamage(scaledDamage, dtp.damageType);
                }
            }
        }
    }
}
