using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    private Light celestialLight;
    private Transform celestialTransform;
    public Color dawnColor = new Color(255f / 255f, 255f / 255f, 200f / 255f);
    public Color noonColor = new Color(255f / 255f, 237f / 255f, 186f / 255f);
    public Color duskColor = new Color(121f / 255f, 13f / 255f, 20f / 255f);
    public Color midnightColor = new Color(37f / 255f, 29f / 255f, 58f / 255f);
    public float dawnIntensity = 0.2f;
    public float noonIntensity = 1.2f;
    public float duskIntensity = 0.2f;
    public float midnightIntensity = -2.0f;
    private Color[] colorSequence;
    private float[] intensitySequence;

    private float time;
    private float fastforwardScale;
    private float phaseDuration => Clock.globalClock.dayDuration / colorSequence.Length;

    private int phaseIndex = 0;
    private float previousPhaseProgress = 0.0f;
    private Color currentColor => colorSequence[phaseIndex];
    private Color nextColor => colorSequence[(phaseIndex + 1) % colorSequence.Length];
    private float currentIntensity => intensitySequence[phaseIndex];
    private float nextIntensity => intensitySequence[(phaseIndex + 1) % intensitySequence.Length];
    public float rotationOffset = 0.0f;


    void Start()
    {
        celestialLight = gameObject.GetComponent<Light>();
        celestialTransform = gameObject.GetComponent<Transform>();
        colorSequence = new Color[] { dawnColor, noonColor, duskColor, midnightColor };
        intensitySequence = new float[] { dawnIntensity, noonIntensity, duskIntensity, midnightIntensity };
    }

    void Update()
    {
        time = Clock.globalClock.time;

        float phaseProgress = ProgressToNextPhase();

        if (phaseProgress < previousPhaseProgress)
        {
            phaseIndex = (phaseIndex + 1) % 4;
        }

        float t = Mathf.SmoothStep(0f, 1f, phaseProgress);
        Color celestialColor = Color.Lerp(currentColor, nextColor, t);
        celestialLight.color = celestialColor;
        float celestialIntensity = Mathf.Lerp(currentIntensity, nextIntensity, t);
        celestialLight.intensity = celestialIntensity <= 0 ? 0 : (float)Math.Sqrt(celestialIntensity);
        float rotation = time * 360f / Clock.globalClock.dayDuration;
        celestialTransform.eulerAngles = new Vector3(
            rotation + rotationOffset,
            celestialTransform.rotation.y,
            celestialTransform.rotation.z);

        previousPhaseProgress = phaseProgress;
        Shader.SetGlobalColor("_Color", celestialLight.color);
        Shader.SetGlobalFloat("_Intensity", celestialLight.intensity);
    }

    private float ProgressToNextPhase()
    {
        float dayTime = time % Clock.globalClock.dayDuration;
        float phaseTime = dayTime % phaseDuration;
        float phaseProgress = phaseTime / phaseDuration;

        return phaseProgress;
    }
}
