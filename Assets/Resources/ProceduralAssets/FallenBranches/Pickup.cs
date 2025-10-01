using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour, IInteractable
{
    public string itemId;

    private ModularInteractText interactText;

    void Start()
    {
        interactText = GetComponent<ModularInteractText>();
        interactText.SetText("'E' to Pick Up");
    }

    public void Interact()
    {
        Item harvestedItem = new Item(itemId);
        Inventory.instance.AddItemToInventory(harvestedItem);

        Destroy(gameObject);
    }

    public void ShowIndicator()
    {
        if (interactText != null)
        {
            interactText.Enable();
        }
    }

    public void HideIndicator()
    {
        if (interactText != null)
        {
            interactText.Disable();
        }
    }

    void OnDestroy()
    {
        if (interactText != null)
        {
            Destroy(interactText.textObject);
        }
    }
}
