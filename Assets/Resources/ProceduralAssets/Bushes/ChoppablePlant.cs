using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoppablePlant : MonoBehaviour, IChoppable
{
    public void Chop(float damage, int axeTier)
    {
        Debug.Log("Destroying");
        Destroy(gameObject);
    }
}
