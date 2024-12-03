using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Blizzard : MonoBehaviour
{
    public float startTimeOfDay = 18f;
    public float endTimeOfDay = 6f;

    private ParticleSystem particleSystem;
    private List<ParticleSystem.MainModule> mainModules;
    private Clock worldClock;
    private List<ICCable> slowableCreaturesInRadius;

    void Awake()
    {
        particleSystem = GetComponent<ParticleSystem>();

        mainModules = new List<ParticleSystem.MainModule>();
        mainModules.Add(particleSystem.main);

        foreach (Transform childTransform in transform)
        {
            ParticleSystem childParticleSystem = childTransform.GetComponent<ParticleSystem>();
            mainModules.Add(childParticleSystem.main);
        }

        SetLooping(false);

        slowableCreaturesInRadius = new List<ICCable>();
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
            
            slowableCreaturesInRadius.ForEach(ccable =>
            {
                try
                {
                    ccable.Slow(2f, 0.6f);
                }
                catch
                {
                    ccablesToDelete.Add(ccable);
                }
            });

            ccablesToDelete.ForEach(health =>
            {
                slowableCreaturesInRadius.Remove(health);
            });
        }
    }

    private void SetLooping(bool shouldLoop)
    {
        bool wasLooping = particleSystem.main.loop;

        mainModules.ForEach(main => main.loop = shouldLoop);

        if (shouldLoop && !wasLooping)
        {
            particleSystem.Play();
        }
        else if (!shouldLoop && wasLooping)
        {
            particleSystem.Stop();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        GameObject collidedObject = other.gameObject;

        ICCable ccable = collidedObject.GetComponent<ICCable>();
        if (ccable != null)
        {
            slowableCreaturesInRadius.Add(ccable);
        }
    }

    void OnTriggerExit(Collider other)
    {
        GameObject collidedObject = other.gameObject;

        ICCable ccable = collidedObject.GetComponent<ICCable>();
        if (ccable != null)
        {
            slowableCreaturesInRadius.Remove(ccable);
        }
    }
}
