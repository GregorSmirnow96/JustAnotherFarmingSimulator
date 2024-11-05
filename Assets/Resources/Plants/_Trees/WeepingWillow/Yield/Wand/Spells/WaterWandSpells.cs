using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaterWandSpells : MonoBehaviour, ISpellProvider
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
        animator.SetTrigger("Spell1");

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
