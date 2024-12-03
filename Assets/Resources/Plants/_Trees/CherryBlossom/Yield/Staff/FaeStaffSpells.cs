using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class FaeStaffSpells : MonoBehaviour, ISpellProvider
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

    public GameObject spell1CharmOrbPrefab;

    public GameObject spell2OrbPrefab;

    void Start()
    {
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

        PlayerStats.instance.ApplyMultiplicativeSpeedModifier(0.1f, 1f);

        // Wait for {spell_particle_spawn_frame} / {fps} (60).
        yield return new WaitForSeconds(33f/60f);

        Transform playerTransform = SceneProperties.playerTransform;
        Vector3 startPosition = playerTransform.position + playerTransform.forward * 2f;
        startPosition.y = SceneProperties.TerrainHeightAtPosition(startPosition);
        Instantiate(spell1CharmOrbPrefab, startPosition, Quaternion.identity);

        inAnimation = false;
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
        animator.SetTrigger("Spell2");

        PlayerStats.instance.ApplyMultiplicativeSpeedModifier(0.5f, 1f);

        // Wait for {spell_particle_spawn_frame} / {fps} (60).
        yield return new WaitForSeconds(34f/60f);

        Transform cameraTransform = SceneProperties.cameraTransform;
        Vector3 startPosition = cameraTransform.position
            + cameraTransform.up * 0.377f
            + cameraTransform.right * -0.363f
            + cameraTransform.forward * 1.0581f;
        Instantiate(spell2OrbPrefab, startPosition, cameraTransform.rotation);

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
