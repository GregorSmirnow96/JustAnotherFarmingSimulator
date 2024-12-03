using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using ItemMetaData;

public class PotionItem : Item
{
    public Dictionary<string, int> propertyStrengths;
    public Color potionColor;

    public PotionItem(
        ItemType itemType,
        Dictionary<string, int> propertyStrengths) : base(itemType)
    {
        this.propertyStrengths = propertyStrengths;
        SetColor();
    }

    public PotionItem(
        string itemId,
        Dictionary<string, int> propertyStrengths) : base(itemId)
    {
        this.propertyStrengths = propertyStrengths;
        SetColor();
    }

    private void SetColor()
    {
        Sprite sprite = type.inventorySprite;
        Texture2D texture = sprite.texture;
        
        Texture2D newTexture = new Texture2D(texture.width, texture.height, TextureFormat.RGBA32, false);

        Color[] pixels = texture.GetPixels();
        CalculateColor();
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = new Color(
                pixels[i].r * potionColor.r,
                pixels[i].g * potionColor.g,
                pixels[i].b * potionColor.b,
                pixels[i].a);
        }

        newTexture.SetPixels(pixels);
        newTexture.Apply();
        Sprite newSprite = Sprite.Create(newTexture, sprite.rect, new Vector2(0.5f, 0.5f));
        type.inventorySprite = newSprite;
    }
    private void CalculateColor()
    {
        List<Vector3> colors = propertyStrengths.Keys.Select(PotionProperty.GetPropertyHSV).ToList();
        float totalH = 0f, totalS = 0f, totalV = 0f;
        int count = colors.Count;

        foreach (Vector3 hsvVector in colors)
        {
            totalH += hsvVector.x;
            totalS += hsvVector.y;
            totalV += hsvVector.z;
        }

        Debug.Log("HEY GREG!!!!!! Don't forget to fix the bug where potions are appearing black. This happens when you make a potion with the WaterDamage property. Make it again and start from there.");
        potionColor = count > 0
            ? Color.HSVToRGB(totalH/count, totalS/count, totalV/count)
            : Color.HSVToRGB(188f/255f, 0.02f, 0.5f);
    }

    public override string GetTooltipText()
    {
        if (propertyStrengths.Keys.Count() == 0)
        {
            return "No useful effects.";
        }
        else
        {
            return "Effects:\n\n" + propertyStrengths
                .Select(kvp => kvp.Value > 0 ? $"{kvp.Key} +{kvp.Value}" : kvp.Key)
                .Aggregate((s1, s2) => $"{s1}\n{s2}");
        }
    }
}
