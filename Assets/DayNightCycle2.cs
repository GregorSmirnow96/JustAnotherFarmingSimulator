using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle2 : MonoBehaviour
{
    public Quaternion startRotation;
    public Quaternion endRotation;
    public float startTime;
    public float endTime;
    public float intensity;
    public float intensityChangeDuration;

    private Clock clock;
    private GameObject lightSource;
    private Light light;
    private bool lightSourceWasOut;
    private float phaseProgress;
    private float phaseStart;
    private float phaseEnd;
    private float duration;
    private float lastClockTime;

    private float timeOfDay => clock.time % clock.dayDuration;

    void Start()
    {
        lightSource = gameObject;
        light = GetComponent<Light>();
    }

    void Update()
    {
        // This is being done here because globalClock may still be null during Start().
        if (clock == null && Clock.globalClock != null)
        {
            clock = Clock.globalClock;
            lastClockTime = clock.time;

            duration = endTime < startTime
                ? (endTime + clock.dayDuration) - startTime
                : endTime - startTime;
        }

        bool lightSourceIsOut = LightSourceIsOut();

        if (lightSourceIsOut)
        {
            float lightDeltaPerHour = intensity / intensityChangeDuration;
            float intensityDelta = lightDeltaPerHour * (clock.time - lastClockTime);
            float newIntensity = light.intensity + intensityDelta;
            if (newIntensity <= intensity)
            {
                light.intensity = newIntensity;
            }
        }
        else
        {
            float lightDeltaPerHour = intensity / intensityChangeDuration;
            float intensityDelta = lightDeltaPerHour * (clock.time - lastClockTime);
            float newIntensity = light.intensity - intensityDelta;
            light.intensity = newIntensity >= 0
                ? newIntensity
                : 0;
        }

        if (lightSourceIsOut && !lightSourceWasOut)
        {
            lightSourceIsOut = true;
            phaseStart = clock.time - (clock.time % clock.dayDuration) + startTime;
            phaseEnd = phaseStart + duration;
        }

        phaseProgress = (clock.time - phaseStart) / duration;
        if (phaseProgress >= 0 && phaseProgress <= 1)
        {
            lightSource.transform.rotation = Quaternion.Lerp(startRotation, endRotation, phaseProgress);
        }

        lightSourceWasOut = lightSourceIsOut;
        lastClockTime = clock.time;
    }

    private bool LightSourceIsOut()
    {
        bool comesOutAtNight = startTime > endTime;
        return comesOutAtNight
            ? timeOfDay >= startTime || timeOfDay <= endTime
            : timeOfDay >= startTime && timeOfDay <= endTime;
    }
}
