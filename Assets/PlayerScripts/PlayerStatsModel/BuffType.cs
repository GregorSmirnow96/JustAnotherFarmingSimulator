using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuffType
{
    public static readonly BuffType Physical = new BuffType("Physical");
    public static readonly BuffType Water = new BuffType("Water");
    public static readonly BuffType Fire = new BuffType("Fire");
    public static readonly BuffType Lightning = new BuffType("Lightning");

    public string name;

    public BuffType(string name)
    {
        this.name = name;
    }

    public static BuffType BuffTypeFromString(string damageType)
    {
        switch (damageType)
        {
            case DamageType.Physical:
                return Physical;
            case DamageType.Water:
                return Water;
            case DamageType.Fire:
                return Fire;
            case DamageType.Lightning:
                return Lightning;
            default:
                Debug.Log($"DamageType {damageType} does not correspond to a BuffType.");
                return null;
        }
    }
}