using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Choppable : MonoBehaviour, IChoppable
{
    public GameObject stumpPrefab;
    public float durability;
    public GameObject logGroundItemPrefab;
    public int minDroppedLogs = 2;
    public int maxDroppedLogs = 3;

    private Vector3 position;

    void Start()
    {
        // Set the position here since, if it's read in the Chop func, it might take the updated position due to shaking.
        position = transform.position;
    }

    public void Chop(float damage)
    {
        durability -= damage;

        if (durability <= 0)
        {
            Quaternion currentRotation = transform.rotation;
            Instantiate(stumpPrefab, position, currentRotation);
            Destroy(gameObject);

            SpawnLogs();
        }
    }

    private void SpawnLogs()
    {
        float logsToSpawn = Random.Range(minDroppedLogs, maxDroppedLogs + 1);
        for (int i = 0; i < logsToSpawn; i++)
        {
            float randomRotationAngle = Random.Range(0f, 360f);
            Quaternion randomRotation = Quaternion.Euler(0, randomRotationAngle, 0);

            float randomLocationAngle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
            Vector3 spawnOffset = new Vector3(
                Mathf.Cos(randomLocationAngle),
                0,
                Mathf.Sin(randomLocationAngle));
            spawnOffset.Normalize();
            Vector3 spawnPosition = position + spawnOffset;

            Instantiate(logGroundItemPrefab, spawnPosition, randomRotation);
        }
    }
}
