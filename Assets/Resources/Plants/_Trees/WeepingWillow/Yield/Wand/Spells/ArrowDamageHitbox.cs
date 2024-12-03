using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowDamageHitbox : MonoBehaviour
{
    public int damage = 8;

    private int scaledDamage => PlayerProperties.GetScaledPlayerDamage(damage, DamageType.Water);

    private List<GameObject> objectsHit = new List<GameObject>();

    void Start()
    {
        StartCoroutine(SelfDestruct());
    }

    private IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        GameObject collidedObject = other.gameObject;

        Health health = collidedObject.GetComponent<Health>();
        if (health != null && !objectsHit.Contains(collidedObject))
        {
            objectsHit.Add(collidedObject);
            health.TakeDamage(scaledDamage, DamageType.Water);
        }
    }
}
