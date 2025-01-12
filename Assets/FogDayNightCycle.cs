using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AtmosphericHeightFog;

public class FogDayNightCycle : MonoBehaviour
{
    // Group 1 (Fog Color)
    public AnimationCurve fogColorCurve;
	public Color dayFogColorStart;
	public Color nightFogColorStart;
	public Color dayFogColorEnd;
	public Color nightFogColorEnd;
    public Color dayDirectionalColor;
    public Color nightDirectionalColor;
    public float dayDirectionalIntensity;
    public float nightDirectionalIntensity;

    // Group 2 (Fog Distance)
    public AnimationCurve fogDensityCurve;
    public float dayFogDistanceEnd;
    public float nightFogDistanceEnd;
    public float dayFogHeightEnd;
    public float nightFogHeightEnd;
    public float dayNoiseDistanceEnd;
    public float nightNoiseDistanceEnd;

    // Group 3 (Skybox)
    public AnimationCurve skyboxFogCurve;
    public float daySkyboxFogHeight;
    public float nightSkyboxFogHeight;

    private HeightFogGlobal fogScript;
    private Clock clock;

    void Start()
    {
        fogScript = GetComponent<HeightFogGlobal>();
        clock = Clock.globalClock;
    }

    void Update()
    {
        float timeOfDay = clock.time % clock.dayDuration;
        float t = timeOfDay / clock.dayDuration;

        float fogColorInterpPercent = fogColorCurve.Evaluate(t);
        fogScript.fogColorStart = Color.Lerp(nightFogColorStart, dayFogColorStart, fogColorInterpPercent).linear;
        fogScript.fogColorEnd = Color.Lerp(nightFogColorEnd, dayFogColorEnd, fogColorInterpPercent).linear;
        fogScript.directionalColor = Color.Lerp(nightDirectionalColor, dayDirectionalColor, fogColorInterpPercent).linear;
        fogScript.directionalIntensity = Mathf.Lerp(nightDirectionalIntensity, dayDirectionalIntensity, fogColorInterpPercent);

        float fogDensityInterpPercent = fogDensityCurve.Evaluate(t);
        fogScript.fogDistanceEnd = Mathf.Lerp(nightFogDistanceEnd, dayFogDistanceEnd, fogDensityInterpPercent);
        fogScript.fogHeightEnd = Mathf.Lerp(nightFogHeightEnd, dayFogHeightEnd, fogDensityInterpPercent);
        fogScript.noiseDistanceEnd = Mathf.Lerp(nightNoiseDistanceEnd, dayNoiseDistanceEnd, fogDensityInterpPercent);

        float skyboxFogInterpPercent = skyboxFogCurve.Evaluate(t);
        fogScript.skyboxFogHeight = Mathf.Lerp(nightSkyboxFogHeight, daySkyboxFogHeight, skyboxFogInterpPercent);
    }
}
