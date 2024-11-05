using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireOrbContainer : MonoBehaviour
{
    public GameObject orbPrefab;
    public float orbRotationSpeed = 120f;
    public float orbDuration = 10f;
    public float orbDistanceFromPlayer = 5f;

    void Start()
    {
        StartCoroutine(SpawnOrbs());
    }

    private IEnumerator SpawnOrbs()
    {
        Vector3 positionAbovePlayerFeet = transform.position + Vector3.up * 0.5f;
        
        Transform playerTransform = SceneProperties.playerTransform;

        Vector3 orb1Position = positionAbovePlayerFeet + playerTransform.forward * orbDistanceFromPlayer;
        GameObject orb1 = Instantiate(orbPrefab, orb1Position, Quaternion.identity);

        Vector3 orb2Position = positionAbovePlayerFeet - playerTransform.forward * orbDistanceFromPlayer;
        GameObject orb2 = Instantiate(orbPrefab, orb2Position, Quaternion.identity);

        float orbTimer = 0f;
        while (orbTimer < orbDuration)
        {
            yield return null;
            orbTimer += Time.deltaTime;

            // Rotate orbs around transform.position (player position)
            orb1.transform.RotateAround(playerTransform.position, Vector3.up, orbRotationSpeed * Time.deltaTime);
            orb2.transform.RotateAround(playerTransform.position, Vector3.up, orbRotationSpeed * Time.deltaTime);
        }

        Destroy(orb1);
        Destroy(orb2);
        Destroy(gameObject);
    }
}
