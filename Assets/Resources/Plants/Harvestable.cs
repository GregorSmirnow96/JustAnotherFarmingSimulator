using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harvestable : MonoBehaviour, IInteractable
{
    public string itemId;
    public bool regrows = false;
    public GameObject harvestedPrefab;

    private ModularInteractText interactText;

    void Start()
    {
        interactText = GetComponent<ModularInteractText>();
        // interactText.SetText("'E' to Harvest");
    }

    public void Interact()
    {
        Item harvestedItem = new Item(itemId);
        Inventory.instance.AddItemToInventory(harvestedItem);

        if (regrows && harvestedPrefab != null)
        {
            // Instantiate(harvestedPrefab, transform.position, transform.rotation);
            Instantiate(harvestedPrefab, transform.parent);
        }

        Destroy(gameObject);
    }

    public void ShowIndicator()
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

    public void HideIndicator()
    {
        if (interactText != null)
        {
            interactText.Disable();
        }
    }

    void OnDestroy()
    {
        if (!regrows)
        {
            Destroy(gameObject.transform.parent.gameObject);
        }
        if (interactText != null)
        {
            Destroy(interactText.textObject);
        }
    }
}
