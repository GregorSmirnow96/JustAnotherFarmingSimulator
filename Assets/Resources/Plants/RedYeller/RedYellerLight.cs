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
    private PlantStageGrowth growthScript;

    void Start()
    {
    }

    void Update()
    {
    }
}
