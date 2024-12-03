using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SpellCDPanel : MonoBehaviour
{
    public RectTransform panelRectTransform;

    private List<RectTransform> cooldownSpriteTransforms = new List<RectTransform>();
    private Image panelImage;
    private ISpellProvider spellProvider;
    private bool spellsChanged;

    void Start()
    {
        panelImage = GetComponent<Image>();

        RenderEquippedItem.instance.RegisterCallback(item => SetSpellSprites(item));
    }

    private void SetSpellSprites(GameObject item)
    {
        ClearSprites();

        GameObject equippedItem = RenderEquippedItem.instance.renderedItem;
        spellProvider = equippedItem?.GetComponent<ISpellProvider>();

        if (spellProvider != null)
        {
            spellProvider.GetSpellSprites().ForEach(sprite =>
            {
                Image cooldownSprite = Instantiate(sprite, panelRectTransform);
                RectTransform spriteRectTransform = cooldownSprite.GetComponent<RectTransform>();
                cooldownSpriteTransforms.Add(spriteRectTransform);
                cooldownSprite.enabled = false;
            });
        }

        spellsChanged = true;
        panelImage.enabled = false;
    }

    private void ClearSprites()
    {
        cooldownSpriteTransforms.ForEach(spriteTransform =>
        {
            Destroy(spriteTransform.gameObject);
        });
        cooldownSpriteTransforms = new List<RectTransform>();
    }

    void Update()
    {
        if (spellsChanged || true)
        {
            cooldownSpriteTransforms.ForEach(spriteTransform =>
            {
                Vector2 sizeDelta = spriteTransform.sizeDelta;
                sizeDelta.x = sizeDelta.y;
                spriteTransform.sizeDelta = sizeDelta;
            });

            UpdateCooldownMasks();

            LayoutRebuilder.ForceRebuildLayoutImmediate(panelRectTransform);

            if (spellsChanged)
            {
                cooldownSpriteTransforms.ForEach(spriteTransform =>
                {
                    spriteTransform.GetComponent<Image>().enabled = true;
                });

                bool hasChildren = transform.childCount > 0;
                panelImage.enabled = hasChildren;

                spellsChanged = false;
            }
        }
    }

    private void UpdateCooldownMasks()
    {
        if (spellProvider == null) return;

        List<float> cooldowns = spellProvider.GetSpellCooldowns();
        List<float> remainingCooldowns = spellProvider.GetRemainingSpellCooldowns();
        for (int spellIndex = 0; spellIndex < cooldownSpriteTransforms.Count; spellIndex++)
        {
            float cooldown = cooldowns.ElementAt(spellIndex);
            float remainingCooldown = remainingCooldowns.ElementAt(spellIndex);
            float remainingCooldownPercent = remainingCooldown / cooldown;

            RectTransform spellSpriteTransform = cooldownSpriteTransforms.ElementAt(spellIndex);
            SpellCooldownMask cooldownMask = spellSpriteTransform.GetChild(0).GetComponent<SpellCooldownMask>();
            cooldownMask.SetSize(remainingCooldownPercent);
            cooldownMask.SetEnabled(true);
        }
    }
}
