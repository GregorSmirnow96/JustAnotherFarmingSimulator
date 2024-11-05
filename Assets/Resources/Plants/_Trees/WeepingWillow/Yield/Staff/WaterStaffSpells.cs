using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaterStaffSpells : MonoBehaviour, ISpellProvider
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

    private GameObject spell1ParticlePrefab;
    private GameObject spell1HitboxPrefab;

    private GameObject spell2OrbPrefab;
    private GameObject spell2DropletContainerPrefab;
    private GameObject spell2StaffEffectPrefab;

    void Start()
    {
        spell1ParticlePrefab = Resources.Load<GameObject>("Plants/_Trees/WeepingWillow/Yield/Staff/Spells/Staff1-ElementalSlash_Water");
        spell1HitboxPrefab = Resources.Load<GameObject>("Plants/_Trees/WeepingWillow/Yield/Staff/Spells/ElementalSlashHitbox");

        spell2OrbPrefab = Resources.Load<GameObject>("Plants/_Trees/WeepingWillow/Yield/Staff/Spells/Staff2.1-Orb_Water");
        spell2DropletContainerPrefab = Resources.Load<GameObject>("Plants/_Trees/WeepingWillow/Yield/Staff/Spells/WateringSphereContainer");
        spell2StaffEffectPrefab = Resources.Load<GameObject>("Plants/_Trees/WeepingWillow/Yield/Staff/Spells/Staff2.3-VFX_WaterGuard");
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

        yield return new WaitForSeconds(0.38f); // Hardcode this for now. 

        // Create spell particle system
        Transform playerTransform = SceneProperties.playerTransform;
        Transform cameraTransform = SceneProperties.cameraTransform;

        Vector3 spawnPosition = playerTransform.position + cameraTransform.up * 0.4f;

        Vector3 forwardDirection = cameraTransform.forward; // new Vector3(cameraTransform.forward.x, 0, cameraTransform.forward.z).normalized;
        Quaternion spawnRotation = Quaternion.LookRotation(forwardDirection);
        Quaternion extraRotation = Quaternion.Euler(0f, 137f, 0f);
        Quaternion finalRotation = spawnRotation * extraRotation;

        MonoBehaviour.Instantiate(spell1ParticlePrefab, spawnPosition, finalRotation);

        // Create spell hitbox in parallel with particle system
        float initialYRotation = 123f;
        float finalYRotation = 285f;
        float spawnTime = 0.02f;
        float despawnTime = 0.18f;
        Quaternion extraHitBoxRotation = Quaternion.Euler(0f, initialYRotation, 0f);
        Quaternion finalHitBoxRotation = spawnRotation * extraHitBoxRotation;
        yield return new WaitForSeconds(spawnTime);
        GameObject hitbox = MonoBehaviour.Instantiate(spell1HitboxPrefab, spawnPosition, finalHitBoxRotation);

        float startTime = Time.time;
        float hitBoxDuration = despawnTime - spawnTime;
        float endTime = Time.time + hitBoxDuration;
        while (Time.time <= endTime)
        {
            yield return null;
            // Instantiate(hitbox, hitbox.transform.position, hitbox.transform.rotation);
            float durationDelta = Time.deltaTime / hitBoxDuration;
            float angleDelta = Mathf.Lerp(initialYRotation, finalYRotation, durationDelta);
            hitbox.transform.RotateAround(playerTransform.position, cameraTransform.up, angleDelta);
            // PICK UP HERE!!!! The hitbox seems to work! test it with the mesh renderer removed :)
            // Add on hit logic with a script attached to the hitbox prefab. It can keep track of what it hits,
            // and make sure each health script can only be damgaged once. Once that's done, plants!!
        }

        inAnimation = false;

        animator.SetTrigger("Idle");
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
        const float maxCastRange = 6f;
        const float heightOffGround = 4f;

        animator.SetTrigger("Spell2");

        // Spawn the particle system inside the staff's prongs.
        Transform staffTransform = transform.GetChild(0);
        GameObject staffEffect = MonoBehaviour.Instantiate(spell2StaffEffectPrefab, staffTransform);
        staffEffect.transform.localPosition = new Vector3(0f, 1.66f, 0f);
        
        // Calculate where to spawn the orb + side-effects.
        Vector3 intersection = SceneProperties.GetViewIntersectionWithTerrain(maxCastRange);
        Vector3 orbLocation = intersection + Vector3.up * heightOffGround;
        GameObject orb = MonoBehaviour.Instantiate(spell2OrbPrefab, orbLocation, Quaternion.identity);
        
        //WateringSphereContainer sphereContainerScript = orb.GetComponent<WateringSphereContainer>();
        GameObject wateringSphereContainer = MonoBehaviour.Instantiate(spell2DropletContainerPrefab, intersection, Quaternion.identity);
        WateringSphereContainer sphereContainerScript = orb.GetComponent<WateringSphereContainer>();

        yield return new WaitForSeconds(0.6f);

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
