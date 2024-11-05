using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FairyLight : MonoBehaviour
{
    public float heightAbovePlayer = 1.56f;
    public float movementDelay = 0.8f;
    public float maxSpeedDistance = 3f;
    public float minSpeedDistance = 0.2f;
    public float maxSpeed = 12f;
    public float minSpeed = 0f;
    public float pulseDuration = 1f;
    public float maxIntensity = 2f;
    public float minIntensity = 1f;
    public float maxRange = 10f;
    public float minRange = 8f;
    public Vector3 maxGlowSize = new Vector3(0.64f, 0.64f, 0.64f);
    public Vector3 minGlowSize = new Vector3(0.48f, 0.48f, 0.48f);
    public AnimationCurve pulseCurve;
    public float decayDuration = 1.3f;

    private Vector3 followPlayerOffset;
    private Transform playerTransform;
    private Light light;
    private float lightDuration;
    private Transform glowPSTransform;

    void Start()
    {
        playerTransform = SceneProperties.playerTransform;
        followPlayerOffset = playerTransform.up * heightAbovePlayer;

        light = GetComponent<Light>();
        ParticleSystem lightParticleSystem = GetComponent<ParticleSystem>();
        lightDuration = lightParticleSystem.main.startLifetime.constant;

        glowPSTransform = transform.Find("GlowWide");

        StartCoroutine(DelayLightMovement());
        StartCoroutine(Pulse());
    }

    void Update()
    {
        Vector3 followPoint = playerTransform.position + followPlayerOffset;
        Vector3 movementDirection = followPoint - transform.position;
        float distanceFromTarget = movementDirection.magnitude;
        float distancePercent = (distanceFromTarget - minSpeedDistance) / (maxSpeed - minSpeedDistance);
        distancePercent = Mathf.Clamp(distancePercent, 0f, 1f);
        float speed = minSpeed + distancePercent * (maxSpeed - minSpeed);

        Vector3 delta = movementDirection.normalized * speed * Time.deltaTime;
        transform.position = transform.position + delta;
    }

    private IEnumerator DelayLightMovement()
    {
        float initialMaxSpeed = maxSpeed;
        maxSpeed = 0f;
        yield return new WaitForSeconds(movementDelay);
        maxSpeed = initialMaxSpeed;
    }

    private IEnumerator Pulse()
    {
        float pulseTimer = 0f;
        while (pulseTimer <= lightDuration)
        {
            float pulsePhase = (pulseTimer % pulseDuration) / pulseDuration;
            float pulseScale = pulseCurve.Evaluate(pulsePhase);

            float nextRange = minRange + pulseScale * (maxRange - minRange);
            light.range = nextRange;

            float nextIntensity = minIntensity + pulseScale * (maxIntensity - minIntensity);
            light.intensity = nextIntensity;

            Vector3 nextGlowSize = minGlowSize + pulseScale * (maxGlowSize - minGlowSize);
            glowPSTransform.localScale = nextGlowSize;

            yield return null;

            pulseTimer += Time.deltaTime;
        }

        float decayTimer = 0f;
        float initialRange = light.range;
        float initialIntensity = light.intensity;
        Vector3 initialGlowScale = glowPSTransform.localScale;
        while (decayTimer <= decayDuration)
        {
            float lightStrength = 1 - (decayTimer / decayDuration);
            light.range = initialRange * lightStrength;
            light.intensity = initialIntensity * lightStrength;
            glowPSTransform.localScale = initialGlowScale * lightStrength;

            yield return null;

            decayTimer += Time.deltaTime;
        }

        Destroy(gameObject);
    }
}
