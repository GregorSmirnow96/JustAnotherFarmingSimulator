using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelestialRockGlow : MonoBehaviour
{
    public Material crystalMaterial;
    // public AnimationCurve colorCurve;
    public Color moonstoneColor;
    public Color moonstoneEmissionColor;
    public Color sunstalColor;
    public Color sunstalEmissionColor;

    private Clock globalClock;

    void Start()
    {
        globalClock = Clock.globalClock;
    }

    void Update()
    {
        float t = GetColorLerpT();
        Color crystalColor = Color.Lerp(moonstoneColor, sunstalColor, t);
        Color emissionColor = Color.Lerp(moonstoneEmissionColor, sunstalEmissionColor, t);

        crystalMaterial.SetColor("_AlbedoColor", crystalColor);
        crystalMaterial.SetColor("_EmissionColor", emissionColor);
    }

    private float GetColorLerpT()
    {
        const float moonstoneToSunstalStart = 0.22f;
        const float moonstoneToSunstalEnd = 0.28f;
        const float sunstalToMoonstoneStart = 0.72f;
        const float sunstalToMoonstoneEnd = 0.78f;

        float percentThroughDay = (globalClock.time % globalClock.dayDuration) / globalClock.dayDuration;

        if (percentThroughDay >= 0f && percentThroughDay < moonstoneToSunstalStart)
        {
            return 0f;
        }
        else if (percentThroughDay >= moonstoneToSunstalStart && percentThroughDay < moonstoneToSunstalEnd)
        {
            float transitionDuration = moonstoneToSunstalEnd - moonstoneToSunstalStart;
            float t = (percentThroughDay - moonstoneToSunstalStart) / transitionDuration;
            return Mathf.Lerp(0f, 1f, t);
        }
        else if (percentThroughDay >= moonstoneToSunstalEnd && percentThroughDay < sunstalToMoonstoneStart)
        {
            return 1;
        }
        else if (percentThroughDay >= sunstalToMoonstoneStart && percentThroughDay < sunstalToMoonstoneEnd)
        {
            float transitionDuration = sunstalToMoonstoneEnd - sunstalToMoonstoneStart;
            float t = (percentThroughDay - sunstalToMoonstoneStart) / transitionDuration;
            return Mathf.Lerp(1f, 0f, t);
        }
        else if (percentThroughDay >= sunstalToMoonstoneEnd && percentThroughDay <= 1f)
        {
            return 0;
        }

        return 0;
    }
}
