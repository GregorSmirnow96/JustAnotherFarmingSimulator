using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerStats : MonoBehaviour, IHasResistances
{
    public static PlayerStats instance;
    
    public Health health;

    #region "Movement Speed Properties"
    private List<float> addativeMovementSpeedMods = new List<float>();
    private List<float> multiplicativeMovementSpeedMods = new List<float>();
    private List<Action<float>> movementSpeedChangeCallbacks = new List<Action<float>>();
    private float baseMovementSpeed = 5f;
    #endregion

    #region "Cooldown Reduction Properties"
    private List<int> cooldownReductionMods = new List<int>();
    #endregion

    #region "Damage Properties"
    private BuffIndex damageBuffs;
    #endregion

    #region "Resistence Properties"
    private BuffIndex resistanceBuffs;
    #endregion

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        NotifyMovementSpeedChanged();

        health = GetComponent<Health>();

        damageBuffs = new BuffIndex();
        resistanceBuffs = new BuffIndex();

        // DELETE THIS. It was added to test buffs from 2 sources, but I want
        //      to keep it for now as a reminder that not all buffs have timers.
        //      The buffs from equipments must apply as long as the palyer is
        //      wearing them. Refactor this.
        //AddDamageBuff(BuffType.Water, BuffSource.Amulet,  0.15f, 10000f);
    }

    void _Update()
    {
        string info = "CDR Buff:\n";
        float score = cooldownReductionMods.Sum();
        float multi = PlayerProperties.GetCDRMulti();
        info += $"Score: {score} - Multi: {multi}";
        Debug.Log(info);
    }

    #region "Movement Speed Functions"
    public void RegisterOnMovementSpeedChangeCallback(Action<float> callback)
    {
        movementSpeedChangeCallbacks.Add(callback);
    }

    private void NotifyMovementSpeedChanged()
    {
        float movementSpeed = baseMovementSpeed;
        foreach (float addativeMod in addativeMovementSpeedMods)
        {
            movementSpeed += addativeMod;
        }
        foreach (float multiplicativeMod in multiplicativeMovementSpeedMods)
        {
            movementSpeed *= multiplicativeMod;
        }

        movementSpeedChangeCallbacks.ForEach(callback => callback(movementSpeed));
    }

    public void ApplyAddativeSpeedModifier(float speedDelta, float duration)
    {
        StartCoroutine(ApplySpeedModifier(speedDelta, duration, true));
    }

    public void ApplyMultiplicativeSpeedModifier(float speedDelta, float duration)
    {
        StartCoroutine(ApplySpeedModifier(speedDelta, duration, false));
    }

    private IEnumerator ApplySpeedModifier(float speedDelta, float duration, bool additive)
    {
        if (additive)
            addativeMovementSpeedMods.Add(speedDelta);
        else
            multiplicativeMovementSpeedMods.Add(speedDelta);

        NotifyMovementSpeedChanged();

        yield return new WaitForSeconds(duration);

        
        if (additive)
            addativeMovementSpeedMods.Remove(speedDelta);
        else
            multiplicativeMovementSpeedMods.Remove(speedDelta);

        NotifyMovementSpeedChanged();
    }
    #endregion

    #region "Cooldown Reduction Functions"
    public float GetCDRMulti()
    {
        int totalCDRScore = cooldownReductionMods.Sum();

        const float halveCDRIncrement = 10f;
        float cdrMulti = Mathf.Pow(
            0.5f,
            totalCDRScore / halveCDRIncrement);

        return cdrMulti;
    }

    public void AddCooldownReductionBuff(int buffStrength, float duration)
    {
        StartCoroutine(ApplyCooldownReductionBuff(buffStrength, duration));
    }

    private IEnumerator ApplyCooldownReductionBuff(int buffStrength, float duration)
    {
        cooldownReductionMods.Add(buffStrength);

        yield return new WaitForSeconds(duration);

        cooldownReductionMods.Remove(buffStrength);
    }
    #endregion

    #region "Buff Index Functions"
    public float GetDamageMulti(string damageType)
    {
        BuffType buffType = BuffType.BuffTypeFromString(damageType);
        return 1f + damageBuffs.GetBuffTypeStrength(buffType);
    }

    public float GetResistanceMulti(string damageType)
    {
        BuffType buffType = BuffType.BuffTypeFromString(damageType);
        float resistanceScore = resistanceBuffs.GetBuffTypeStrength(buffType);

        const float halveDamageIncrement = 10f;
        float resistanceMulti = Mathf.Pow(
            0.5f,
            resistanceScore / halveDamageIncrement);

        return resistanceMulti;
    }

    public void AddDamageBuff(
        BuffType type,
        BuffSource source,
        float strength,
        float duration)
    {
        damageBuffs.AddBuff(type, source, strength);
        StartCoroutine(
            ExpireBuffAfterDuration(damageBuffs, type, source, strength, duration));
    }

    public void AddPersistentDamageBuff(
        BuffType type,
        BuffSource source,
        float strength)
    {
        damageBuffs.AddBuff(type, source, strength);
    }

    public void RemovePersistentDamageBuff(
        BuffType type,
        BuffSource source,
        float strength)
    {
        RemoveBuff(damageBuffs, type, source, strength);
    }

    public void AddResistanceBuff(
        BuffType type,
        BuffSource source,
        float strength,
        float duration)
    {
        resistanceBuffs.AddBuff(type, source, strength);
        StartCoroutine(
            ExpireBuffAfterDuration(resistanceBuffs, type, source, strength, duration));
    }

    public void AddPersistentResistanceBuff(
        BuffType type,
        BuffSource source,
        float strength)
    {
        resistanceBuffs.AddBuff(type, source, strength);
    }

    public void RemovePersistentResistanceBuff(
        BuffType type,
        BuffSource source,
        float strength)
    {
        RemoveBuff(resistanceBuffs, type, source, strength);
    }

    private IEnumerator ExpireBuffAfterDuration(
        BuffIndex buffIndex,
        BuffType type,
        BuffSource source,
        float strength,
        float duration)
    {
        yield return new WaitForSeconds(duration);

        RemoveBuff(buffIndex, type, source, strength);
    }

    public void RemoveBuff(
        BuffIndex buffIndex,
        BuffType type,
        BuffSource source,
        float strength)
    {
        // Continue only if Type key is valid.
        if (buffIndex.buffs.ContainsKey(type))
        {
            Dictionary<BuffSource, List<float>> typeSourceBuffs = buffIndex.buffs[type];

            // Continue only if Source key is valid.
            List<float> sourceModifiers;
            if (typeSourceBuffs.ContainsKey(source))
            {
                List<float> sourceBuffs = typeSourceBuffs[source];
                sourceBuffs.Remove(strength);
            }
            else
            {
                Debug.Log($"Cannot remove buff ({type.name}:{source.name}:{strength}) because the Source is invalid.");
            }
        }
        else
        {
            Debug.Log($"Cannot remove buff ({type.name}:{source.name}:{strength}) because the Type is invalid.");
        }
    }
    #endregion
}
