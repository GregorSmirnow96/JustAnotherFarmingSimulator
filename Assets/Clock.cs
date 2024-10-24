using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Clock : MonoBehaviour
{
    public static Clock globalClock;

    public float time;
    public float dayDuration = 24.0f;
    public float fastforwardScale = 0.1f;
    public float freezeTime = 25f;

    void Awake()
    {
        globalClock = GameObject.Find("CelestialBodies").GetComponent<Clock>();
    }

    void Update()
    {
        if (freezeTime <= 24f && time >= freezeTime) return;

        time += Time.deltaTime * fastforwardScale;
    }
}
