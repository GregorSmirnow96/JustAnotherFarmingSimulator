using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector3 startPosition;
    public GameObject target;
    public float travelTime;
    public int damage;
    public object onHitEffects; // TODO: This isn't used right now. Come up with what this type will be and how to perform the on hit effects.
    private float timeElapsed;

    private float percentTravelled => timeElapsed / travelTime;
    private Vector3 startToTargetVector => target.transform.position - startPosition;
    private Vector3 currentPosition => startPosition + startToTargetVector * percentTravelled;

    void Start()
    {
        timeElapsed = 0.0f;
    }

    void Update()
    {
        timeElapsed += Time.deltaTime;
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }
        else if (timeElapsed >= travelTime)
        {
            Health healthScript = target.GetComponent<Health>();
            healthScript.TakeDamage(damage, DamageType.Physical);
            Destroy(gameObject);
            return;
        }

        gameObject.transform.position = currentPosition;
    }
}
