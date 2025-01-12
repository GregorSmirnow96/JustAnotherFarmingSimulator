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
            new EquipmentEffect("FaeWand", ApplyFaeWand, RemoveFaeWand)
        };
    }

    public EquipmentEffect GetEquipmentEffect(string itemId)
    {
        return effects.FirstOrDefault(effect => effect.itemId.Equals(itemId));
    }

    private void ApplyFaeWand()
    {
        Debug.Log("Applying FaeWand effect: +75% Physical damage buff");
        playerStats.AddPersistentDamageBuff(BuffType.Physical, BuffSource.Armour, 0.75f);
        Debug.Log(playerStats.GetDamageMulti(BuffType.Physical.name));
    }

    private void RemoveFaeWand()
    {
        Debug.Log("Removing FaeWand effect: -75% Physical damage buff");
        playerStats.RemovePersistentDamageBuff(BuffType.Physical, BuffSource.Armour, 0.75f);
        Debug.Log(playerStats.GetDamageMulti(BuffType.Physical.name));
    }
}
