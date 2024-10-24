using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using ItemMetaData;

public class ToolbarImage : MonoBehaviour
{
    public int slotIndex;

    private Image slotImage;
    private Image slotTileImage;
    private Toolbar toolbar;
    private ItemTypeRepo itemTypeRepo;

    void Start()
    {
        slotImage = GetComponent<Image>();
        slotTileImage = transform.parent.gameObject.GetComponent<Image>();
        toolbar = Toolbar.instance;
        itemTypeRepo = ItemTypeRepo.GetInstance();
    }

    void Update()
    {
        Item item = toolbar.GetItem(slotIndex);
        ItemType itemType = itemTypeRepo.TryFindItemType(item?.type.id);
        slotImage.sprite = itemType?.inventorySprite;

        bool hasItem = item != null;
        if (slotIndex == toolbar.equippedItemIndex)
        {
            SetAlphas(true, hasItem);
        }
        else
        {
            SetAlphas(false, hasItem);
        }
    }

    void SetAlphas(bool isEquipped, bool hasItem)
    {
        const float equippedTileAlpha = 255.0f / 255.0f;
        const float unequippedTileAlpha = 176.0f / 255.0f;
        const float equippedItemAlpha = 229.5f / 255.0f;
        const float unequippedItemAlpha = 176.0f / 255.0f;

        Color spriteColor = slotImage.color;
        spriteColor.a =
            hasItem
                ? isEquipped
                    ? equippedItemAlpha
                    : unequippedItemAlpha
                : 0;
        slotImage.color = spriteColor;

        Color tileColor = slotTileImage.color;
        tileColor.a = isEquipped ? equippedTileAlpha : unequippedTileAlpha;
        slotTileImage.color = tileColor;
    }
}
