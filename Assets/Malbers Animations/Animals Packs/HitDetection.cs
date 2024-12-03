using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HitDetection : MonoBehaviour
{
    public GameObject attackerObject;
    public List<string> attackNames;

    private IAttacker attackerScript;

    void Start()
    {
        attackerScript = attackerObject.GetComponent<IAttacker>();
    }

    private void OnTriggerEnter(Collider otherCollider)
    {
        string currentAttack = attackerScript.GetCurrentAttackName();
        if (!attackNames.Contains(currentAttack))
        {
            return;
        }

        if (otherCollider.gameObject.tag == "Player")
        {
            bool isAttacking = attackerScript.InAttackAnimaion();
            bool attackHasAlreadyHit = attackerScript.GetAttackHasHit();
            if (isAttacking && !attackHasAlreadyHit)
            {
                attackerScript.SetAttackHasHit();

                GameObject otherObject = otherCollider.gameObject;
                Health healthToDamage = otherObject.GetComponent<Health>();
                if (healthToDamage != null)
                {
                    int hitDamage = attackerScript.GetDamageForCurrentAttack();
                    healthToDamage.TakeDamage(hitDamage, DamageType.Physical);
                }
            }
        }
    }
}
