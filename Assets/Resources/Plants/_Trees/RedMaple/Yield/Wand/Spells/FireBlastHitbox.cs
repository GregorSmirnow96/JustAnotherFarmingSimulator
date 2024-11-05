using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBlastHitbox : MonoBehaviour
{
    public int damage = 6;
    public float knockBackDistance = 3f;
    public float knockBackDuration = 0.6f;
    public float stunDuration = 2f;

    void OnTriggerEnter(Collider other)
    {
        GameObject collidedObject = other.gameObject;

        string collidedLayerName = LayerMask.LayerToName(collidedObject.layer);
        if (collidedLayerName != "Player")
        {
            Health health = collidedObject.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }

            ICCable ccable = collidedObject.GetComponent<ICCable>();
            if (health != null)
            {
                ccable.KnockBack(knockBackDistance, knockBackDuration, transform.position);
                ccable.Stun(stunDuration);
            }
        }
    }
}
