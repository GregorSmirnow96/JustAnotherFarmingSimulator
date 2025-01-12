using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleHitbox : MonoBehaviour
{
    public int damage = 2;
    public float displacementSpeed = 0.2f;
    public float displacementDuration = 0.4f;

    private int scaledDamage => PlayerProperties.GetScaledPlayerDamage(damage, DamageType.Lightning);

    private List<GameObject> collidedObjects = new List<GameObject>();

    void OnTriggerEnter(Collider other)
    {
        GameObject collidedObject = other.gameObject;

        if (collidedObjects.Contains(collidedObject))
        {
            return;
        }
        else
        {
            collidedObjects.Add(collidedObject);
        }

        Health health = collidedObject.GetComponent<Health>();
        if (health != null)
        {
            health.TakeDamage(scaledDamage, DamageType.Lightning);
            Vector3 step = (transform.position - collidedObject.transform.position) * displacementSpeed;
            step = new Vector3(step.x, 0f, step.z);

            StartCoroutine(PullInObject(collidedObject.transform, step));
        }
    }

    private IEnumerator PullInObject(Transform collidedTransform, Vector3 displacement)
    {
        float timer = 0f;
        while (timer <= displacementDuration)
        {
            timer += Time.deltaTime;
            float magnitude = Time.deltaTime / displacementDuration;
            if (collidedTransform != null)
            {
                collidedTransform.position = collidedTransform.position + displacement * magnitude;
            }
            yield return null;
        }
    }
}
