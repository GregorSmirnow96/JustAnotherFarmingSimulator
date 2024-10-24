using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttacker
{
    public string GetCurrentAttackName();
    public bool InAttackAnimaion();
    public bool GetAttackHasHit();
    public void SetAttackHasHit();
    public int GetDamageForCurrentAttack();
}
