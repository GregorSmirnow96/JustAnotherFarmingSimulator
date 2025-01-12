using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBlastHitbox : MonoBehaviour
{
    public int damage = 6;
    public float knockBackDistance = 3f;
    public float knockBackDuration = 0.6f;
    public float stunDuration = 2f;

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

            ICCable ccable = collidedObject.GetComponent<ICCable>();
            if (ccable != null)
            {
                ccable.KnockBack(knockBackDistance, knockBackDuration, transform.position);
                ccable.Stun(stunDuration);
            }
        }
    }
}
