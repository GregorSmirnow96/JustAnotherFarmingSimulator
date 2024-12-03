using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BuffSource
{
    public static readonly BuffSource Potion = new BuffSource("Potion");
    public static readonly BuffSource Spell = new BuffSource("Spell");
    public static readonly BuffSource Amulet = new BuffSource("Amulet");
    public static readonly BuffSource Ring = new BuffSource("Ring");

    public string name;

    public BuffSource(string name)
    {
        this.name = name;
    }
}