using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EquipmentEffect
{
    public string itemId;
    public Action applyEffect;
    public Action removeEffect;

    public EquipmentEffect(string itemId, Action applyEffect, Action removeEffect)
    {
        this.itemId = itemId;
        this.applyEffect = applyEffect;
        this.removeEffect = removeEffect;
    }
}

public class EquipmentEffectRepo
{
    private static EquipmentEffectRepo Instance;
    public static EquipmentEffectRepo GetInstance() =>
        Instance == null
            ? Instance = new EquipmentEffectRepo()
            : Instance;

    private PlayerStats playerStats;
    private List<EquipmentEffect> effects;

    private EquipmentEffectRepo()
    {
        playerStats = SceneProperties.playerTransform.GetComponent<PlayerStats>();

        effects = new List<EquipmentEffect>()
        {
            new EquipmentEffect("RoughTunic", ApplyRoughTunic, RemoveRoughTunic),
            new EquipmentEffect("QualityTunic", ApplyQualityTunic, RemoveQualityTunic),
            new EquipmentEffect("FireTunic", ApplyFireTunic, RemoveFireTunic),
            new EquipmentEffect("LightningTunic", ApplyLightningTunic, RemoveLightningTunic),
            new EquipmentEffect("WaterTunic", ApplyWaterTunic, RemoveWaterTunic),
            new EquipmentEffect("FaeTunic", ApplyFaeTunic, RemoveFaeTunic),

            new EquipmentEffect("IronArmour", ApplyIronArmour, RemoveIronArmour),
            new EquipmentEffect("SilverArmour", ApplySilverArmour, RemoveSilverArmour),
            new EquipmentEffect("GoldArmour", ApplyGoldArmour, RemoveGoldArmour),
            new EquipmentEffect("CelestialArmour", ApplyCelestialArmour, RemoveCelestialArmour),
            new EquipmentEffect("FireArmour", ApplyFireArmour, RemoveFireArmour),
            new EquipmentEffect("LightningArmour", ApplyLightningArmour, RemoveLightningArmour),
            new EquipmentEffect("WaterArmour", ApplyWaterArmour, RemoveWaterArmour),
            new EquipmentEffect("FaeArmour", ApplyFaeArmour, RemoveFaeArmour),

            new EquipmentEffect("AquamarineRing", ApplyAquamarineRing, RemoveAquamarineRing),
            new EquipmentEffect("OnyxRing", ApplyOnyxRing, RemoveOnyxRing),
            new EquipmentEffect("RubyRing", ApplyRubyRing, RemoveRubyRing),
            new EquipmentEffect("TopazRing", ApplyTopazRing, RemoveTopazRing),

            new EquipmentEffect("AquamarineNecklace", ApplyAquamarineNecklace, RemoveAquamarineNecklace),
            new EquipmentEffect("OnyxNecklace", ApplyOnyxNecklace, RemoveOnyxNecklace),
            new EquipmentEffect("RubyNecklace", ApplyRubyNecklace, RemoveRubyNecklace),
            new EquipmentEffect("TopazNecklace", ApplyTopazNecklace, RemoveTopazNecklace)
        };
    }

    public EquipmentEffect GetEquipmentEffect(string itemId)
    {
        return effects.FirstOrDefault(effect => effect.itemId.Equals(itemId));
    }

    // Aquamarine Ring
    private void ApplyAquamarineRing()
    {
        playerStats.AddPersistentResistanceBuff(BuffType.Water, BuffSource.Ring, 3f);
        playerStats.AddPersistentDamageBuff(BuffType.Water, BuffSource.Ring, 0.3f);
    }

    private void RemoveAquamarineRing()
    {
        playerStats.RemovePersistentResistanceBuff(BuffType.Water, BuffSource.Ring, 3f);
        playerStats.RemovePersistentDamageBuff(BuffType.Water, BuffSource.Ring, 0.3f);
    }

    // Onyx Ring
    private void ApplyOnyxRing()
    {
        playerStats.AddPersistentResistanceBuff(BuffType.Physical, BuffSource.Ring, 3f);
        playerStats.AddPersistentDamageBuff(BuffType.Physical, BuffSource.Ring, 0.3f);
    }

    private void RemoveOnyxRing()
    {
        playerStats.RemovePersistentResistanceBuff(BuffType.Physical, BuffSource.Ring, 3f);
        playerStats.RemovePersistentDamageBuff(BuffType.Physical, BuffSource.Ring, 0.3f);
    }

    // Ruby Ring
    private void ApplyRubyRing()
    {
        playerStats.AddPersistentResistanceBuff(BuffType.Fire, BuffSource.Ring, 3f);
        playerStats.AddPersistentDamageBuff(BuffType.Fire, BuffSource.Ring, 0.3f);
    }

