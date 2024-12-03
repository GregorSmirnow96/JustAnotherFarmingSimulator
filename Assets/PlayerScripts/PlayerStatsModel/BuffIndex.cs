using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuffIndex
{
    /* FOR ALL DAMAGE/RESISTANCE MODS:
        There are different damage amp sources (potions, amulets, spells, etc.).
        Each source only applies the strongest buff of that source.
        I.E:
            If two potions were used where they give:
                1) 20% amp for 10 seconds
                2) 40% amp for 6 seconds
            The player gets 40% amp for 6 seconds, then 20% for 4 seconds.

            If a potion is giving 15% amp and an amulet is giving 10%, the total
                amp is 25% (15% + 10%).
    */

    public Dictionary<BuffType, Dictionary<BuffSource, List<float>>> buffs;

    public BuffIndex()
    {
        buffs = new Dictionary<BuffType, Dictionary<BuffSource, List<float>>>();
    }

    public void AddBuff(
        BuffType type,
        BuffSource source,
        float strength,
        float duration)
    {
        // Get (& add if necessary) the Type KVP.
        Dictionary<BuffSource, List<float>> typeSourceBuffs;
        if (buffs.ContainsKey(type))
        {
            typeSourceBuffs = buffs[type];
        }
        else
        {
            typeSourceBuffs = new Dictionary<BuffSource, List<float>>();
            buffs.Add(type, typeSourceBuffs);
        }

        // Get (& add if necessary) the TypeSource KVP.
        List<float> sourceBuffs;
        if (typeSourceBuffs.ContainsKey(source))
        {
            sourceBuffs = typeSourceBuffs[source];
        }
        else
        {
            sourceBuffs = new List<float>();
            typeSourceBuffs.Add(source, sourceBuffs);
        }

        // Add mod to TypeSources.
        sourceBuffs.Add(strength);
    }

    public float GetBuffTypeStrength(BuffType type)
    {
        // Continue only if Type key is valid.
        if (buffs.ContainsKey(type))
        {
            Dictionary<BuffSource, List<float>> typeSourceBuffs = buffs[type];

            // If there are no buffs of this type, return 0;
            if (typeSourceBuffs.ToList().Count() == 0)
            {
                return 0f;
            }

            // If there are buffs, sum up all the largest buffs from each source.
            return typeSourceBuffs
                .ToList()
                .Select(sourceBuffKvp => sourceBuffKvp.Value)
                .Select(sourceBuffs => sourceBuffs.Count() == 0 ? 0 : sourceBuffs.Max())
                .Aggregate((buff1, buff2) => buff1 + buff2);
        }
        else
        {
            // Debug.Log($"Cannot get buff strength of the Type {type.name} because the Type is invalid.");
        }

        return 0f;
    }
}
