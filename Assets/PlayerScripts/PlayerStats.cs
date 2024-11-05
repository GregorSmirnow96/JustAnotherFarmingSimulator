using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats instance;
    
    private Health health;

    private List<float> addativeMovementSpeedMods = new List<float>();
    private List<float> multiplicativeMovementSpeedMods = new List<float>();
    private List<Action<float>> movementSpeedChangeCallbacks = new List<Action<float>>();
    private float baseMovementSpeed = 5f;
    private void NotifyMovementSpeedChanged()
    {
        float movementSpeed = baseMovementSpeed;
        foreach (float addativeMod in addativeMovementSpeedMods)
        {
            movementSpeed += addativeMod;
        }
        foreach (float multiplicativeMod in multiplicativeMovementSpeedMods)
        {
            movementSpeed *= multiplicativeMod;
        }

        movementSpeedChangeCallbacks.ForEach(callback => callback(movementSpeed));
    }

    public void RegisterOnMovementSpeedChangeCallback(Action<float> callback)
    {
        movementSpeedChangeCallbacks.Add(callback);
    }

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        NotifyMovementSpeedChanged();
        health = GetComponent<Health>();
    }

    public void ApplyAddativeSpeedModifier(float speedDelta, float duration)
    {
        StartCoroutine(ApplySpeedModifier(speedDelta, duration, true));
    }

    public void ApplyMultiplicativeSpeedModifier(float speedDelta, float duration)
    {
        StartCoroutine(ApplySpeedModifier(speedDelta, duration, false));
    }

    private IEnumerator ApplySpeedModifier(float speedDelta, float duration, bool additive)
    {
        if (additive)
            addativeMovementSpeedMods.Add(speedDelta);
        else
            multiplicativeMovementSpeedMods.Add(speedDelta);

        NotifyMovementSpeedChanged();

        yield return new WaitForSeconds(duration);

        
        if (additive)
            addativeMovementSpeedMods.Remove(speedDelta);
        else
            multiplicativeMovementSpeedMods.Remove(speedDelta);

        NotifyMovementSpeedChanged();
    }
}
