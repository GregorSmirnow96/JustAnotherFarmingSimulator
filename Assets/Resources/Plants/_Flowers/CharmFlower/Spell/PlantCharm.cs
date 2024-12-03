using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlantCharm : MonoBehaviour
{
    public float startTimeOfDay = 18f;
    public float endTimeOfDay = 6f;
    public float charmDuration = 6f;
    public float lightFadeDuration = 1f;
    public float maxLightIntensity = 0.6f;

    private ParticleSystem particleSystem;
    private List<ParticleSystem.MainModule> mainModules;
    private Clock worldClock;
    private List<ICCable> charmableCreaturesInRadius;
    private List<ICCable> recentlyCharmedCreatures;
    private Light light;

    void Awake()
    {
        particleSystem = GetComponent<ParticleSystem>();

        mainModules = new List<ParticleSystem.MainModule>();
        mainModules.Add(particleSystem.main);

        foreach (Transform childTransform in transform)
        {
            ParticleSystem childParticleSystem = childTransform.GetComponent<ParticleSystem>();
            if (childParticleSystem != null)
            {
                mainModules.Add(childParticleSystem.main);
            }
            else
            {
                light = childTransform.GetComponent<Light>();
            }
        }

        charmableCreaturesInRadius = new List<ICCable>();
        recentlyCharmedCreatures = new List<ICCable>();

        SetLooping(false);
    }

    void Start()
    {
        worldClock = Clock.globalClock;

        float timeOfDay = worldClock.time % worldClock.dayDuration;
        bool isPastStartTime = timeOfDay >= startTimeOfDay || timeOfDay <= endTimeOfDay;
        if (isPastStartTime)
        {
            SetLooping(true);
        }
    }

    void Update()
    {
        bool isStartTime = (worldClock.time % worldClock.dayDuration) >= startTimeOfDay;
        bool wasStartTime = (worldClock.previousTime % worldClock.dayDuration) >= startTimeOfDay;
        if (isStartTime && !wasStartTime)
        {
            SetLooping(true);
        }

        bool isEndTime = (worldClock.time % worldClock.dayDuration) >= endTimeOfDay;
        bool wasEndTime = (worldClock.previousTime % worldClock.dayDuration) >= endTimeOfDay;
        if (isEndTime && !wasEndTime)
        {
            SetLooping(false);
        }

        bool isActive = particleSystem.main.loop;
        if (isActive)
        {
            List<ICCable> ccablesToDelete = new List<ICCable>();

            charmableCreaturesInRadius.ForEach(ccable =>
            {
                try
                {
                    if (!recentlyCharmedCreatures.Contains(ccable))
                    {
                        ccable.Charm(charmDuration, transform);
                        recentlyCharmedCreatures.Add(ccable);
                    }
                }
                catch
                {
                    ccablesToDelete.Add(ccable);
                }
            });

            ccablesToDelete.ForEach(ccable =>
            {
                charmableCreaturesInRadius.Remove(ccable);
                recentlyCharmedCreatures.Remove(ccable);
            });
        }
    }

    private void SetLooping(bool shouldLoop)
    {
        bool wasLooping = particleSystem.main.loop;

        mainModules.ForEach(main => main.loop = shouldLoop);

        if (shouldLoop && !wasLooping)
        {
            StartCoroutine(TurnOnLight());
            particleSystem.Play();
        }
        else if (!shouldLoop && wasLooping)
        {
            recentlyCharmedCreatures.Clear();
            StartCoroutine(TurnOffLight());
            particleSystem.Stop();
        }
    }

    private IEnumerator TurnOnLight()
    {
        float fadeTimer = 0f;
        while (fadeTimer <= lightFadeDuration)
        {
            float lightScale = fadeTimer / lightFadeDuration;
            light.intensity = lightScale * maxLightIntensity;

            yield return null;

            fadeTimer += Time.deltaTime;
        }

        light.intensity = maxLightIntensity;
    }

    private IEnumerator TurnOffLight()
    {
        float fadeTimer = 0f;
        while (fadeTimer <= lightFadeDuration)
        {
            float lightScale = 1 - fadeTimer / lightFadeDuration;
            light.intensity = lightScale * maxLightIntensity;

            yield return null;

            fadeTimer += Time.deltaTime;
        }

        light.intensity = 0f;
    }

    void OnTriggerEnter(Collider other)
    {
        GameObject collidedObject = other.gameObject;

        ICCable ccable = collidedObject.GetComponent<ICCable>();
        if (ccable != null)
        {
            charmableCreaturesInRadius.Add(ccable);
        }
    }

    void OnTriggerExit(Collider other)
    {
        GameObject collidedObject = other.gameObject;

        ICCable ccable = collidedObject.GetComponent<ICCable>();
        if (ccable != null)
        {
            charmableCreaturesInRadius.Remove(ccable);
        }
    }
}
