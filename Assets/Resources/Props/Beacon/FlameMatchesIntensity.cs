using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameMatchesIntensity : MonoBehaviour
{
    public ParticleSystem flameParticleSystem;
    public ParticleSystem emberParticleSystem;
    public Light light;

    private float initialIntensity;

    void Start()
    {
        initialIntensity = light.intensity;
    }

    void Update()
    {
        float intensity = light.intensity;

        var flameMain = flameParticleSystem.main;
        flameMain.startLifetime = intensity / initialIntensity;

        var emberMain = emberParticleSystem.main;
        emberMain.startLifetime = intensity / initialIntensity;
    }
}
