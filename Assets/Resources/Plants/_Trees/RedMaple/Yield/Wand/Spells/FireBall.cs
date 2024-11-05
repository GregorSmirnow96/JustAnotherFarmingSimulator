using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    public GameObject muzzleFlashPrefab;
    public GameObject impactPrefab;
    public float speed = 20f;
    public float maxDuration = 2f;

    private float lifespanTimer = 0f;

    void Start()
    {
        Instantiate(muzzleFlashPrefab, transform.position, transform.rotation);
    }

    void Update()
    {
        transform.position = transform.position + transform.forward * speed * Time.deltaTime;

        lifespanTimer += Time.deltaTime;
        if (lifespanTimer > maxDuration)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        GameObject collidedObject = other.gameObject;

        string collidedLayerName = LayerMask.LayerToName(collidedObject.layer);
        if (collidedLayerName != "Player")
        {
            Vector3 collisionPoint = other.ClosestPoint(transform.position);
            Instantiate(impactPrefab, collisionPoint, Quaternion.identity);

            Destroy(gameObject);
        }
    }
}
