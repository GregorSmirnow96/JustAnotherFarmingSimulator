using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSlashHitbox : MonoBehaviour
{
    public int damage = 12;
    public int rotationDurationInFrames = 14;
    public int selfDeleteFrame = 15;
    public float endRotation = 100f;

    private int scaledDamage => PlayerProperties.GetScaledPlayerDamage(damage, DamageType.Water);

    private float startRotation;
    private List<Health> damagedHealthScripts;

    void Start()
    {
        startRotation = transform.localEulerAngles.y;
        damagedHealthScripts = new List<Health>();

        StartCoroutine(RotateHitbox());
    }

    private IEnumerator RotateHitbox()
    {
        float rotationDuration = rotationDurationInFrames / 60f;
        float rotationTimer = 0;
        while (rotationTimer <= rotationDuration)
        {
            float rotationProgress = rotationTimer / rotationDuration;
            float localRotationAngle = (endRotation - startRotation) * rotationProgress;

            transform.parent.localEulerAngles = new Vector3(0f, localRotationAngle, 0f);

            yield return null;

            rotationTimer += Time.deltaTime;
        }

        float postRotationDeleteDelay = (selfDeleteFrame - rotationDurationInFrames) / selfDeleteFrame;
        yield return new WaitForSeconds(postRotationDeleteDelay);

        Destroy(transform.parent.gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        GameObject collidedObject = other.gameObject;

        string collidedLayerName = LayerMask.LayerToName(collidedObject.layer);
        if (collidedLayerName != "Player")
        {
            Health health = collidedObject.GetComponent<Health>();
            if (health != null && !damagedHealthScripts.Contains(health))
            {
                damagedHealthScripts.Add(health);
                health.TakeDamage(scaledDamage, DamageType.Water);
            }
        }
    }
}
