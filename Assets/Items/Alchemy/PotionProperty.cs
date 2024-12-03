using UnityEngine;

public class PotionProperty
{
    public static string None = "None";

    public const string HealImmediately = "HealImmediately";
    public const string HealOverTime = "HealOverTime";
    public const string MovementSpeed = "MovementSpeed";
    public const string CooldownReduction = "CooldownReduction";
    public const string PhysicalDamage = "PhysicalrDamage";
    public const string WaterDamage = "WaterDamage";
    public const string FireDamage = "FireDamage";
    public const string LightningDamage = "LightningDamage";
    public const string PhysicalResistance = "PhysicalResistance";
    public const string WaterResistance = "WaterResistance";
    public const string FireResistance = "FireResistance";
    public const string LightningResistance = "LightningResistance";
    public const string Fertilize = "Fertilize";

    public static Vector3 GetPropertyHSV(string property)
    {
        switch (property)
        {
            case HealImmediately:
                return new Vector3(26f/255f, 0.46f, 1f);
            case HealOverTime:
                return new Vector3(6f/255f, 0.46f, 1f);
            case MovementSpeed:
                return new Vector3(278f/255f, 0.46f, 1f);
            case CooldownReduction:
                return new Vector3(190f/255f, 0.46f, 1f);
            case PhysicalDamage:
                return new Vector3(128f/255f, 0.46f, 1f);
            case WaterDamage:
                return new Vector3(208f/255f, 0.46f, 1f);
            case FireDamage:
                return new Vector3(10f/255f, 0.46f, 1f);
            case LightningDamage:
                return new Vector3(57f/255f, 0.46f, 1f);
            case PhysicalResistance:
                return new Vector3(108f/255f, 0.46f, 1f);
            case WaterResistance:
                return new Vector3(228f/255f, 0.46f, 1f);
            case FireResistance:
                return new Vector3(0f/255f, 0.46f, 1f);
            case LightningResistance:
                return new Vector3(37f/255f, 0.46f, 1f);
            case Fertilize:
                return new Vector3(221f/255f, 0.46f, 1f);
            default:
                return new Vector3(188f/255f, 0.02f, 0.5f);
        }
    }
}
