using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonlightPulse : MonoBehaviour
{
    public float minIntensity;
    public float maxIntensity;
    public float pulsePeriod;

    private Light moonlight;
    private float pulseTimer;

    void Start()
    {
        moonlight = GetComponent<Light>();
        pulseTimer = 0;
    }

    void Update()
    {
        pulseTimer += Time.deltaTime;

        float pulseProgress = pulseTimer % pulsePeriod / pulsePeriod;
        float normalizedSin = Mathf.Sin(pulseProgress * Mathf.PI);
        float intensity = Mathf.Lerp(minIntensity, maxIntensity, normalizedSin);

        moonlight.intensity = intensity;
    }
}
