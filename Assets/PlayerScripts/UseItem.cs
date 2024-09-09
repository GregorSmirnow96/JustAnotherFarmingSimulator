using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseItem : MonoBehaviour
{
    void Start()
    {
        PlayerInputs.GetInstance().RegisterGameplayAction(
            PlayerInputs.USE_ITEM,
            UseEquippedItem);
    }

    private void UseEquippedItem()
    {
        GameObject renderedItem = RenderEquippedItem.instance.renderedItem;
        if (renderedItem == null) return;

        IUsable usableItem = renderedItem.GetComponent<IUsable>();

        if (usableItem == null)
        {
            usableItem = renderedItem.GetComponentInChildren<IUsable>();
        }

        if (usableItem != null)
        {
            usableItem.Use();
        }
    }
}