    private void RemoveRubyRing()
    {
        playerStats.RemovePersistentResistanceBuff(BuffType.Fire, BuffSource.Ring, 3f);
        playerStats.RemovePersistentDamageBuff(BuffType.Fire, BuffSource.Ring, 0.3f);
    }

    // Topaz Ring
    private void ApplyTopazRing()
    {
        playerStats.AddPersistentResistanceBuff(BuffType.Lightning, BuffSource.Ring, 3f);
        playerStats.AddPersistentDamageBuff(BuffType.Lightning, BuffSource.Ring, 0.3f);
    }

    private void RemoveTopazRing()
    {
        playerStats.RemovePersistentResistanceBuff(BuffType.Lightning, BuffSource.Ring, 3f);
        playerStats.RemovePersistentDamageBuff(BuffType.Lightning, BuffSource.Ring, 0.3f);
    }

    // Aquamarine Necklace
    private void ApplyAquamarineNecklace()
    {
        playerStats.AddPersistentResistanceBuff(BuffType.Physical, BuffSource.Armour, 4f);
    }

    private void RemoveAquamarineNecklace()
    {
        playerStats.RemovePersistentResistanceBuff(BuffType.Physical, BuffSource.Armour, 4f);
    }

    // Onyx Necklace
    private void ApplyOnyxNecklace()
    {
        playerStats.AddPersistentResistanceBuff(BuffType.Water, BuffSource.Armour, 4f);
        playerStats.AddPersistentResistanceBuff(BuffType.Lightning, BuffSource.Armour, 4f);
        playerStats.AddPersistentResistanceBuff(BuffType.Fire, BuffSource.Armour, 4f);
    }

    private void RemoveOnyxNecklace()
    {
        playerStats.RemovePersistentResistanceBuff(BuffType.Water, BuffSource.Armour, 4f);
        playerStats.RemovePersistentResistanceBuff(BuffType.Lightning, BuffSource.Armour, 4f);
        playerStats.RemovePersistentResistanceBuff(BuffType.Fire, BuffSource.Armour, 4f);
    }

    // Ruby Necklace
    private void ApplyRubyNecklace()
    {
        playerStats.AddPersistentCDRBuff(4);
    }

    private void RemoveRubyNecklace()
    {
        playerStats.RemovePersistentCDRBuff(4);
    }

    // Topaz Necklace
    private void ApplyTopazNecklace()
    {
        playerStats.AddPersistentSpeedModifier(0.15f, true);
    }

    private void RemoveTopazNecklace()
    {
        playerStats.RemovePersistentSpeedModifier(0.15f, true);
    }

    // Rough Tunic
    private void ApplyRoughTunic()
    {
        playerStats.AddPersistentResistanceBuff(BuffType.Physical, BuffSource.Armour, 4f);
        Debug.Log(playerStats.GetResistanceMulti(BuffType.Physical.name));
    }

    private void RemoveRoughTunic()
    {
        playerStats.RemovePersistentResistanceBuff(BuffType.Physical, BuffSource.Armour, 4f);
        Debug.Log(playerStats.GetResistanceMulti(BuffType.Physical.name));
    }

    // Quality Tunic
    private void ApplyQualityTunic()
    {
        playerStats.AddPersistentResistanceBuff(BuffType.Physical, BuffSource.Armour, 7f);
    }

    private void RemoveQualityTunic()
    {
        playerStats.RemovePersistentResistanceBuff(BuffType.Physical, BuffSource.Armour, 7f);
    }

    // Fire Tunic
    private void ApplyFireTunic()
    {
        playerStats.AddPersistentResistanceBuff(BuffType.Physical, BuffSource.Armour, 7f);
        playerStats.AddPersistentDamageBuff(BuffType.Fire, BuffSource.Armour, 0.5f);
    }

    private void RemoveFireTunic()
    {
        playerStats.RemovePersistentResistanceBuff(BuffType.Physical, BuffSource.Armour, 7f);
        playerStats.RemovePersistentDamageBuff(BuffType.Fire, BuffSource.Armour, 0.5f);
    }

    // Lightning Tunic
    private void ApplyLightningTunic()
    {
        playerStats.AddPersistentResistanceBuff(BuffType.Physical, BuffSource.Armour, 7f);
        playerStats.AddPersistentDamageBuff(BuffType.Lightning, BuffSource.Armour, 0.5f);
    }

    private void RemoveLightningTunic()
    {
        playerStats.RemovePersistentResistanceBuff(BuffType.Physical, BuffSource.Armour, 7f);
        playerStats.RemovePersistentDamageBuff(BuffType.Lightning, BuffSource.Armour, 0.5f);
    }

    // Water Tunic
    private void ApplyWaterTunic()
    {
        playerStats.AddPersistentResistanceBuff(BuffType.Physical, BuffSource.Armour, 7f);
        playerStats.AddPersistentDamageBuff(BuffType.Water, BuffSource.Armour, 0.5f);
    }

