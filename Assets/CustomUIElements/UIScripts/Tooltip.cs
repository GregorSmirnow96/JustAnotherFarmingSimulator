using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    public TMP_Text tooltipText;
    public RectTransform backgroundRectTransform;

    private void Awake()
    {
        HideTooltip();
    }

    public void ShowTooltip(string description)
    {
        tooltipText.text = description;
        SetPosition();
        LayoutRebuilder.ForceRebuildLayoutImmediate(backgroundRectTransform);
        gameObject.SetActive(true);
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }

    void Update()
    {
        SetPosition();
    }

    private void SetPosition()
    {
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)transform.parent,
            Input.mousePosition,
            null,
            out position);
        
        transform.localPosition = position;
    }
}
