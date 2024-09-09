using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingBench : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Debug.Log("Interact with crafting bench");
    }

    public void ShowIndicator()
    {
        Debug.Log("Show indicator");
    }

    public void HideIndicator()
    {
        Debug.Log("Hide indicator");
    }
}
