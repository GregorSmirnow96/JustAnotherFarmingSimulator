using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZapHitbox : MonoBehaviour
{
    public int damage = 10;
    public float slowDuration = 0.5f;
    public float slowStrength = 0.9f;

    private int scaledDamage => PlayerProperties.GetScaledPlayerDamage(damage, DamageType.Lightning);

    void OnTriggerEnter(Collider other)
    {
        GameObject collidedObject = other.gameObject;

        Health health = collidedObject.GetComponent<Health>();
        if (health != null)
        {
            health.TakeDamage(scaledDamage, DamageType.Lightning);
        }

        ICCable ccable = collidedObject.GetComponent<ICCable>();
        if (ccable != null)
        {
            ccable.Slow(slowDuration, slowStrength);
        }
    }
}
