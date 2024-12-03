using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectableButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    private static SelectableButton selectedButton;

    private Image backgroundImage;
    private bool selected;

    private Color selectedColor = new Color(234f/255f, 182f/255f, 142f/255f, 143f/255f);
    private Color hoveredColor = new Color(255f/255f, 239f/255f, 239f/255f, 186f/255f);

    void Start()
    {
        backgroundImage = GetComponent<Image>();
    }

    private void SetColorTint(Color newTint)
    {
        if (backgroundImage == null)
        {
            backgroundImage = GetComponent<Image>();
        }

        Debug.Log(newTint);
        backgroundImage.color = newTint;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (selectedButton != null)
        {
            selectedButton.SetColorTint(Color.white);
            selectedButton.selected = false;
        }

        selected = true;
        selectedButton = this;
        SetColorTint(selectedColor);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!selected)
        {
            SetColorTint(hoveredColor);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!selected)
        {
            SetColorTint(Color.white);
        }
    }
}
