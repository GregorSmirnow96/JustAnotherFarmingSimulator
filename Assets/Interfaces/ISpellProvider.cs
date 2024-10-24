using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpellProvider
{
    public int GetSpellCount();
    public List<float> GetSpellCooldowns();
    public List<float> GetRemainingSpellCooldowns();
    public List<bool> GetSpellCastabilities();
    public void CastSpell1();
    public void CastSpell2();
    public void CastSpell3();
    public void CastSpell4();
}
