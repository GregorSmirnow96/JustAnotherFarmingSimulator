using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RedYellerLight : MonoBehaviour
{
    ParticleSystem fireflies;
    Dictionary<int, int> fireFliesByTime;
    Dictionary<int, int> lightCountByFireflies;
    LightSource lightSource;
    float initialLightSourceIntensity = 1.2f;
    public AnimationCurve lightIntensityCurve;
    private PlantSizeGrowth growthScript;

    void Start()
    {
        growthScript = GetComponent<PlantSizeGrowth>();

        fireflies = transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>(); 
        fireFliesByTime = new Dictionary<int, int>()
        {
            { 18, 3 },
            { 19, 8 },
            { 20, 15 },
            { 21, 20 },
            { 22, 24 },
            { 23, 24 },
            { 0, 24 },
            { 1, 24 },
            { 2, 24 },
            { 3, 20 },
            { 4, 15 },
            { 5, 8 },
            { 6, 3 }
        };
        lightCountByFireflies = new Dictionary<int, int>()
        {
            { 3, 0 },
            { 8, 1 },
            { 15, 2 },
            { 20, 3 },
            { 24, 4 }
        };
        lightSource = GetComponent<LightSource>();
        lightSource.UpdateLightIntensity(0f);
    }

    void Update()
    {
        float timeOfDay = Clock.globalClock.time % 24;
        if (growthScript.fullyGrown)
        {
            float t = timeOfDay / 24;
            float intensity = lightIntensityCurve.Evaluate(t);
            if (!lightSource.isFlickering)
            {
                lightSource.UpdateLightIntensity(intensity);
            }
        }

        int hour = (int) timeOfDay;
        var main = fireflies.main;
        int maxFireflies = fireFliesByTime.Keys.Contains(hour)
            ? fireFliesByTime[hour]
            : 0;
        main.maxParticles = maxFireflies;
    }
}
