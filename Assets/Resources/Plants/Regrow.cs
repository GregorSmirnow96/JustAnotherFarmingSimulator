using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Regrow : MonoBehaviour
{
    public GameObject harvestablePrefab;
    public float regrowTime;

    private float startTime;

    void Start()
    {
        startTime = Time.time;
    }

    void Update()
    {
        if (Time.time >= startTime + regrowTime)
        {
            Instantiate(harvestablePrefab, transform.position, transform.rotation);

            Destroy(gameObject);
        }
    }
}
