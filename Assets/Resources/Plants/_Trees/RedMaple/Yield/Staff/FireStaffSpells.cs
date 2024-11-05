using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FireStaffSpells : MonoBehaviour, ISpellProvider
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

    public GameObject spell1FlameContainerPrefab;

    public GameObject spell2OrbContainerPrefab;

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

        // I know 40/60 = 2/3. 40 represents the frame of the 60 fps animation that corresponds to the fire spawning.
        yield return new WaitForSeconds(40f/60f);

        Transform cameraTransform = SceneProperties.cameraTransform;
        Vector3 breathPosition = cameraTransform.position + cameraTransform.up * -0.1f + SceneProperties.cameraTransform.forward * 0.4f;
        Instantiate(spell1FlameContainerPrefab, breathPosition, cameraTransform.rotation);

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
        const float maxCastRange = 6f;
        const float heightOffGround = 4f;

        animator.SetTrigger("Spell2");

        yield return new WaitForSeconds(0.6f);

        inAnimation = false;

        Transform playerTransform = SceneProperties.playerTransform;
        Instantiate(spell2OrbContainerPrefab, playerTransform);
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
