using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFlowerOrb : MonoBehaviour
{
    public GameObject impactPrefab;
    public float growthDuration = 1f;
    public float startSize = 0.2f;
    public float fadeOutDuration = 0.6f;

    private Vector3 initialScale;

    void Start()
    {
        initialScale = transform.localScale;
        transform.localScale = initialScale * startSize;
        foreach (Transform childTransform in transform)
        {
            childTransform.localScale = initialScale * startSize;
        }

        StartCoroutine(GrowOrb());
    }

    private IEnumerator GrowOrb()
    {
        Vector3 lowerScale = initialScale * startSize;

        List<Transform> transforms = new List<Transform>();
        transforms.Add(transform);
        foreach (Transform psTransform in transform)
        {
            transforms.Add(psTransform);
        }

        float growthTimer = 0f;
        Vector3 growthDelta = initialScale - lowerScale;
        while (growthTimer <= growthDuration)
        {
            foreach (Transform psTransform in transforms)
            {
                float sizeScale = growthTimer / growthDuration;
                psTransform.localScale = lowerScale + sizeScale * growthDelta;

                yield return null;

                growthTimer += Time.deltaTime;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Vector3 contactPosition = transform.position;
        contactPosition.y = SceneProperties.TerrainHeightAtPosition(contactPosition);
        Instantiate(impactPrefab, contactPosition, Quaternion.identity);
        StartCoroutine(DespawnOrb());
    }

    private IEnumerator DespawnOrb()
    {
        List<Transform> transforms = new List<Transform>();
        transforms.Add(transform);
        foreach (Transform psTransform in transform)
        {
            transforms.Add(psTransform);
        }

        float fadeOutTimer = 0f;
        while (fadeOutTimer <= fadeOutDuration)
        {
            foreach (Transform psTransform in transforms)
            {
                float sizeScale = 1 - fadeOutTimer / fadeOutDuration;
                psTransform.localScale = sizeScale * initialScale;

                yield return null;

                fadeOutTimer += Time.deltaTime;
            }
        }

        Destroy(gameObject);
    }
}
