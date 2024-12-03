using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IngredientInfo
{
    public List<string> properties;
    public string boostedProperty;

    public IngredientInfo(IEnumerable<string> properties, string boostedProperty)
    {
        this.properties = properties.ToList();
        this.boostedProperty = boostedProperty;
    }
}
