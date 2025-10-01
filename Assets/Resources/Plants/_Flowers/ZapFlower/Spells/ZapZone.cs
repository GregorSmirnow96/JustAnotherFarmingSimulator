using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ZapZone : MonoBehaviour
{
    public float startTimeOfDay = 18f;
    public float endTimeOfDay = 6f;
    public float lightFadeDuration = 1f;
    public float maxLightIntensity = 0.6f;
    public float zapFrequency = 2f;
    public GameObject zapPrefab;

    private ParticleSystem particleSystem;
    private List<ParticleSystem.MainModule> mainModules;
    private Clock worldClock;
    private List<Health> creaturesInRadius;
    private Light light;
    private Dictionary<Health, float> previousZapTimes;

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

        creaturesInRadius = new List<Health>();
        previousZapTimes = new Dictionary<Health, float>();

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
            List<Health> healthsToDelete = new List<Health>();
            creaturesInRadius.ForEach(health =>
            {
                try
                {
                    if (!previousZapTimes.ContainsKey(health))
                    {
                        previousZapTimes.Add(health, 0f);
                    }

                    float lastZapTime = previousZapTimes[health];
                    if (Time.time >= lastZapTime + zapFrequency)
                    {
                        Vector3 spawnLocation = health.gameObject.transform.position;
                        spawnLocation.y = SceneProperties.TerrainHeightAtPosition(health.gameObject.transform.position) + 0.12f;
                        Instantiate(zapPrefab, spawnLocation, Quaternion.identity);
                        previousZapTimes[health] = Time.time;
                    }
                }
                catch
                {
                    healthsToDelete.Add(health);
                }
            });

            healthsToDelete.ForEach(health =>
            {
                creaturesInRadius.Remove(health);
                previousZapTimes.Remove(health);
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

        if (transform.IsChildOf(collidedObject.transform)) return;

        Health health = collidedObject.GetComponent<Health>();
        if (health != null)
        {
            creaturesInRadius.Add(health);
        }
    }

    void OnTriggerExit(Collider other)
    {
        GameObject collidedObject = other.gameObject;

        Health health = collidedObject.GetComponent<Health>();
        if (health != null)
        {
            creaturesInRadius.Remove(health);
        }
    }
}
