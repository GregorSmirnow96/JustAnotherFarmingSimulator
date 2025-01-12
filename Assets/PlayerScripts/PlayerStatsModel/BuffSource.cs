using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BuffSource
{
    public static readonly BuffSource Potion = new BuffSource("Potion");
    public static readonly BuffSource Spell = new BuffSource("Spell");
    public static readonly BuffSource Necklace = new BuffSource("Necklace");
    public static readonly BuffSource Ring = new BuffSource("Ring");
    public static readonly BuffSource Armour = new BuffSource("Armour");

    public string name;

    public BuffSource(string name)
    {
        this.name = name;
    }
}