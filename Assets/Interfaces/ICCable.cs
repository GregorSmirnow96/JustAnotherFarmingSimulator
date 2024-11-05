using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICCable
{
    public void Stun(float duration);
    public void Slow(float duration, float strength);
    public void KnockBack(float distance, float duration, Vector3 sourcePosition);
    public void Charm(float duration, Transform sourceTransform);
}
