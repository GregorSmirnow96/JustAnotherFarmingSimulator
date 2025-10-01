using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class OreVein : MonoBehaviour, IMinable
{
    public GameObject richPrefab;
    public float health = 8;
    public float regenerationTime = 24;
    public GameObject orePrefab;
    public GameObject sparksPrefab;
    public int requiredPickAxeTier = 1;

    private MeshCollider parentCollider;
    private float lastHitOrRegenTime;
    private float initialHealth;
    private float lastTime;
    private Clock clock;
    private bool fullyDegraded => health == 0;
    private GameObject damageIndicatorPrefab;

    void Start()
    {
        clock = Clock.globalClock;

        initialHealth = health;
        lastHitOrRegenTime = clock.time;

        lastTime = clock.time;

        parentCollider = GetComponent<MeshCollider>();
        SetColliderMesh();

        damageIndicatorPrefab = AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/CustomUIElements/DamageIndicator.prefab");
    }

    /*
    void Update()
    {
        float timeSinceLastHit = clock.time - lastHitOrRegenTime;
        if (timeSinceLastHit >= regenerationTime)
        {
            Regenerate();
        }
    }
    */

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

    /*
    private void RemoveColliderMesh()
    {
        parentCollider.sharedMesh = null;
    }

    private GameObject GetRockObject()
    {
        Transform childtransform = transform.GetChild(0);
        return childtransform?.gameObject;
    }
    */

    public void Mine(float damage, int pickaxeTier)
    {
        if (pickaxeTier < requiredPickAxeTier)
        {
            damage = 0;
        }

        if (!fullyDegraded)
        {
            lastHitOrRegenTime = clock.time;

            health -= damage;
            SpawnDamageIndicator((int) damage, DamageType.Physical);
            if (health <= 0)
            {
                Degrade();
            }
        }
    }

    private void SpawnDamageIndicator(int damage, string damageType)
    {
        GameObject damageIndicator = Instantiate(damageIndicatorPrefab, SceneProperties.canvasTransform);
        DamageIndicator damageIndicatorScript = damageIndicator.GetComponent<DamageIndicator>();
        
        damageIndicatorScript.damage = damage;
        damageIndicatorScript.damageType = damageType;
        damageIndicatorScript.damagedTransform = transform;
        damageIndicatorScript.transformOffset = new Vector3(0f, 0.9f, 0f);
    }

    public bool FullyDegraded() => fullyDegraded;

    public void Degrade()
    {
        //GameObject previousRock = GetRockObject();
        //Destroy(previousRock);

        //health = 0;

        //RemoveColliderMesh();

        Vector3 veinPosition = transform.position;
        Vector3 cameraPosition = SceneProperties.cameraTransform.position;
        Vector3 vectorToCamera = cameraPosition - veinPosition;
        Vector3 midwayPoint = transform.position + vectorToCamera * 0.3f;

        GameObject minedOre = Instantiate(orePrefab, midwayPoint, Quaternion.identity);
        GameObject oreMesh = minedOre.transform.GetChild(0).gameObject;

        Rigidbody oreRigidbody = oreMesh.GetComponent<Rigidbody>();
        oreRigidbody.AddForce(new Vector3(0, 140, 0));

        Destroy(gameObject);
    }

    public void Regenerate()
    {
    /*
        if (health == 0)
        {
            GameObject newRock = Instantiate(richPrefab, transform);
            SetColliderMesh();
        }

        health = initialHealth;
        lastHitOrRegenTime = clock.time;
    */
    }

    public GameObject GetCollisionParticleSystem()
    {
        return sparksPrefab;
    }
}
