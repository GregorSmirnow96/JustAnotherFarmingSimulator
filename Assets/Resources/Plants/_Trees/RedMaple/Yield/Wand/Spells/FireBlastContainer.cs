using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBlastContainer : MonoBehaviour
{
    public GameObject fireBlastParticlePrefab;
    public GameObject fireBlastHitboxPrefab;
    public float hitboxDuration = 0.2f;
    public float hitboxDelay = 1f;


    void Start()
    {
        StartCoroutine(SpawnBlast());
    }

    private IEnumerator SpawnBlast()
    {
        yield return new WaitForSeconds(hitboxDelay);

        GameObject hitbox = Instantiate(fireBlastHitboxPrefab, transform);

        yield return new WaitForSeconds(hitboxDuration);

        Destroy(hitbox);

        yield return new WaitForSeconds(2f);
        
        Destroy(gameObject);
    }
}
