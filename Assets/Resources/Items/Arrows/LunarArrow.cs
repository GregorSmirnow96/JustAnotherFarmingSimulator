using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LunarArrow : MonoBehaviour, IArrow
{
    public GameObject impactParticleSystem;

    private Coroutine flightCoroutine;
    private bool wasFired;

    public void Fire()
    {
        wasFired = true;
        flightCoroutine = StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        const float maxFlightTime = 6f;
        const float flightSpeed = 30f;
        const float rotationSpeed = 20f;

        float flightTimer = 0;
        while (flightTimer <= maxFlightTime)
        {
            // Move forward
            transform.position += -transform.right * flightSpeed * Time.deltaTime;
            // And tilt down
            transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime, Space.Self);

            flightTimer += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }

    private int ScaledDamage(
        int damage,
        string damageType) =>
            PlayerProperties.GetScaledPlayerDamage(damage, damageType);

    private void OnTriggerEnter(Collider other)
    {
        if (!wasFired) return;

        StopCoroutine(flightCoroutine);

        Instantiate(impactParticleSystem, transform.position, Quaternion.identity);

        GameObject collidedObject = other.gameObject;
        ICCable ccable = collidedObject.GetComponent<ICCable>();
        if (ccable != null)
        {
            ccable.Slow(4f, 0.9f);
        }
        Health health = collidedObject.GetComponent<Health>();
        if (health != null)
        {
            health.TakeDamage(ScaledDamage(20, DamageType.Water), DamageType.Water);
            health.TakeDamage(ScaledDamage(20, DamageType.Lightning), DamageType.Lightning);
            StartCoroutine(SelfDestruct(0f));
        }
        else
        {
            StartCoroutine(SelfDestruct(12f));
        }
    }

    private IEnumerator SelfDestruct(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
