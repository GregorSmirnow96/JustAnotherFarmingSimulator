using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Drinkable : MonoBehaviour, IUsable
{
    private Animator animator;
    private PlayerStats stats;

    void Start()
    {
        animator = GetComponent<Animator>();
        stats = SceneProperties.playerTransform.GetComponent<PlayerStats>();
    }

    public void Use()
    {
        Toolbar.instance.SetScrollingEnabled(false);
        animator.SetTrigger("Drink");
        StartCoroutine(ApplyEffectsWhenDrank());
        StartCoroutine(SelfDestructWhenPlayerThrows());
    }

    private IEnumerator ApplyEffectsWhenDrank()
    {
        // 45 is the drinking frame of the animation.
        yield return new WaitForSeconds(45f/60f);

        Item equippedItem = Toolbar.instance.GetEquippedItem();
        if (equippedItem is PotionItem)
        {
            PotionItem equippedPotion = equippedItem as PotionItem;

            string properties = equippedPotion
                .propertyStrengths
                .ToList()
                .Select(kvp => $"{kvp.Key}:{kvp.Value}")
                .Aggregate((s1, s2) => $"{s1}, {s2}");
            Debug.Log(properties);

            equippedPotion
                .propertyStrengths
                .ToList()
                .ForEach(kvp => ApplyEffect(kvp.Key, kvp.Value));
        }
    }

    private IEnumerator SelfDestructWhenPlayerThrows()
    {
        // 72 is the last frame of the animation.
        // Destroy the potion flask once animation completes.
        yield return new WaitForSeconds(72f/60f);

        Toolbar.instance.DeleteEquippedItem();
        Toolbar.instance.SetScrollingEnabled(true);
    }

    #region "Buff Implementations"

    private void ApplyEffect(string property, int tier)
    {
        switch (property)
        {
            case PotionProperty.HealImmediately:
                ApplyHealImmediatelyBuff(tier);
                break;
            case PotionProperty.HealOverTime:
                ApplyHealOverTimeBuff(tier);
                break;
            case PotionProperty.MovementSpeed:
                ApplyMovementSpeedBuff(tier);
                break;
            case PotionProperty.CooldownReduction:
                ApplyCooldownReductionBuff(tier);
                break;
            case PotionProperty.PhysicalDamage:
                ApplyPhysicalDamageBuff(tier);
                break;
            case PotionProperty.WaterDamage:
                ApplyWaterDamageBuff(tier);
                break;
            case PotionProperty.FireDamage:
                ApplyFireDamageBuff(tier);
                break;
            case PotionProperty.LightningDamage:
                ApplyLightningDamageBuff(tier);
                break;
            case PotionProperty.PhysicalResistance:
                ApplyPhysicalResistanceBuff(tier);
                break;
            case PotionProperty.WaterResistance:
                ApplyWaterResistanceBuff(tier);
                break;
            case PotionProperty.FireResistance:
                ApplyFireResistanceBuff(tier);
                break;
            case PotionProperty.LightningResistance:
                ApplyLightningResistanceBuff(tier);
                break;
        }
    }

    private void ApplyHealImmediatelyBuff(int tier)
    {
        switch (tier)
        {
            case 0:
                stats.health.HealPercentImmediately(0.2f);
                break;
            case 1:
                stats.health.HealPercentImmediately(0.35f);
                break;
            case 2:
                stats.health.HealPercentImmediately(0.5f);
                break;
            default:
                Debug.Log($"Invalid HealImmediately buff tier: {tier}.");
                break;
        }
    }

    private void ApplyHealOverTimeBuff(int tier)
    {
        switch (tier)
        {
            case 0:
                stats.health.HealPercentOverTime(0.3f, 8f);
                break;
            case 1:
                stats.health.HealPercentOverTime(0.45f, 12f);
                break;
            case 2:
                stats.health.HealPercentOverTime(0.6f, 15f);
                break;
            default:
                Debug.Log($"Invalid HealOverTime buff tier: {tier}.");
                break;
        }
    }

    private void ApplyMovementSpeedBuff(int tier)
    {
        switch (tier)
        {
            case 0:
                stats.ApplyAddativeSpeedModifier(1f, 20f);
                break;
            case 1:
                stats.ApplyAddativeSpeedModifier(2f, 20f);
                break;
            case 2:
                stats.ApplyMultiplicativeSpeedModifier(1.4f, 30f);
                break;
            default:
                Debug.Log($"Invalid movement speed buff tier: {tier}.");
                break;
        }
    }

    private void ApplyCooldownReductionBuff(int tier)
    {
        switch (tier)
        {
            case 0:
                stats.AddCooldownReductionBuff(3, 15f);
                break;
            case 1:
                stats.AddCooldownReductionBuff(6, 15f);
                break;
            case 2:
                stats.AddCooldownReductionBuff(10, 20f);
                break;
            default:
                Debug.Log($"Invalid CooldownReduction buff tier: {tier}.");
                break;
        }
    }

    private void ApplyPhysicalDamageBuff(int tier)
    {
        switch (tier)
        {
            case 0:
                stats.AddDamageBuff(BuffType.Physical, BuffSource.Potion, 0.2f, 30f);
                break;
            case 1:
                stats.AddDamageBuff(BuffType.Physical, BuffSource.Potion, 0.4f, 30f);
                break;
            case 2:
                stats.AddDamageBuff(BuffType.Physical, BuffSource.Potion, 0.5f, 45f);
                break;
            default:
                Debug.Log($"Invalid PhysicalDamage buff tier: {tier}.");
                break;
        }
    }

    private void ApplyWaterDamageBuff(int tier)
    {
        switch (tier)
        {
            case 0:
                stats.AddDamageBuff(BuffType.Water, BuffSource.Potion, 0.2f, 30f);
                break;
            case 1:
                stats.AddDamageBuff(BuffType.Water, BuffSource.Potion, 0.4f, 30f);
                break;
            case 2:
                stats.AddDamageBuff(BuffType.Water, BuffSource.Potion, 0.5f, 45f);
                break;
            default:
                Debug.Log($"Invalid WaterDamage buff tier: {tier}.");
                break;
        }
    }

    private void ApplyFireDamageBuff(int tier)
    {
        switch (tier)
        {
            case 0:
                stats.AddDamageBuff(BuffType.Fire, BuffSource.Potion, 0.2f, 30f);
                break;
            case 1:
                stats.AddDamageBuff(BuffType.Fire, BuffSource.Potion, 0.4f, 30f);
                break;
            case 2:
                stats.AddDamageBuff(BuffType.Fire, BuffSource.Potion, 0.5f, 45f);
                break;
            default:
                Debug.Log($"Invalid FireDamage buff tier: {tier}.");
                break;
        }
    }

    private void ApplyLightningDamageBuff(int tier)
    {
        switch (tier)
        {
            case 0:
                stats.AddDamageBuff(BuffType.Lightning, BuffSource.Potion, 0.2f, 30f);
                break;
            case 1:
                stats.AddDamageBuff(BuffType.Lightning, BuffSource.Potion, 0.4f, 30f);
                break;
            case 2:
                stats.AddDamageBuff(BuffType.Lightning, BuffSource.Potion, 0.5f, 45f);
                break;
            default:
                Debug.Log($"Invalid LightningDamage buff tier: {tier}.");
                break;
        }
    }

    private void ApplyPhysicalResistanceBuff(int tier)
    {
        switch (tier)
        {
            case 0:
                stats.AddResistanceBuff(BuffType.Physical, BuffSource.Potion, 3f, 30f);
                break;
            case 1:
                stats.AddResistanceBuff(BuffType.Physical, BuffSource.Potion, 6f, 30f);
                break;
            case 2:
                stats.AddResistanceBuff(BuffType.Physical, BuffSource.Potion, 10f, 45f);
                break;
            default:
                Debug.Log($"Invalid PhysicalResistance buff tier: {tier}.");
                break;
        }
    }

    private void ApplyWaterResistanceBuff(int tier)
    {
        switch (tier)
        {
            case 0:
                stats.AddResistanceBuff(BuffType.Water, BuffSource.Potion, 3f, 30f);
                break;
            case 1:
                stats.AddResistanceBuff(BuffType.Water, BuffSource.Potion, 6f, 30f);
                break;
            case 2:
                stats.AddResistanceBuff(BuffType.Water, BuffSource.Potion, 10f, 45f);
                break;
            default:
                Debug.Log($"Invalid WaterResistance buff tier: {tier}.");
                break;
        }
    }

    private void ApplyFireResistanceBuff(int tier)
    {
        switch (tier)
        {
            case 0:
                stats.AddResistanceBuff(BuffType.Fire, BuffSource.Potion, 3f, 30f);
                break;
            case 1:
                stats.AddResistanceBuff(BuffType.Fire, BuffSource.Potion, 6f, 30f);
                break;
            case 2:
                stats.AddResistanceBuff(BuffType.Fire, BuffSource.Potion, 10f, 45f);
                break;
            default:
                Debug.Log($"Invalid FireResistance buff tier: {tier}.");
                break;
        }
    }

    private void ApplyLightningResistanceBuff(int tier)
    {
        switch (tier)
        {
            case 0:
                stats.AddResistanceBuff(BuffType.Lightning, BuffSource.Potion, 3f, 30f);
                break;
            case 1:
                stats.AddResistanceBuff(BuffType.Lightning, BuffSource.Potion, 6f, 30f);
                break;
            case 2:
                stats.AddResistanceBuff(BuffType.Lightning, BuffSource.Potion, 10f, 45f);
                break;
            default:
                Debug.Log($"Invalid LightningResistance buff tier: {tier}.");
                break;
        }
    }

    #endregion
}
