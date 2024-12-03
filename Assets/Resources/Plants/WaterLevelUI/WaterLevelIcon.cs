using UnityEngine;
using UnityEngine.UI;

public class WaterLevelIcon : MonoBehaviour
{
    public RectTransform waterMaskTransform;
    public RectTransform waterImageTransform;
    public float waterLevel = 0.0f;
    public Transform waterableTransform;
    public GameObject iconBackgroundObject;
    public GameObject iconForegroundObject;

    private float originalImageHeight;
    private Vector2 originalImagePosition;
    private RectTransform iconContainerRectTransform;
    private Image iconBackground;
    private Image iconForeground;
    private bool showIndicator;

    void Start()
    {
        originalImageHeight = waterImageTransform.rect.height;
        originalImagePosition = waterImageTransform.anchoredPosition;

        iconContainerRectTransform = GetComponent<RectTransform>();

        iconBackground = iconBackgroundObject.GetComponent<Image>();
        iconForeground = iconForegroundObject.GetComponent<Image>();
    }

    void Update()
    {
        Vector3 worldOffset = new Vector3(0, 0.75f, 0);
        Vector3 worldPosition = waterableTransform.position + worldOffset;
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);
        iconContainerRectTransform.position = screenPosition;

        float maskOffset = originalImagePosition.y - (originalImageHeight * (1 - waterLevel));
        waterMaskTransform.anchoredPosition = new Vector2(originalImagePosition.x, maskOffset);
        waterImageTransform.anchoredPosition = new Vector2(originalImagePosition.x, -maskOffset);

        iconBackground.enabled = showIndicator;
        iconForeground.enabled = showIndicator;
    }

    public void ShowIndicator()
    {
        showIndicator = true;
    }

    public void HideIndicator()
    {
        showIndicator = false;
    }
}
