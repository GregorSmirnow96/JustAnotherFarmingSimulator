using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Campfire : MonoBehaviour
{
    public ParticleSystem alphaParticleSystem;
    public ParticleSystem addParticleSystem;
    public ParticleSystem glowParticleSystem;
    public ParticleSystem sparksParticleSystem;
    public Light light;

    private float timer;
    private bool on;

    void Start()
    {
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 10)
        {
            if (on)
            {
                turnOff();
            }
            else
            {
                turnOn();
            }

            timer = 0;
        }
        Debug.Log(timer);
    }

    public void turnOn()
    {
        on = true;
        light.gameObject.SetActive(true);
        alphaParticleSystem.Play();
        addParticleSystem.Play();
        glowParticleSystem.Play();
        sparksParticleSystem.Play();
    }

    public void turnOff()
    {
        on = false;
        light.gameObject.SetActive(false);
        alphaParticleSystem.Stop();
        addParticleSystem.Stop();
        glowParticleSystem.Stop();
        sparksParticleSystem.Stop();
    }
}
