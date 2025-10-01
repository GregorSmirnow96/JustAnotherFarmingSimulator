using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButterflySpawnManager : MonoBehaviour
{
    public GameObject redButterflyPrefab;
    public GameObject blueButterflyPrefab;
    public GameObject yellowButterflyPrefab;
    public float minDistanceFromCenter = 40f;
    public float maxDistanceFromCenter = 100f;
    public float minVerticalOffset = 6.2f;
    public float maxVerticalOffset = 7.3f;
    public float spawnInterval = 1f;
    public float butterflyDuration = 180f;

    private float lastSpawnTime;

    void Update()
    {
        if (Clock.globalClock.time - lastSpawnTime >= spawnInterval)
        {
            lastSpawnTime = Clock.globalClock.time;
            StartCoroutine(SpawnButterflyGroup());
        }
    }

    private IEnumerator SpawnButterflyGroup()
    {
        const int maxAttempts = 5;

        GameObject butterflyGroup = null;
        for (int attemptCount = 0; attemptCount < maxAttempts; attemptCount++)
        {
            float roll = Random.Range(0f, 1f);
            GameObject butterflyGroupPrefab = roll <= 0.333f
                ? redButterflyPrefab
                : roll <= 0.666f
                    ? blueButterflyPrefab
                    : yellowButterflyPrefab;

            Vector3 position = GenerateCartesianButterflyCoordinate();

            SphereCollider sphereCollider = butterflyGroupPrefab.GetComponent<SphereCollider>();
            float radius = sphereCollider.radius * butterflyGroupPrefab.transform.localScale.x;

            int layerMask = ~0;
            QueryTriggerInteraction triggerInteraction = QueryTriggerInteraction.Ignore;
            bool canSpawn = !Physics.CheckSphere(position, radius, layerMask, triggerInteraction);

            if (canSpawn)
            {
                butterflyGroup = Instantiate(
                    butterflyGroupPrefab,
                    position,
                    Quaternion.identity);

                break;
            }
        }

        if (butterflyGroup != null)
        {
            yield return new WaitForSeconds(butterflyDuration);

            Destroy(butterflyGroup);
        }
    }

    private Vector3 GenerateCartesianButterflyCoordinate()
    {
        // random angle in degrees
        float angleDeg = Random.Range(0f, 360f);
        // convert to radians
        float angleRad = angleDeg * Mathf.Deg2Rad;

        // random distance between min and max
        float distance = Random.Range(minDistanceFromCenter, maxDistanceFromCenter);

        // polar → cartesian
        float x = SceneProperties.sceneCenter.x + distance * Mathf.Cos(angleRad);
        float z = SceneProperties.sceneCenter.z + distance * Mathf.Sin(angleRad);

        Vector2 butterflyPosition2D = new Vector2(x, z);
        float verticalOffset = Random.Range(minVerticalOffset, maxVerticalOffset);
        float y = SceneProperties.TerrainHeightAtPosition(butterflyPosition2D) + verticalOffset;

        return new Vector3(x, y, z);
    }
}
