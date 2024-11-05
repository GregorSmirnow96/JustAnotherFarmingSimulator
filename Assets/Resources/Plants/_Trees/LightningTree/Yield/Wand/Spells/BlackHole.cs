using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    public GameObject buildUpPrefab;
    public GameObject loopPrefab;
    public GameObject windDownPrefab;
    public GameObject pulseHitboxPrefab;
    public float upwardForce = 10f;
    public float forwardForce = 10f;
    public float startScale = 0.0f;
    public float endScale = 1.0f;
    public float pulseInterval = 0.35f;
    public int pulses = 8;
    public float transitionPeriod = 0.44f;

    private GameObject loop;
    private Rigidbody rigidbody;
    private bool pulsing;

    void Awake()
    {
        transform.localScale = new Vector3(startScale, startScale, startScale);
    }

    void Start()
    {
        StartCoroutine(SpawnBlackHole());
    }

    private IEnumerator SpawnBlackHole()
    {
        Instantiate(buildUpPrefab, transform);

        rigidbody = GetComponent<Rigidbody>();
        Transform cameraTransform = SceneProperties.cameraTransform;
        Vector3 initialForce = cameraTransform.forward * forwardForce + cameraTransform.up * upwardForce;
        rigidbody.AddForce(initialForce, ForceMode.Impulse);

        float windUpTimer = 0f;
        while (windUpTimer <= transitionPeriod)
        {
            float currentScale = endScale * (windUpTimer / transitionPeriod);
            currentScale = Mathf.Min(currentScale, endScale);
            transform.localScale = new Vector3(currentScale, currentScale, currentScale);

            yield return null;

            windUpTimer += Time.deltaTime;
        }

        loop = Instantiate(loopPrefab, transform);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.name.Equals("Player") && !pulsing)
        {
            pulsing = true;
            rigidbody.constraints = RigidbodyConstraints.FreezePositionX
                | RigidbodyConstraints.FreezePositionY
                | RigidbodyConstraints.FreezePositionZ
                | RigidbodyConstraints.FreezeRotationX
                | RigidbodyConstraints.FreezeRotationY
                | RigidbodyConstraints.FreezeRotationZ;
            StartCoroutine(StartPulsing());
        }
    }

    private IEnumerator StartPulsing()
    {
        for (int loopIndex = 0; loopIndex < pulses; loopIndex++)
        {
            GameObject pulseHitbox = Instantiate(pulseHitboxPrefab, transform);
            yield return new WaitForSeconds(pulseInterval);
            Destroy(pulseHitbox);
        }

        Destroy(loop);
        Instantiate(windDownPrefab, transform);

        yield return new WaitForSeconds(transitionPeriod);

        Destroy(gameObject);
    }
}
