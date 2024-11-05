using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : MonoBehaviour
{
    public float speedBuff = 1.3f;
    public float buffDuration = 15;

    void Start()
    {
        StartCoroutine(SelfDestruct());
    }

    private IEnumerator SelfDestruct()
    {
        PlayerStats.instance.ApplyMultiplicativeSpeedModifier(speedBuff, buffDuration);
        yield return new WaitForSeconds(buffDuration);
    }
}
