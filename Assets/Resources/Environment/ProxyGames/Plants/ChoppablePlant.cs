using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoppablePlant : MonoBehaviour, IChoppable
{
    public void Chop(float damage)
    {
        Debug.Log("Destroying");
        Destroy(gameObject);
    }
}
