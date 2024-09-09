using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AtmosphericHeightFog;

public class FogBehavior : MonoBehaviour
{
    public GameObject player;
    public Vector2 sceneCenter;
    public float maxDistanceFromCenter;

    // Group 1
    public AnimationCurve fogColorCurve;
	public Color startFogColorStart;
	public Color endFogColorStart;
	public Color startFogColorEnd;
	public Color endFogColorEnd;
    public Color startDirectionalColor;
    public Color endDirectionalColor;

    // Group 2
    public AnimationCurve fogDensityCurve;
    public float startFogDistanceEnd;
    public float endFogDistanceEnd;
    public float startNoiseDistanceEnd;
    public float endNoiseDistanceEnd;
    public float startNoiseSpeed;
    public float endNoiseSpeed;

    // Group 3
    public AnimationCurve skyboxFogCurve;
    public float startSkyboxFogHeight;
    public float endSkyboxFogHeight;

    private Transform playerTransform;
    private HeightFogGlobal fogScript;

    private Vector2 playerCenter => new Vector2(playerTransform.position.x, playerTransform.position.z);
    private float playerDistanceFromCenter => (playerCenter - sceneCenter).magnitude;

    void Start()
    {
        playerTransform = player.transform;
        fogScript = GetComponent<HeightFogGlobal>();
    }

    void Update()
    {
        float t = Mathf.Clamp01(playerDistanceFromCenter / maxDistanceFromCenter);

        float fogDensityInterpPercent = fogDensityCurve.Evaluate(t);
        fogScript.fogDistanceEnd = Mathf.Lerp(startFogDistanceEnd, endFogDistanceEnd, fogDensityInterpPercent);
        fogScript.noiseDistanceEnd = Mathf.Lerp(startNoiseDistanceEnd, endNoiseDistanceEnd, fogDensityInterpPercent);
        float noiseSpeed = Mathf.Lerp(startNoiseSpeed, endNoiseSpeed, fogDensityInterpPercent);
        fogScript.noiseSpeed.x = noiseSpeed;
        fogScript.noiseSpeed.y = noiseSpeed;

        float fogColorInterpPercent = fogColorCurve.Evaluate(t);
        fogScript.fogColorStart = Color.Lerp(startFogColorStart, endFogColorStart, fogColorInterpPercent);
        fogScript.fogColorEnd = Color.Lerp(startFogColorEnd, endFogColorEnd, fogColorInterpPercent);
        fogScript.directionalColor = Color.Lerp(startDirectionalColor, endDirectionalColor, fogColorInterpPercent);

        float skyboxFogInterpPercent = skyboxFogCurve.Evaluate(t);
        fogScript.skyboxFogHeight = Mathf.Lerp(startSkyboxFogHeight, endSkyboxFogHeight, skyboxFogInterpPercent);
    }
}
