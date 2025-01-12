using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyEquipment : MonoBehaviour
{
    private PlayerStats stats;
    private Inventory inventory;
    private EquipmentEffectRepo equipementEffectRepo;

    private string previousNecklaceItemId;
    private string previousArmourItemId;
    private string previousRingItemId;

    void Start()
    {
        stats = GetComponent<PlayerStats>();
        inventory = GetComponent<Inventory>();
        equipementEffectRepo = EquipmentEffectRepo.GetInstance();
    }

    void Update()
    {
        string necklaceItemId = inventory.necklace?.type?.id ?? "NULL";
        string armourItemId = inventory.armour?.type?.id ?? "NULL";
        string ringItemId = inventory.ring?.type?.id ?? "NULL";
        // Debug.Log($"Equipped: {necklaceItemId} - {armourItemId} - {ringItemId}");

        // Update necklace effect
        if (!necklaceItemId.Equals(previousNecklaceItemId))
        {
            EquipmentEffect previousItemEffect = equipementEffectRepo.GetEquipmentEffect(previousNecklaceItemId);
            EquipmentEffect currentItemEffect = equipementEffectRepo.GetEquipmentEffect(necklaceItemId);

            if (previousItemEffect != null) previousItemEffect.removeEffect();
            if (currentItemEffect != null) currentItemEffect.applyEffect();
        }
        
        // Update armour effect
        if (!armourItemId.Equals(previousArmourItemId))
        {
            EquipmentEffect previousItemEffect = equipementEffectRepo.GetEquipmentEffect(previousArmourItemId);
            EquipmentEffect currentItemEffect = equipementEffectRepo.GetEquipmentEffect(armourItemId);

            if (previousItemEffect != null) previousItemEffect.removeEffect();
            if (currentItemEffect != null) currentItemEffect.applyEffect();
        }
        
        // Update ring effect
        if (!ringItemId.Equals(previousRingItemId))
        {
            EquipmentEffect previousItemEffect = equipementEffectRepo.GetEquipmentEffect(previousRingItemId);
            EquipmentEffect currentItemEffect = equipementEffectRepo.GetEquipmentEffect(ringItemId);

            if (previousItemEffect != null) previousItemEffect.removeEffect();
            if (currentItemEffect != null) currentItemEffect.applyEffect();
        }

        previousNecklaceItemId = necklaceItemId;
        previousArmourItemId = armourItemId;
        previousRingItemId = ringItemId;
    }
}
