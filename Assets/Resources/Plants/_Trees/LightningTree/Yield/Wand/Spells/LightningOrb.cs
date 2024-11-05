using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningOrb : MonoBehaviour
{
    public GameObject impactPrefab;
    public float speed = 3f;
    public float duration = 1.4f;

    private Coroutine traversalCoroutine;
    private bool inFlight;

    void Start()
    {
        traversalCoroutine = StartCoroutine(MoveForward());
    }

    private IEnumerator MoveForward()
    {
        inFlight = true;

        float timer = 0f;
        while (timer <= duration)
        {
            transform.position = transform.position + transform.forward * speed * Time.deltaTime;
            timer += Time.deltaTime;

            yield return null;
        }

        Explode();
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject collidedObject = other.gameObject;

        string collidedLayerName = LayerMask.LayerToName(collidedObject.layer);
        if (collidedLayerName != "Player")
        {
            Explode();
            StopCoroutine(traversalCoroutine);
        }
    }

    private void Explode()
    {
        Debug.Log("Explode");
        if (inFlight)
        {
            inFlight = false;
            Instantiate(impactPrefab, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