    private void RemoveWaterTunic()
    {
        playerStats.RemovePersistentResistanceBuff(BuffType.Physical, BuffSource.Armour, 7f);
        playerStats.RemovePersistentDamageBuff(BuffType.Water, BuffSource.Armour, 0.5f);
    }

    // Fae Tunic
    private void ApplyFaeTunic()
    {
        playerStats.AddPersistentResistanceBuff(BuffType.Physical, BuffSource.Armour, 7f);
        playerStats.AddPersistentSpeedModifier(0.3f, true);
        playerStats.AddPersistentCDRBuff(6);
    }

    private void RemoveFaeTunic()
    {
        playerStats.RemovePersistentResistanceBuff(BuffType.Physical, BuffSource.Armour, 7f);
        playerStats.RemovePersistentSpeedModifier(0.3f, true);
        playerStats.RemovePersistentCDRBuff(6);
    }

    // Iron Armour
    private void ApplyIronArmour()
    {
        playerStats.AddPersistentResistanceBuff(BuffType.Physical, BuffSource.Armour, 5f);
    }

    private void RemoveIronArmour()
    {
        playerStats.RemovePersistentResistanceBuff(BuffType.Physical, BuffSource.Armour, 5f);
    }

    // Silver Armour
    private void ApplySilverArmour()
    {
        playerStats.AddPersistentResistanceBuff(BuffType.Physical, BuffSource.Armour, 7f);
        playerStats.AddPersistentSpeedModifier(0.12f, true);
    }

    private void RemoveSilverArmour()
    {
        playerStats.RemovePersistentResistanceBuff(BuffType.Physical, BuffSource.Armour, 7f);
        playerStats.RemovePersistentSpeedModifier(0.12f, true);
    }

    // Gold Armour
    private void ApplyGoldArmour()
    {
        playerStats.AddPersistentResistanceBuff(BuffType.Physical, BuffSource.Armour, 9f);
        playerStats.AddPersistentSpeedModifier(0.12f, true);
    }

    private void RemoveGoldArmour()
    {
        playerStats.RemovePersistentResistanceBuff(BuffType.Physical, BuffSource.Armour, 9f);
        playerStats.RemovePersistentSpeedModifier(0.12f, true);
    }

    // Celestial Armour
    private void ApplyCelestialArmour()
    {
        playerStats.AddPersistentResistanceBuff(BuffType.Physical, BuffSource.Armour, 13f);
    }

    private void RemoveCelestialArmour()
    {
        playerStats.RemovePersistentResistanceBuff(BuffType.Physical, BuffSource.Armour, 13f);
    }

    // Fire Armour
    private void ApplyFireArmour()
    {
        playerStats.AddPersistentResistanceBuff(BuffType.Physical, BuffSource.Armour, 13f);
        playerStats.AddPersistentResistanceBuff(BuffType.Fire, BuffSource.Armour, 7f);
    }

    private void RemoveFireArmour()
    {
        playerStats.RemovePersistentResistanceBuff(BuffType.Physical, BuffSource.Armour, 13f);
        playerStats.RemovePersistentResistanceBuff(BuffType.Fire, BuffSource.Armour, 7f);
    }

    // Lightning Armour
    private void ApplyLightningArmour()
    {
        playerStats.AddPersistentResistanceBuff(BuffType.Physical, BuffSource.Armour, 13f);
        playerStats.AddPersistentResistanceBuff(BuffType.Lightning, BuffSource.Armour, 7f);
    }

    private void RemoveLightningArmour()
    {
        playerStats.RemovePersistentResistanceBuff(BuffType.Physical, BuffSource.Armour, 13f);
        playerStats.RemovePersistentResistanceBuff(BuffType.Lightning, BuffSource.Armour, 7f);
    }

    // Water Armour
    private void ApplyWaterArmour()
    {
        playerStats.AddPersistentResistanceBuff(BuffType.Physical, BuffSource.Armour, 13f);
        playerStats.AddPersistentResistanceBuff(BuffType.Water, BuffSource.Armour, 7f);
    }

    private void RemoveWaterArmour()
    {
        playerStats.RemovePersistentResistanceBuff(BuffType.Physical, BuffSource.Armour, 13f);
        playerStats.RemovePersistentResistanceBuff(BuffType.Water, BuffSource.Armour, 7f);
    }

    // Fae Armour
    private void ApplyFaeArmour()
    {
        playerStats.AddPersistentResistanceBuff(BuffType.Physical, BuffSource.Armour, 13f);
        playerStats.AddPersistentSpeedModifier(0.2f, true);
    }

    private void RemoveFaeArmour()
    {
        playerStats.RemovePersistentResistanceBuff(BuffType.Physical, BuffSource.Armour, 13f);
        playerStats.RemovePersistentSpeedModifier(0.2f, true);
    }
}
