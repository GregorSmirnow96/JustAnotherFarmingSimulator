using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowDamageHitbox : MonoBehaviour
{
    public int damage = 12;

    private List<GameObject> objectsHit = new List<GameObject>();

    void Start()
    {
        StartCoroutine(SelfDestruct());
    }

    private IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(2f);
        Debug.Log("Destroying self (arrow dmg)");
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        GameObject collidedObject = other.gameObject;

        Debug.Log($"Arrows damaging: {collidedObject.name}");

        Health health = collidedObject.GetComponent<Health>();
        if (health != null && !objectsHit.Contains(collidedObject))
        {
            objectsHit.Add(collidedObject);
            health.TakeDamage(damage);
        }
    }
}
