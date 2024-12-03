using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class WaterStaffSpells : MonoBehaviour, ISpellProvider
{
    public float spell1Cooldown = 4f;
    public float spell2Cooldown = 4f;
    public float spell3Cooldown = 4f;
    public float spell4Cooldown = 4f;
    public Image spell1Sprite;
    public Image spell2Sprite;

    private static float lastSpell1CastTime = Int32.MinValue;
    private static float lastSpell2CastTime = Int32.MinValue;
    private static float lastSpell3CastTime = Int32.MinValue;
    private static float lastSpell4CastTime = Int32.MinValue;

    private Animator animator;
    private bool inAnimation;

    public GameObject spell1ParticlePrefab;
    //private GameObject spell1HitboxPrefab;

    private GameObject spell2OrbPrefab;
    private GameObject spell2DropletContainerPrefab;
    private GameObject spell2StaffEffectPrefab;

    void Start()
    {
        spell2OrbPrefab = Resources.Load<GameObject>("Plants/_Trees/WeepingWillow/Yield/Staff/Spells/Staff2.1-Orb_Water");
        spell2DropletContainerPrefab = Resources.Load<GameObject>("Plants/_Trees/WeepingWillow/Yield/Staff/Spells/WateringSphereContainer");
        spell2StaffEffectPrefab = Resources.Load<GameObject>("Plants/_Trees/WeepingWillow/Yield/Staff/Spells/Staff2.3-VFX_WaterGuard");
        animator = GetComponent<Animator>();
    }

    public int GetSpellCount()
    {
        return 2;
    }

    public List<Image> GetSpellSprites()
    {
        return new List<Image>()
        {
            spell1Sprite,
            spell2Sprite
        };
    }

    public List<float> GetSpellCooldowns()
    {
        return new List<float>()
        {
            spell1Cooldown,
            spell2Cooldown,
            spell3Cooldown,
            spell4Cooldown
        }.Select(cd => cd * PlayerProperties.GetCDRMulti()).ToList();
    }

    public List<float> GetRemainingSpellCooldowns()
    {
        List<float> cooldowns = GetSpellCooldowns();
        float remainingSpell1Cooldown = cooldowns.ElementAt(0) - (Time.time - lastSpell1CastTime);
        float remainingSpell2Cooldown = cooldowns.ElementAt(1) - (Time.time - lastSpell2CastTime);
        float remainingSpell3Cooldown = cooldowns.ElementAt(2) - (Time.time - lastSpell3CastTime);
        float remainingSpell4Cooldown = cooldowns.ElementAt(3) - (Time.time - lastSpell4CastTime);

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
        float spell1Cd = GetSpellCooldowns().ElementAt(0);
        if (timeSinceLastCast >= spell1Cd && !inAnimation)
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

        PlayerStats.instance.ApplyMultiplicativeSpeedModifier(0.6f, 31f/60f);

        yield return new WaitForSeconds(0.38f); // Hardcode this for now. 

        // Create spell particle system
        Transform playerTransform = SceneProperties.playerTransform;
        Transform cameraTransform = SceneProperties.cameraTransform;

        Vector3 spawnPosition = playerTransform.position + cameraTransform.up * 0.4f;

        Vector3 forwardDirection = cameraTransform.forward;
        Quaternion spawnRotation = Quaternion.LookRotation(forwardDirection);
        Quaternion extraRotation = Quaternion.Euler(0f, 137f, 0f);
        Quaternion finalRotation = spawnRotation * extraRotation;

        MonoBehaviour.Instantiate(spell1ParticlePrefab, spawnPosition, finalRotation);

        inAnimation = false;

        animator.SetTrigger("Idle");
    }

    public void CastSpell2()
    {
        float timeSinceLastCast = Time.time - lastSpell2CastTime;
        float spell2Cd = GetSpellCooldowns().ElementAt(1);
        if (timeSinceLastCast >= spell2Cd && !inAnimation)
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

        PlayerStats.instance.ApplyMultiplicativeSpeedModifier(0.2f, 48f/60f);

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
