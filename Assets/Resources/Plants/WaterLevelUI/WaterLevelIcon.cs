using UnityEngine;
using UnityEngine.UI;

public class WaterLevelIcon : MonoBehaviour
{
    public RectTransform waterMaskTransform;
    public RectTransform waterImageTransform;
    public float waterLevel = 0.5f;
    public Transform waterableTransform;

    private float originalImageHeight;
    private Vector2 originalImagePosition;
    private RectTransform iconContainerRectTransform;

    void Start()
    {
        originalImageHeight = waterImageTransform.rect.height;
        originalImagePosition = waterImageTransform.anchoredPosition;

        iconContainerRectTransform = GetComponent<RectTransform>();
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
    }
}
