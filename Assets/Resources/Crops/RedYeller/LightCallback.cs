using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightCallback : MonoBehaviour
{
    public GameObject lightSourceObject;
    private LightSource lightSource;
    private Light light;
    private Action<float> intensityCallback;

    void Start()
    {
        lightSource = lightSourceObject.GetComponent<LightSource>();
        light = GetComponent<Light>();
        intensityCallback = AdjustLightIntensity;
        lightSource.RegisterIntensityCallback(intensityCallback);
    }

    void AdjustLightIntensity(float newIntensity) => light.intensity = newIntensity;

    void OnDestroy() => lightSource.RemoveIntensityCallback(intensityCallback);
}
