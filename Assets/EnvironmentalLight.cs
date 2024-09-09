using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentalLight : MonoBehaviour
{
    public GameObject sun;

    private Light light;

    void Start()
    {
        light = sun.GetComponent<Light>();
    }

    void Update()
    {
        /*
        float peakLightTime = Clock.globalClock.dayDuration / 2f;
        float timeOfDay = Clock.globalClock.time % 24f;
        float distanceToPeak = Math.Abs(peakLightTime - timeOfDay);
        float lightScaler = 1 - distanceToPeak / (Clock.globalClock.dayDuration / 2);
        RenderSettings.ambientIntensity = lightScaler;
        */

        RenderSettings.ambientIntensity = light.intensity;
    }
}
