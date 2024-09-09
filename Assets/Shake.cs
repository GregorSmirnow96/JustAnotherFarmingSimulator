using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour
{
    public float shakeDuration = 0.5f;
    public float shakeMagnitude = 0.02f;

    bool shaking;

    public void ShakeTree()
    {
        StartCoroutine(ShakeCoroutine());
    }

    private IEnumerator ShakeCoroutine()
    {
        if (!shaking)
        {
            shaking = true;

            Vector3 originalPosition = transform.localPosition;

            float elapsedTime = 0f;

            while (elapsedTime < shakeDuration)
            {
                float deltaX = Random.Range(-1f, 1f) * shakeMagnitude;
                float deltaZ = Random.Range(-1f, 1f) * shakeMagnitude;
                transform.localPosition = new Vector3(originalPosition.x - deltaX, originalPosition.y, originalPosition.z - deltaZ);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.localPosition = originalPosition;
            shaking = false;
        }
    }
}
