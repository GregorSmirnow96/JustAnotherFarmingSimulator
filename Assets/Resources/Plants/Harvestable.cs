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

        if (regrows)
        {
            Instantiate(harvestedPrefab, transform.position, transform.rotation);
        }

        Destroy(gameObject);
    }

    public void ShowIndicator()
    {
        interactText.Enable();
    }

    public void HideIndicator()
    {
        interactText.Disable();
    }

    void OnDestroy()
    {
        // TODO: This first Destroy call throws an error.
        //  The gameObject is null (destroyed) already?? But when I don't call Destroy,
        //  the object doesn't get destroyed. If this causes errors later, come back.
        Destroy(transform.parent.gameObject);
        Destroy(interactText.textObject);
    } 
}
