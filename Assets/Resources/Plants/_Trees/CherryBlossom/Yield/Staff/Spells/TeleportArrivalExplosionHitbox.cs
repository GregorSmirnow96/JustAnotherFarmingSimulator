using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportArrivalHitbox : MonoBehaviour
{
    public float knockBackDistance = 4f;
    public float knockBackDuration = 0.4f;

    void OnTriggerEnter(Collider other)
    {
        GameObject collidedObject = other.gameObject;

        if (collidedObject.name == "Player")
        {
            return;
        }

        ICCable ccable = collidedObject.GetComponent<ICCable>();
        if (ccable != null)
        {
            ccable.KnockBack(knockBackDistance, knockBackDuration, transform.position);
        }
    }
}
