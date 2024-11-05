using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrage : MonoBehaviour
{
    public GameObject explosionPrefab;
    public float maxDuration = 3f;
    public int damage = 8;
    public float slowPercentage = 70f;
    public float slowDuration = 4f;

    private float speed = 1f;
    private float flightTimer = 0f;

    private GameObject barrageSystem;

    void Start()
    {
        barrageSystem = transform.GetChild(0).gameObject;
    }

    void Update()
    {
        flightTimer += Time.deltaTime;
        if (flightTimer >= maxDuration)
        {
            Destroy(gameObject);
        }
        else
        {
            transform.position = transform.position + (speed * transform.forward);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        GameObject collidedObject = other.gameObject;

        string collidedLayerName = LayerMask.LayerToName(collidedObject.layer);
        if (collidedLayerName != "Player")
        {
            Debug.Log($"Barrage collided with: {collidedObject.name}");

            GameObject claw = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Transform cameraTransform = SceneProperties.cameraTransform;
            claw.transform.rotation = Quaternion.LookRotation(cameraTransform.forward);

            Health health = collidedObject.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }

            ICCable ccable = collidedObject.GetComponent<ICCable>();
            if (ccable != null)
            {
                ccable.Slow(slowDuration, slowPercentage);
            }

            Destroy(gameObject);
        }
    }
}
