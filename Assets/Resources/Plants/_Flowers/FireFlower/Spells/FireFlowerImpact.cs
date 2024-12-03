using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFlowerImpact : MonoBehaviour
{
    public int damage = 14;

    private int scaledDamage => PlayerProperties.GetScaledPlayerDamage(damage, DamageType.Fire);

    void OnTriggerEnter(Collider other)
    {
        GameObject collidedObject = other.gameObject;

        Health health = collidedObject.GetComponent<Health>();
        if (health != null)
        {
            health.TakeDamage(scaledDamage, DamageType.Fire);
        }
    }
}
