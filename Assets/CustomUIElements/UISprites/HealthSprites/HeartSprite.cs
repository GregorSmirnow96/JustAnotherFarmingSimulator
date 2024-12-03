using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartSprite : MonoBehaviour
{
    public Sprite heart4of4Prefab;
    public Sprite heart3of4Prefab;
    public Sprite heart2of4Prefab;
    public Sprite heart1of4Prefab;
    public Sprite heart0of4Prefab;

    private Image heartImage;

    void Start()
    {
        heartImage = GetComponent<Image>();
    }

    public void SetFilledQuarters(int quartersFilled)
    {
        if (quartersFilled < 0 || quartersFilled > 4)
        {
            Debug.Log("Filled quarters must be between 0 and 5.");
            return;
        }

        switch (quartersFilled)
        {
            case 0:
                heartImage.sprite = heart0of4Prefab;
                break;
            case 1:
                heartImage.sprite = heart1of4Prefab;
                break;
            case 2:
                heartImage.sprite = heart2of4Prefab;
                break;
            case 3:
                heartImage.sprite = heart3of4Prefab;
                break;
            case 4:
                heartImage.sprite = heart4of4Prefab;
                break;
        }
    }
}
