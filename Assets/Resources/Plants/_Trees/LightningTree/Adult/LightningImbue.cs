using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningImbue : MonoBehaviour, IInteractable
{
    private ModularInteractText interactText;
    private Toolbar toolbar;
    private ImbuableItems imbuableItems;

    void Start()
    {
        interactText = GetComponent<ModularInteractText>();
        interactText.SetText("'E' to Imbue");

        toolbar = Toolbar.instance;

        imbuableItems = ImbuableItems.GetInstance();
    }

    public void Interact()
    {
        Item equippedItem = toolbar.GetEquippedItem();
        string equippedItemId = equippedItem?.type.id;
        bool equippedItemIsImbuable = imbuableItems.ItemIdIsLightningImbuable(equippedItemId);

        if (equippedItemIsImbuable)
        {
            string imbuedId = imbuableItems.GetLightningImbuedItemId(equippedItem.type.id);
            if (imbuedId != null)
            {
                int equippedSlotIndex = toolbar.equippedItemIndex;
                toolbar.DeleteEquippedItem();
                Item imbuedItem = new Item(imbuedId); 
                toolbar.SetInventorySlot(equippedSlotIndex, imbuedItem);
            }
        }
    }

    public void ShowIndicator()
    {
        Item equippedItem = toolbar.GetEquippedItem();
        string equippedItemId = equippedItem?.type.id;
        bool equippedItemIsImbuable = imbuableItems.ItemIdIsLightningImbuable(equippedItemId);

        if (!equippedItemIsImbuable)
        {
            HideIndicator();
        }
        else
        {
            if (interactText == null)
            {
                Destroy(gameObject);
            }
            else
            {
                interactText.Enable();
            }
        }
    }

    public void HideIndicator()
    {
        interactText.Disable();
    }

    void OnDestroy()
    {
        Destroy(interactText.textObject);
    } 
}
