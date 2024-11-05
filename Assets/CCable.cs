using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCable : MonoBehaviour, ICCable
{
    public void Stun(float duration)
    {
        Debug.Log($"Stunning '{gameObject.name}' for {duration} seconds.");
    }

    public void Slow(float duration, float strength)
    {
        Debug.Log($"Slowing '{gameObject.name}' for {duration} seconds. Strength = {strength}");
    }
    
    public void KnockBack(float distance, float duration, Vector3 sourcePosition)
    {
        Debug.Log($"Knocking back '{gameObject.name}' {distance} meters over {duration} seconds. (Source: {sourcePosition})");
    }

    public void Charm(float duration, Transform sourceTransform)
    {
        Debug.Log($"Charming '{gameObject.name}' for {duration} seconds. (Source Position: {sourceTransform.position})");
    }
}
