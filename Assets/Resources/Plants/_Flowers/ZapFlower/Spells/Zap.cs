using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zap : MonoBehaviour
{
    public GameObject hitboxPrefab;
    public float hitBoxSpawnTime = 0.9f;
    public float hitBoxDespawnTime = 1.3f;

    void Start()
    {
        StartCoroutine(SpawnZapHitbox());
    }

    private IEnumerator SpawnZapHitbox()
    {
        yield return new WaitForSeconds(hitBoxSpawnTime);

        GameObject zapHitbox = Instantiate(hitboxPrefab, transform);

        yield return new WaitForSeconds(hitBoxDespawnTime - hitBoxSpawnTime);

        Destroy(zapHitbox);
    }
}
