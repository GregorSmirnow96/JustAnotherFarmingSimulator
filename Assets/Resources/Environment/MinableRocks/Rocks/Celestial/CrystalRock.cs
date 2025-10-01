using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CrystalRock : MonoBehaviour, IMinable
{
    public float health = 20;
    public float regenerationTime = 24;
    public GameObject moonstoneOrePrefab;
    public GameObject sunstalOrePrefab;
    public GameObject sparksPrefab;

    private MeshCollider parentCollider;
    private float lastHitOrRegenTime;
    private float initialHealth;
    private float lastTime;
    private Clock clock;
    private bool fullyDegraded => health == 0;
    private int requiredPickAxeTier = 3;
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
    }

    public bool FullyDegraded() => fullyDegraded;

    public void Degrade()
    {
        Vector3 veinPosition = transform.position;
        Vector3 cameraPosition = SceneProperties.cameraTransform.position;
        Vector3 vectorToCamera = cameraPosition - veinPosition;
        Vector3 midwayPoint = transform.position + vectorToCamera * 0.3f;

        GameObject minedOre;
        if (Clock.globalClock.time < 6f || Clock.globalClock.time > 18f)
        {
            minedOre = Instantiate(moonstoneOrePrefab, midwayPoint, Quaternion.identity);
        }
        else
        {
            minedOre = Instantiate(sunstalOrePrefab, midwayPoint, Quaternion.identity);
        }

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
