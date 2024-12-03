using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningOrbHitbox : MonoBehaviour
{
    public int damage = 8;
    public float duration = 2f;

    private int scaledDamage => PlayerProperties.GetScaledPlayerDamage(damage, DamageType.Lightning);

    void Start()
    {
        StartCoroutine(SelfDestruct());
    }

    private IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        GameObject collidedObject = other.gameObject;

        Health health = collidedObject.GetComponent<Health>();
        if (health != null)
        {
            health.TakeDamage(scaledDamage, DamageType.Lightning);
        }
    }
}
