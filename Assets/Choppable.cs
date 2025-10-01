using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Choppable : MonoBehaviour, IChoppable
{
    public GameObject stumpPrefab;
    public float durability = 10;
    public GameObject logGroundItemPrefab;
    public int minDroppedLogs = 2;
    public int maxDroppedLogs = 3;
    public int requiredAxeTier = 1;

    private Vector3 position;
    private GameObject damageIndicatorPrefab;

    void Start()
    {
        // Set the position here since, if it's read in the Chop func, it might take the updated position due to shaking.
        position = transform.position;
        damageIndicatorPrefab = AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/CustomUIElements/DamageIndicator.prefab");
    }

    public void Chop(float damage, int axeTier)
    {
        if (axeTier < requiredAxeTier)
        {
            damage = 0;
        }

        durability -= damage;
        SpawnDamageIndicator((int) damage, DamageType.Physical);

        if (durability <= 0)
        {
            if (stumpPrefab != null)
            {
                Quaternion currentRotation = transform.rotation;
                Instantiate(stumpPrefab, position, currentRotation);
            }

            if (transform.parent.GetComponent<PlantStageGrowth>() == null)
            {
                Destroy(gameObject);
            }
            else
            {
                Destroy(transform.parent.gameObject);
            }

            SpawnLogs();
        }
    }

    private void SpawnDamageIndicator(int damage, string damageType)
    {
        GameObject damageIndicator = Instantiate(damageIndicatorPrefab, SceneProperties.canvasTransform);
        DamageIndicator damageIndicatorScript = damageIndicator.GetComponent<DamageIndicator>();
        
        damageIndicatorScript.damage = damage;
        damageIndicatorScript.damageType = damageType;
        damageIndicatorScript.damagedTransform = transform;
        damageIndicatorScript.transformOffset = new Vector3(0f, 1.2f, 0f);
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
