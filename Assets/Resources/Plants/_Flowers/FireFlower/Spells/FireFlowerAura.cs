using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FireFlowerAura : MonoBehaviour
{
    public float startTimeOfDay = 18f;
    public float endTimeOfDay = 6f;
    public float lightFadeDuration = 1f;
    public float maxLightIntensity = 0.6f;
    public GameObject orbPrefab;
    public Vector3 orbSpawnOffset;
    public float minLaunchForce = 6f;
    public float maxLaunchForce = 12f;
    public float minYaw = 45f;
    public float maxYaw = 60f;
    public float minLaunchFrequency = 1f;
    public float maxLaunchFrequency = 1.6f;

    private ParticleSystem particleSystem;
    private List<ParticleSystem.MainModule> mainModules;
    private Clock worldClock;
    private Light light;
    private float lastLaunchTime;
    private float launchFrequency;

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

        SetLooping(false);
    }

    void Start()
    {
        worldClock = Clock.globalClock;

        launchFrequency = minLaunchFrequency;

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
            if (Time.time >= lastLaunchTime + launchFrequency)
            {
                Vector3 spawnPosition = transform.position + orbSpawnOffset;
                GameObject newOrb = Instantiate(orbPrefab, spawnPosition, Quaternion.identity);
                Rigidbody orbRigidbody = newOrb.GetComponent<Rigidbody>();

                LaunchOrb(orbRigidbody);
                lastLaunchTime = Time.time;
                launchFrequency = Random.Range(minLaunchFrequency, maxLaunchFrequency);
            }
        }
    }

    private void LaunchOrb(Rigidbody orbRigidbody)
    {
        float yAngle = Random.Range(0, 360);
        float xAngle = Random.Range(minYaw, maxYaw);
        float forceMagnitude = Random.Range(minLaunchForce, maxLaunchForce);

        float yRad = yAngle * Mathf.Deg2Rad;
        float xRad = xAngle * Mathf.Deg2Rad;

        Vector3 direction = new Vector3(
            Mathf.Cos(yRad) * Mathf.Cos(xRad),
            Mathf.Sin(xRad),
            Mathf.Sin(yRad) * Mathf.Cos(xRad));

        orbRigidbody.AddForce(direction * forceMagnitude, ForceMode.Impulse);
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
}
