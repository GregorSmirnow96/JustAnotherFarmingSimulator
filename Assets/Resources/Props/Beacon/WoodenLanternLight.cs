using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WoodenLanternLight : MonoBehaviour
{
    private LightSource lightSource;
    private float initialLightSourceIntensity = 1.2f;

    void Start()
    {
        lightSource = GetComponent<LightSource>();
        lightSource.UpdateLightIntensity(initialLightSourceIntensity);
    }
}
