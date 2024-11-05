using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WateringSphereContainer : MonoBehaviour
{
    public GameObject waterDropletPrefab;
    public GameObject dropletHitboxPrefab;

    void Start()
    {
        StartCoroutine(CreateWaterDroplets());
    }

    private IEnumerator CreateWaterDroplets()
    {
        const float dropletRepeatCount = 20;

        Vector3 intersection = transform.position;
        GameObject preHitbox = MonoBehaviour.Instantiate(dropletHitboxPrefab, intersection, Quaternion.identity);

        yield return new WaitForSeconds(0.6f);
        
        for (int repeatCounter = 0; repeatCounter < dropletRepeatCount; repeatCounter++)
        {
            MonoBehaviour.Instantiate(waterDropletPrefab, intersection, Quaternion.identity);
            yield return new WaitForSeconds(0.2f);
        }
        // Spawn a second watering hitbox to make sure any plants that are unwatered during the last splash get their water status updated aswell.
        GameObject postHitbox = MonoBehaviour.Instantiate(dropletHitboxPrefab, intersection, Quaternion.identity);

        // Cleanup the hitboxes.        
        Destroy(preHitbox);
        Destroy(postHitbox);
        Destroy(gameObject);
    }
}
