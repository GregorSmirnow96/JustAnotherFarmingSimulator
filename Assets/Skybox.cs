using System;
using UnityEngine;

public class Skybox : MonoBehaviour
{
    public Material skyboxBlenderMaterial;
    [Range(0, 1)] public float blendValue = 0f;
    public bool maintainNightBox = false;

    private float sunRiseStart = 6f;
    private float sunRiseEnd = 8f;
    private float sunSetStart = 18f;
    private float sunSetEnd = 19f;

    void Start()
    {
        RenderSettings.skybox = skyboxBlenderMaterial;
    }

    void Update()
    {
        float time = Clock.globalClock.time % 24;
        
        if (time >= sunRiseStart && time <= sunRiseEnd)
        {
            blendValue = 1 - (time - sunRiseStart) / (sunRiseEnd - sunRiseStart);
        }
        else if (time >= sunSetStart && time <= sunSetEnd)
        {
            blendValue = (time - sunSetStart) / (sunSetEnd - sunSetStart);
        }

        //blendValue = Math.Abs((time) - 12) / 12;
        
        if (maintainNightBox)
        {
            blendValue = 1f;
        }

        skyboxBlenderMaterial.SetFloat("_Blend", blendValue);
        DynamicGI.UpdateEnvironment();
    }
}
