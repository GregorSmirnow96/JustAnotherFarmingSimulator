using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButterflyGroup : MonoBehaviour
{
    public float maxIntensity;
    public float minIntensity;
    public float period;
    public Light light;
    public string wingItemName;

    private bool dimming;

    void Update()
    {
        if (dimming)
        {
            light.intensity = light.intensity - Time.deltaTime / period;
        }
        else
        {
            light.intensity = light.intensity + Time.deltaTime / period;
        }

        if (light.intensity <= minIntensity)
        {
            dimming = false;
        }
        else if (light.intensity >= maxIntensity)
        {
            dimming = true;
        }
    }
}
