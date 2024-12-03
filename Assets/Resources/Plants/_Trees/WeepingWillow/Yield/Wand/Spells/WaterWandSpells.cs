using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class WaterWandSpells : MonoBehaviour, ISpellProvider
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

    private GameObject spell1MuzzleFlashPrefab;
    private GameObject spell1ArrowsPrefab;
    private GameObject spell1ImpactPrefab;
    private GameObject spell1HitboxPrefab;

    private GameObject spell2MuzzleFlashPrefab;
    private GameObject spell2BarragePrefab;
    private GameObject spell2HitboxPrefab;

    void Start()
    {
        spell1MuzzleFlashPrefab = Resources.Load<GameObject>("Plants/_Trees/WeepingWillow/Yield/Wand/Spells/Wand1.1-MuzzleFlash_Water");
        spell1ArrowsPrefab = Resources.Load<GameObject>("Plants/_Trees/WeepingWillow/Yield/Wand/Spells/Wand1.2-Arrow Rain_Water");
        spell1ImpactPrefab = Resources.Load<GameObject>("Plants/_Trees/WeepingWillow/Yield/Wand/Spells/Wand1.3-Spell_WaterImpact");
        spell1HitboxPrefab = Resources.Load<GameObject>("Plants/_Trees/WeepingWillow/Yield/Wand/Spells/ArrowHitbox");

        spell2MuzzleFlashPrefab = Resources.Load<GameObject>("Plants/_Trees/WeepingWillow/Yield/Wand/Spells/Wand2.1-Spell_WaterSweep");
        spell2BarragePrefab = Resources.Load<GameObject>("Plants/_Trees/WeepingWillow/Yield/Wand/Spells/BarrageContainer");
        spell2HitboxPrefab = Resources.Load<GameObject>("Plants/_Trees/WeepingWillow/Yield/Wand/Spells/Wand2.3-Slash_Wave_Water_Cast");

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
        animator.SetTrigger("Spell1");

        PlayerStats.instance.ApplyMultiplicativeSpeedModifier(0.8f, 31f/60f);

        // Wait for wand to flick forward.
        yield return new WaitForSeconds(31f / 60f);

        // Spawn muzzle flash.
        Transform cameraTransform = SceneProperties.cameraTransform;
        Vector3 arrowSpawnPosition = cameraTransform.position + (cameraTransform.forward * 13f) + new Vector3(0.3f, 0.1f, 0f);
        Vector3 arrowRotation = cameraTransform.eulerAngles - new Vector3(90f, 0f, 0f);

        Transform wandTransform = transform.GetChild(0);
        GameObject muzzleFlash = MonoBehaviour.Instantiate(spell1MuzzleFlashPrefab, wandTransform);

        inAnimation = false;

        // Spawn projectiles.
        Vector3 directionToTarget = muzzleFlash.transform.position - arrowSpawnPosition;
        Quaternion rotationTowardsWand = Quaternion.LookRotation(directionToTarget);
        // GameObject arrows = MonoBehaviour.Instantiate(spell1ArrowsPrefab, arrowSpawnPosition, Quaternion.Euler(arrowRotation));
        GameObject arrows = MonoBehaviour.Instantiate(spell1ArrowsPrefab, arrowSpawnPosition, rotationTowardsWand);

        float flightDuration = 0.38f;
        float timer = 0f;

        Vector3 hitboxStart = cameraTransform.position + new Vector3(0.3f, 0.1f, 0f) + (cameraTransform.forward);
        Vector3 hitboxEnd = arrowSpawnPosition + (cameraTransform.forward);

        Vector3 direction = cameraTransform.position - hitboxStart;
        GameObject arrowHitbox = MonoBehaviour.Instantiate(spell1HitboxPrefab, hitboxStart, Quaternion.LookRotation(direction));
        

        arrowHitbox.GetComponent<ArrowHitbox>().arrowParticleSystem = arrows;
        while (timer < flightDuration)
        {
            Vector3 hitboxPosition = Vector3.Lerp(hitboxStart, hitboxEnd, timer / flightDuration);
            arrowHitbox.transform.position = hitboxPosition;
            timer += Time.deltaTime;
            yield return null;
            if (arrowHitbox == null)
            {
                Debug.Log("Arrows collided. Ending coroutine.");
                break;
            }
        }

        Destroy(arrowHitbox);
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

        PlayerStats.instance.ApplyMultiplicativeSpeedModifier(0.8f, 31f/60f);

        // Wait for wand to flick forward.
        yield return new WaitForSeconds(31f / 60f);

        // Spawn muzzle flash.
        Transform wandTransform = transform.GetChild(0);
        GameObject muzzleFlash = MonoBehaviour.Instantiate(spell2MuzzleFlashPrefab, wandTransform);

        Transform cameraTransform = SceneProperties.cameraTransform;

        // Spawn barrage.
        Vector3 flashPosition = muzzleFlash.transform.position;
        GameObject barrage = Instantiate(spell2BarragePrefab, flashPosition + cameraTransform.forward * 0.6f, spell2BarragePrefab.transform.rotation);
        barrage.transform.rotation = Quaternion.LookRotation(cameraTransform.forward);

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
