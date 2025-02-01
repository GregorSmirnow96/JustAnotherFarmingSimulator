using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarArrow : MonoBehaviour, IArrow
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

    private void OnTriggerEnter(Collider other)
    {
        if (!wasFired) return;

        StopCoroutine(flightCoroutine);

        Instantiate(impactParticleSystem, transform.position, Quaternion.identity);

        GameObject collidedObject = other.gameObject;
        Health health = collidedObject.GetComponent<Health>();
        if (health != null)
        {
            collidedObject.AddComponent<SolarDot>();
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
