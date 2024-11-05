using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FaeWandSpells : MonoBehaviour, ISpellProvider
{
    public float spell1Cooldown = 4f;
    public float spell2Cooldown = 4f;
    public float spell3Cooldown = 4f;
    public float spell4Cooldown = 4f;

    private float lastSpell1CastTime;
    private float lastSpell2CastTime;
    private float lastSpell3CastTime;
    private float lastSpell4CastTime;

    private Animator animator;
    private bool inAnimation;

    public GameObject spell1LightPrefab;

    public GameObject spell2MuzzleFlashPrefab;
    public GameObject spell2ExplosionPrefab;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public int GetSpellCount()
    {
        return 2;
    }

    public List<float> GetSpellCooldowns()
    {
        return new List<float>()
        {
            spell1Cooldown,
            spell2Cooldown,
            spell3Cooldown,
            spell4Cooldown
        };
    }

    public List<float> GetRemainingSpellCooldowns()
    {
        float remainingSpell1Cooldown = spell1Cooldown - (Time.time - lastSpell1CastTime);
        float remainingSpell2Cooldown = spell2Cooldown - (Time.time - lastSpell2CastTime);
        float remainingSpell3Cooldown = spell3Cooldown - (Time.time - lastSpell3CastTime);
        float remainingSpell4Cooldown = spell4Cooldown - (Time.time - lastSpell4CastTime);

        return new List<float>()
        {
            remainingSpell1Cooldown,
            remainingSpell2Cooldown,
            remainingSpell3Cooldown,
            remainingSpell4Cooldown
        };
    }

    public List<bool> GetSpellCastabilities()
    {
        // Determine whether or not each spell can be cast based on costs.
        // If they can't be cast for whatever reason (like a traditional mana cost) return false.
        return new List<bool>()
        {
            true,
            true,
            true,
            true
        };
    }

    public void CastSpell1()
    {
        float timeSinceLastCast = Time.time - lastSpell1CastTime;
        if (timeSinceLastCast >= spell1Cooldown && !inAnimation)
        {
            inAnimation = true;
            lastSpell1CastTime = Time.time;
            StartCoroutine(Spell1Coroutine());
        }
    }

    private IEnumerator Spell1Coroutine()
    {
        // Perform staff animation
        animator.SetTrigger("Spell1");

        // Wait for {spell_particle_spawn_frame} / {fps} (60).
        yield return new WaitForSeconds(23f/60f);

        Transform cameraTransform = SceneProperties.cameraTransform;
        Vector3 startPosition = cameraTransform.position
            + cameraTransform.up * 0.282f
            + cameraTransform.right * 0.196f
            + cameraTransform.forward * 0.92f;
        GameObject fairyLightPS = Instantiate(spell1LightPrefab, startPosition, Quaternion.identity);

        FairyLight fairyLight = fairyLightPS.GetComponent<FairyLight>();
        float initialMaxSpeed = fairyLight.maxSpeed;
        fairyLight.maxSpeed = 0f;
        yield return new WaitForSeconds(0.8f);
        fairyLight.maxSpeed = initialMaxSpeed;

        inAnimation = false;
    }

    public void CastSpell2()
    {
        float timeSinceLastCast = Time.time - lastSpell2CastTime;
        if (timeSinceLastCast >= spell2Cooldown && !inAnimation)
        {
            inAnimation = true;
            lastSpell2CastTime = Time.time;
            StartCoroutine(Spell2Coroutine());
        }
    }

    private IEnumerator Spell2Coroutine()
    {
        animator.SetTrigger("Spell2");

        // Wait for {spell_particle_spawn_frame} / {fps} (60).
        yield return new WaitForSeconds(37f/60f);

        // Spawn muzzle flash.
        Transform cameraTransform = SceneProperties.cameraTransform;
        Vector3 startPosition = cameraTransform.position
            + cameraTransform.up * -0.225f
            + cameraTransform.right * 0.153f
            + cameraTransform.forward * 1.568f;
        Instantiate(spell2MuzzleFlashPrefab, startPosition, cameraTransform.rotation);
        
        // Spawn the explosion if the player is looking at terrain.
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);

        RaycastHit hit;
        int terrainLayer = LayerMask.NameToLayer("Terrain");
        Vector3 hitPoint;
        if (Physics.Raycast(ray, out hit, terrainLayer))
        {
            GameObject hitObject = hit.collider.gameObject;
            hitPoint = hit.point;
        }
        else
        {
            Vector2 cameraDirectionXZ = cameraTransform.forward.ToXZ();
            Vector2 cameraDirectionXZScaled = cameraDirectionXZ.normalized * 8;
            Vector2 hitPointXZ = cameraTransform.position.ToXZ() + cameraDirectionXZScaled;
            float hitPointY = SceneProperties.TerrainHeightAtPosition(hitPointXZ);
            hitPoint = new Vector3(hitPointXZ.x, hitPointY, hitPointXZ.y);
        }
        
        Instantiate(spell2ExplosionPrefab, hitPoint, Quaternion.identity);

        inAnimation = false;
    }

    public void CastSpell3()
    {
        Debug.Log("No spell 3 exists for WaterStaff.");
    }

    public void CastSpell4()
    {
        Debug.Log("No spell 4 exists for WaterStaff.");
    }
}
