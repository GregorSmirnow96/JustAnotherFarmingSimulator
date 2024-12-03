using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSlash : MonoBehaviour
{
    public GameObject waterSlashHitboxPrefab;

    void Start()
    {
        GameObject waterSlashHitbox = Instantiate(waterSlashHitboxPrefab, SceneProperties.cameraTransform);
        waterSlashHitbox.transform.localEulerAngles = new Vector3(0f, 91f, 0);
    }
}
