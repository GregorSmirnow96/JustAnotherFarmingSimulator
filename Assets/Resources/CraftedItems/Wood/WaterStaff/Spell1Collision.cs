using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell1Collision : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
    }

    private void OnTriggerEnter(Collider otherCollider)
    {
        GameObject hitObject = otherCollider.gameObject;

        Health healthScript = hitObject.gameObject.GetComponent<Health>();
        if (healthScript == null)
        {
            healthScript = hitObject.gameObject.transform.parent.gameObject.GetComponentInParent<Health>();
        }
        if (healthScript != null)
        {
            Debug.Log($"WATER SPELL HIT: {healthScript.gameObject.name}");
        }
    }
}
