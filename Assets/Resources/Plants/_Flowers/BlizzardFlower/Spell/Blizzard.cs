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
    }

    void Start()
    {
        worldClock = Clock.globalClock;

        // This logic makes sense IF the plant it supposed to run at night (it is).
        bool isPastStartTime = (worldClock.time % worldClock.dayDuration) >= startTimeOfDay;
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
}
