using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundItem : MonoBehaviour
{
    private static string groundItemTag = "GroundItem";

    public string itemId;

    public float setTagDelay = 0f;

    void Start()
    {
        StartCoroutine(SetTag());
    }

    private IEnumerator SetTag()
    {
        yield return new WaitForSeconds(setTagDelay);
        gameObject.tag = groundItemTag;
    }
}
