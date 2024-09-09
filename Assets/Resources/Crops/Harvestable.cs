using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harvestable : MonoBehaviour, IInteractable
{
    public string itemId;
    public bool hasContainer = true;

    private HarvestText interactionIndicatorScript;

    void Start()
    {
        interactionIndicatorScript = gameObject.AddComponent<HarvestText>();
    }

    public void Interact()
    {
        Inventory.instance.AddItemToInventory(itemId);
        if (hasContainer)
        {
            Destroy(gameObject.transform.parent.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowIndicator()
    {
        interactionIndicatorScript.Enable();
    }

    public void HideIndicator()
    {
        interactionIndicatorScript.Disable();
    }
}
