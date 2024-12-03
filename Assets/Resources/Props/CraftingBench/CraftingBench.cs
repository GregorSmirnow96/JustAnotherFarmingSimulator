using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingBench : MonoBehaviour, IInteractable
{
    private ModularInteractText interactText;

    void Start()
    {
        interactText = GetComponent<ModularInteractText>();
        interactText.SetText("'E' to Craft");
    }

    public void Interact()
    {
        UIManager.instance.ActivateCraftingMenu();
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
        interactText.Disable();
    }

    void OnDestroy()
    {
        Destroy(interactText.textObject);
    } 
}
