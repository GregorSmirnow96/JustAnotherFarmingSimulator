using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LightSource : MonoBehaviour
{
    public float intensity { get; private set; }
    public float radius;
    public AnimationCurve intensityFlickerCurve;

    public static List<LightSource> worldLights = new List<LightSource>();
    private List<Action<float>> intensityChangeCallbacks = new List<Action<float>>();
    public bool isFlickering = false;

    void Start()
    {
        worldLights.Add(this);
    }

    public void RegisterIntensityCallback(Action<float> newCallback) => intensityChangeCallbacks.Add(newCallback);

    public void RemoveIntensityCallback(Action<float> callback) => intensityChangeCallbacks.Remove(callback);

    public void UpdateLightIntensity(float newIntensity)
    {
        intensity = newIntensity;
        intensityChangeCallbacks.ForEach(callback => callback(newIntensity));
    }

    public bool PlayerIsInRadius()
    {
        return intensity > 0 && PlayerIsInVicinity(radius);
    }
    
    public bool PlayerIsInVicinity(float vicinityRadius)
    {
        float distanceToLight = Vector3.Distance(
            SceneProperties.playerTransform.position,
            transform.position);
        return distanceToLight <= vicinityRadius;
    }

    public void FlickerOff(float fadeOutDuration, float offDuration, float fadeInDuration)
    {
        StartCoroutine(FlickerOffCoroutine(fadeOutDuration, offDuration, fadeInDuration));
    }

    public IEnumerator FlickerOffCoroutine(float fadeOutDuration, float offDuration, float fadeInDuration)
    {
        if (!isFlickering)
        {
            Debug.Log($"Extinguish: ({fadeOutDuration}, {offDuration}, {fadeInDuration}) - Initial Intensity: {intensity}");
            isFlickering = true;

            float initialIntensity = intensity;

            float flickerTimer = 0f;
            float flickerOnTime = fadeOutDuration + offDuration;
            float totalDuration = fadeOutDuration + offDuration + fadeInDuration;

            while (flickerTimer < fadeOutDuration)
            {
                float t = Mathf.Clamp01(flickerTimer / fadeOutDuration);
                float newIntensity = initialIntensity * intensityFlickerCurve.Evaluate(t);
                UpdateLightIntensity(newIntensity);
                yield return null;
                flickerTimer += Time.deltaTime;
            }
            UpdateLightIntensity(0);

            while (flickerTimer < flickerOnTime)
            {
                yield return null;
                flickerTimer += Time.deltaTime;
            }
            
            while (flickerTimer < totalDuration)
            {
                float currentPhaseTime = flickerTimer - flickerOnTime;
                float t = 1 - Mathf.Clamp01(currentPhaseTime / fadeInDuration);
                float newIntensity = initialIntensity * intensityFlickerCurve.Evaluate(t);
                UpdateLightIntensity(newIntensity);
                yield return null;
                flickerTimer += Time.deltaTime;
            }
            UpdateLightIntensity(initialIntensity);

            isFlickering = false;
        }
    }
}
