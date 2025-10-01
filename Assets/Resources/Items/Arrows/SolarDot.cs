using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarDot : MonoBehaviour
{
    public int minDamagePerTick = 2;
    public int maxDamagePerTick = 5;
    public int ticks = 12;

    private float damageMulti;

    void Start()
    {
        StartCoroutine(ApplyDot());
    }

    public void SetDamageMulti(float multiplier)
    {
        damageMulti = multiplier;
    }

    private int ScaledDamage(
        int damage,
        string damageType) =>
            (int) (damageMulti * PlayerProperties.GetScaledPlayerDamage(damage, damageType));

    private IEnumerator ApplyDot()
    {
        Health health = GetComponent<Health>();

        if (health != null)
        {
            for (int i = 0; i < ticks; i++)
            {
                string damageType = i % 2 == 0 ? DamageType.Fire : DamageType.Lightning;
                int baseDamage = Random.Range(minDamagePerTick, maxDamagePerTick);
                int scaledDamage = ScaledDamage(baseDamage, damageType);
                Debug.Log($"DamageMulti: {damageMulti} ==> {scaledDamage}");
                health.TakeDamage(scaledDamage, damageType);

                yield return new WaitForSeconds(0.15f);
            }
        }

        Destroy(this);
    }
}
