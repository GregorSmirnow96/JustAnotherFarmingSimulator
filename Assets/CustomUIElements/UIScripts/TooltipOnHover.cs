using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Item item;

    private Tooltip tooltip;

    private void Start()
    {
        GameObject rootCanvasObject = GameObject.Find("UICanvas");
        tooltip = rootCanvasObject.transform.Find("Tooltip").GetComponent<Tooltip>();
    }
    void OnDisable()
    {
        tooltip?.HideTooltip();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log($"Hello!!! from tooltip on hover --- {gameObject.name}");
        if (item != null)
        {
            tooltip.transform.SetAsLastSibling();
            tooltip.ShowTooltip(item.GetTooltipText());
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.HideTooltip();
    }
}
