using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GridLayoutPretty : MonoBehaviour
{
    public int gridWidth;
    public int gridHeight;
    public float horizontalMarginPercent;
    public float verticalMarginPercent;
    public float horizontalSpacing;
    public float verticalSpacing;

    void Update()
    {
        //if (!Application.isPlaying)
        //{
            RepositionChildren();
        //}
    }

    private void RepositionChildren()
    {
        if (gridWidth * gridHeight == 0)
        {
            Debug.Log($"Grid height and width must be > 0. Actual dimensions: ({gridWidth}, {gridHeight})");
            return;
        }

        RectTransform rectTransform = GetComponent<RectTransform>();
        float containerWidth = rectTransform.rect.width;
        float containerHeight = rectTransform.rect.height;

        float horizontalMarginPixels = horizontalMarginPercent * containerWidth;
        float verticalMarginPixels = verticalMarginPercent * containerHeight;
        float horizontalMarginTotalPixels = horizontalMarginPixels * 2;
        float verticalMarginTotalPixels = verticalMarginPixels * 2;

        float horizontalSpacingPixels = horizontalSpacing * containerWidth;
        float verticalSpacingPixels = verticalSpacing * containerHeight;

        float childWidth = (containerWidth - horizontalMarginTotalPixels) / gridWidth;
        float childHeight = (containerHeight - verticalMarginTotalPixels) / gridHeight;

        float childOffsetY = childHeight / 2;
        float childOffsetX = childWidth / 2;
        
        float smallestDimension = Mathf.Min(childWidth, childHeight);

        int xIndex = 0;
        int yIndex = 0;
        foreach (RectTransform child in transform)
        {
            float currentHorizontalImagePixels = (xIndex + 0.5f) * childWidth;
            float x = horizontalMarginPixels + currentHorizontalImagePixels;

            float currentVerticalImagePixels = (yIndex + 0.5f) * childHeight;
            float y = verticalMarginPixels + currentVerticalImagePixels;

            child.anchoredPosition = new Vector2(x, -y);
            float spacing = Mathf.Min(horizontalSpacingPixels, verticalSpacingPixels);
            child.sizeDelta = new Vector2(smallestDimension - spacing, smallestDimension - spacing);

            xIndex = (xIndex + 1) % gridWidth;
            if (xIndex == 0) yIndex++;
        }
    }
}
