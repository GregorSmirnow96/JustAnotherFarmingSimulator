using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonBreathContainer : MonoBehaviour
{
    public GameObject dragonBreathParticlePrefab;
    public GameObject dragonBreathHitboxPrefab;

    private GameObject hitbox;
    private Transform hitboxTransform;

    void Start()
    {
        hitbox = Instantiate(dragonBreathHitboxPrefab, transform.position, transform.rotation);
        hitboxTransform = hitbox.transform;

        StartCoroutine(SpawnBreath());
    }

    private IEnumerator SpawnBreath()
    {
        const float hitboxTravelTime = 0.45f;
        const float hitboxTravelDistance = 8;

        // Use this object's transform data instead of the transform itself so that the breath doesn't move with the player.
        // If it did, the player could spin while casting to hit everyone around them. CHEATER!
        Instantiate(dragonBreathParticlePrefab, transform.position, transform.rotation);

        yield return null;

        // Move the hitbox forwards by transform.forward * someDistBasedOnDeltaTime
        float hitboxTimer = 0f;
        while (hitboxTimer < hitboxTravelTime)
        {
            hitboxTimer += Time.deltaTime;

            float hitboxProgress = Mathf.Min(Time.deltaTime / hitboxTravelTime, hitboxTravelTime);
            float travelMagnitude = hitboxProgress * hitboxTravelDistance;

            hitboxTransform.position = hitboxTransform.position + transform.forward * travelMagnitude;
            yield return null;
        }

        yield return new WaitForSeconds(0.6f);

        Destroy(hitbox);
        Destroy(gameObject);
    }
}
