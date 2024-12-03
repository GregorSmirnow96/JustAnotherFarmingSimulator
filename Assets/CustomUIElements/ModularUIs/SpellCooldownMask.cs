using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellCooldownMask : MonoBehaviour
{
    private SpellCooldownMask cooldownMask;
    private RectTransform parentRectTransform;
    private RectTransform rectTransform;
    private Image maskImage;
    private float heightScale;

    void Start()
    {
        cooldownMask = GetComponent<SpellCooldownMask>();
        rectTransform = GetComponent<RectTransform>();
        parentRectTransform = rectTransform.parent.GetComponent<RectTransform>();
        heightScale = 1f;

        maskImage = GetComponent<Image>();
        SetEnabled(false);
    }

    void Update()
    {
        float parentHeight = parentRectTransform.sizeDelta.y;
        rectTransform.sizeDelta = new Vector2(0f, parentHeight * heightScale);
    }

    public void SetEnabled(bool enabled)
    {
        maskImage.enabled = enabled;
    }

    public void SetSize(float newScale)
    {
        heightScale = newScale;
    }
}
