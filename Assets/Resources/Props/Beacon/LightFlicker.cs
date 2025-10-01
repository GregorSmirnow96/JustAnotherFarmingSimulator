using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    public Color color1;
    public Color color2;
    public AnimationCurve flickerCurve;
    public float flickerDuration;

    public Light light;
    private float flickerTimer;

    void Start()
    {
        light = GetComponent<Light>();
        flickerTimer = 0f;
    }

    void Update()
    {
        float t = Mathf.Clamp01(flickerTimer / flickerDuration);
        t = flickerCurve.Evaluate(t);
        light.color = Color.Lerp(color1, color2, t);

        flickerTimer = (flickerTimer + Time.deltaTime) % flickerDuration;
    }
}
