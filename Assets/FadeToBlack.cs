using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeToBlack : MonoBehaviour
{
    public static FadeToBlack instance;

    public float fadeDuration = 2f;

    private Image image;

    void Start()
    {
        instance = this;

        image = GetComponent<Image>();
    }

    public void StartFading()
    {
        StartCoroutine(Fade());
    }

    private IEnumerator Fade()
    {
        float timer = 0;
        while (timer <= fadeDuration)
        {
            Color newColor = image.color;
            newColor.a = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            image.color = newColor;

            timer += Time.deltaTime;

            yield return null;
        }
    }
}
