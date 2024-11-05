using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningWave : MonoBehaviour
{
    public float knockBackDistance = 6f;
    public float knockBackDuration = 0.8f;
    public int damage = 4;

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
            }
        }
    }
}
