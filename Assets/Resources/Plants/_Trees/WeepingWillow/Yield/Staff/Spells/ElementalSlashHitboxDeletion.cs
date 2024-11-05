using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementalSlashHitboxDeletion : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(SelfDestruct());
    }

    private IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(0.205f);

        Destroy(gameObject);
    }
}
