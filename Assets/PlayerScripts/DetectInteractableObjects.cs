using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectInteractableObjects : MonoBehaviour
{
    public float maxInteractionRange = 4;

    private IInteractable previousInteractable;
    private IInteractable currentInteractable;

    void Start()
    {
        PlayerInputs inputs = PlayerInputs.GetInstance();
        inputs.RegisterGameplayAction(PlayerInputs.INTERACT, InteractWithViewedObject);
    }

    void Update()
    {
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, maxInteractionRange))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable == null)
            {
                GameObject hitParent = hit.collider.transform.parent?.gameObject;
                if (hitParent)
                {
                    interactable = hitParent.GetComponent<IInteractable>();
                }
            }

            if (interactable != null)
            {
                interactable.ShowIndicator();
                currentInteractable = interactable;
            }
            else
            {
                currentInteractable = null;
            }
        }
        else
        {
            currentInteractable = null;
        }

        if (previousInteractable != null && currentInteractable != previousInteractable)
        {
            previousInteractable.HideIndicator();
        }

        previousInteractable = currentInteractable;
    }

    private void InteractWithViewedObject()
    {
        if (currentInteractable != null)
        {
            currentInteractable.Interact();
        }
    }
}
