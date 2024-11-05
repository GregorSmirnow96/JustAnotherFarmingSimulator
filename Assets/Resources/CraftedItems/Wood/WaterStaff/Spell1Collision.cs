using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell1Collision : MonoBehaviour
{
    public int damage = 12;

    private void OnTriggerEnter(Collider otherCollider)
    {
        GameObject hitObject = otherCollider.gameObject;

        Debug.Log(hitObject.name);

        string collidedLayerName = LayerMask.LayerToName(hitObject.layer);
        if (collidedLayerName != "Player")
        {
            Health healthScript = hitObject.GetComponent<Health>();
            Debug.Log($"Tried to get Health script from: {hitObject.name}");
            if (healthScript == null)
            {
                healthScript = hitObject.transform?.parent?.gameObject.GetComponentInParent<Health>();
            }
            if (healthScript != null)
            {
                Debug.Log($"WATER SPELL HIT: {healthScript.gameObject.name}");
                healthScript.TakeDamage(damage);
            }
        }
    }
}
