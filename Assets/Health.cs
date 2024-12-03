using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int health;
    public int maxHealth;

    public List<Action> healthChangeCallbacks = new List<Action>();
    public List<Action> maxHealthChangeCallbacks = new List<Action>();

    private IHasResistances resistances;

    void Start()
    {
        health = maxHealth;
        resistances = GetComponent<IHasResistances>();
    }

    public void TakeDamage(int damage, string damageType)
    {
        if (resistances != null)
        {
            float resistanceMulti = resistances.GetResistanceMulti(damageType);
            float damageMulti = 1 - resistanceMulti;
            int baseDamage = damage;
            damage = (int) (damage * resistanceMulti);
            Debug.Log($"DmgMulti: {resistanceMulti}, BaseDmg: {baseDamage}, Dmg: {damage}");
        }

        health -= damage;
        healthChangeCallbacks.ForEach(callback => callback());
        if (health <= 0) Destroy(gameObject);
    }

    public void SetMaxHealth(int newMaxHealth)
    {
        int previousMaxHealth = maxHealth;
        maxHealth = newMaxHealth;

        if (maxHealth > previousMaxHealth)
        {
            int gainedHealth = maxHealth - previousMaxHealth;
            HealImmediately(gainedHealth);
        }

        maxHealthChangeCallbacks.ForEach(callback => callback());
    }

    public void RegisterHealthChangeCallback(Action callback)
    {
        healthChangeCallbacks.Add(callback);
    }

    public void RegisterMaxHealthChangeCallback(Action callback)
    {
        maxHealthChangeCallbacks.Add(callback);
    }

    public void HealPercentImmediately(float percent)
    {
        int flatHeal = (int) Math.Round(maxHealth * percent, 0);
        HealImmediately(flatHeal);
    }

    public void HealImmediately(int recoveryAmount)
    {
        health = Mathf.Clamp(health + recoveryAmount, 0, maxHealth);
        healthChangeCallbacks.ForEach(callback => callback());
    }

    public void HealPercentOverTime(float percent, float duration)
    {
        int flatHeal = (int) Math.Round(maxHealth * percent, 0);
        HealOverTime(flatHeal, duration);
    }

    public void HealOverTime(int recoveryAmount, float duration)
    {
        StartCoroutine(ApplyHealOverTime(recoveryAmount, duration));
    }

    private IEnumerator ApplyHealOverTime(int recoveryAmount, float duration)
    {
        float timePerHealthRecovered = duration / recoveryAmount;

        for (int i = 0; i < recoveryAmount; i++)
        {
            HealImmediately(1);
            healthChangeCallbacks.ForEach(callback => callback());
            yield return new WaitForSeconds(timePerHealthRecovered);
        }
    }
}
