using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AlchemyBench : MonoBehaviour, IInteractable
{
    private ModularInteractText interactText;

    void Start()
    {
        interactText = GetComponent<ModularInteractText>();
        interactText.SetText("'E' to Make Potions");
    }

    public void Interact()
    {
        UIManager.instance.ActivateAlchemyMenu();
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
