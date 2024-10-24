using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreRock : MonoBehaviour, IMinable
{
    public GameObject richPrefab;
    public GameObject minedPrefab;
    public float health = 10;
    public float regenerationTime = 24;
    public GameObject orePrefab;
    public GameObject sparksPrefab;

    private MeshCollider parentCollider;
    private float lastHitOrRegenTime;
    private float initialHealth;
    private float lastTime;
    private Clock clock;
    private bool fullyDegraded => health == 0;

    void Start()
    {
        clock = Clock.globalClock;

        initialHealth = health;
        lastHitOrRegenTime = clock.time;

        lastTime = clock.time;

        parentCollider = GetComponent<MeshCollider>();
        SetColliderMesh();
    }

    void Update()
    {
        float timeSinceLastHit = clock.time - lastHitOrRegenTime;
        if (timeSinceLastHit >= regenerationTime)
        {
            Regenerate();
        }
    }

    private void SetColliderMesh()
    {
        // Assign the child mesh to the vein container's mesh collider
        MeshFilter childMeshFilter = GetComponentInChildren<MeshFilter>();
        if (parentCollider != null && childMeshFilter != null)
        {
            parentCollider.sharedMesh = childMeshFilter.mesh;
        }
        else
        {
            Debug.Log($"The ore vein's MeshCollider or the child's MeshFilter was not found!");
        }
    }

    private GameObject GetRockObject()
    {
        Transform childtransform = transform.GetChild(0);
        return childtransform?.gameObject;
    }

    public void Mine(float damage)
    {
        if (!fullyDegraded)
        {
            lastHitOrRegenTime = clock.time;

            health -= damage;
            if (health <= 0)
            {
                Degrade();
            }
        }
    }

    public bool FullyDegraded() => fullyDegraded;

    public void Degrade()
    {
        GameObject previousRock = GetRockObject();
        Destroy(previousRock);
        GameObject newRock = Instantiate(minedPrefab, transform);

        health = 0;

        Vector3 veinPosition = transform.position;
        Vector3 cameraPosition = SceneProperties.cameraTransform.position;
        Vector3 vectorToCamera = cameraPosition - veinPosition;
        Vector3 midwayPoint = transform.position + vectorToCamera * 0.5f;
        midwayPoint = midwayPoint + new Vector3(0f, 0.3f, 0f); // Shift it up a little

        GameObject minedOre = Instantiate(orePrefab, midwayPoint, Quaternion.identity);
        GameObject oreMesh = minedOre.transform.GetChild(0).gameObject;

        Rigidbody oreRigidbody = oreMesh.GetComponent<Rigidbody>();
        oreRigidbody.AddForce(new Vector3(0, 140, 0));
    }

    public void Regenerate()
    {
        GameObject previousRock = GetRockObject();
        Destroy(previousRock);
        GameObject newRock = Instantiate(richPrefab, transform);

        health = initialHealth;
        lastHitOrRegenTime = clock.time;
    }

    public GameObject GetCollisionParticleSystem()
    {
        return sparksPrefab;
    }
}
