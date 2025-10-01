using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InFlightBomb : MonoBehaviour
{
    public GameObject explosionPrefab;
    public Rigidbody bombRigidbody;
    public float fuseTime = 3f;

    private bool hasBeenThrown;

    public void Start()
    {
        StartCoroutine(Throw());
    }

    private IEnumerator Throw()
    {
        const float forwardForce = 600f;
        const float upwardForce = 200f;

        transform.parent = null;

        Transform playerTransform = SceneProperties.cameraTransform;
        bombRigidbody.AddForce(playerTransform.forward * forwardForce + playerTransform.up * upwardForce);

        bombRigidbody.useGravity = true;

        float fuseTimer = 0f;
        while (fuseTimer < fuseTime)
        {
            yield return null;
            fuseTimer += Time.deltaTime;
        }

        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
