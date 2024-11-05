using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TeleportOrb : MonoBehaviour
{
    public GameObject takeOffExplosion;
    public GameObject arrivalExplosion;
    public float upwardForce = 4f;
    public float forwardForce = 13f;
    public float startScale = 0.1f;
    public float endScale = 1.0f;
    public float transitionPeriod = 0.84f;
    public float maxLifespan = 8;
    public float postCollisionTakeOffDelay = 0.0f;
    public float postTakeOffVFXTeleportTeleportDelay = 1.5f;

    private Dictionary<string, Vector3> originalChildSizes;
    private Coroutine forceAndScaleCoroutine;
    private bool triggeredTeleport;

    void Start()
    {
        originalChildSizes = transform.Cast<Transform>().ToDictionary(
            child => child.gameObject.name,
            child => child.localScale);

        forceAndScaleCoroutine = StartCoroutine(ApplyForceAndScale());
    }

    private IEnumerator ApplyForceAndScale()
    {
        Transform cameraTransform = SceneProperties.cameraTransform;
        Vector3 initialForce = cameraTransform.forward * forwardForce + cameraTransform.up * upwardForce;
        GetComponent<Rigidbody>().AddForce(initialForce, ForceMode.Impulse);

        float windUpTimer = 0f;
        while (windUpTimer <= transitionPeriod)
        {
            windUpTimer += Time.deltaTime;
            foreach (Transform child in transform)
            {
                float relativeScale = Mathf.Min(windUpTimer / transitionPeriod, 1) * (endScale - startScale) + startScale;
                child.localScale = originalChildSizes[child.name] * relativeScale;
            }

            yield return null;
        }

        float fullSizeLifespan = maxLifespan - transitionPeriod * 2;
        yield return new WaitForSeconds(fullSizeLifespan);

        if (!triggeredTeleport)
        {
            StartCoroutine(Shrink());
            yield return new WaitForSeconds(transitionPeriod + 0.2f);
            Destroy(gameObject);
        }
    }

    private IEnumerator Shrink()
    {
        float windDownTimer = 0f;
        while (windDownTimer <= transitionPeriod)
        {
            windDownTimer += Time.deltaTime;
            foreach (Transform child in transform)
            {
                float relativeScale = endScale * Mathf.Max(1 - (windDownTimer / transitionPeriod), 0f);
                child.localScale = originalChildSizes[child.name] * relativeScale;
            }
            
            yield return null;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        GameObject collidedObject = other.gameObject;

        int terrainLayerId = LayerMask.NameToLayer("Terrain");
        if (collidedObject.layer == terrainLayerId && !triggeredTeleport)
        {
            triggeredTeleport = true;
            StartCoroutine(TeleportPlayer());
        }
    }

    private IEnumerator TeleportPlayer()
    {
        // Wait a little after impact before teleporting.
        yield return new WaitForSeconds(postCollisionTakeOffDelay);
        StartCoroutine(Shrink());

        // Spawn take-off VFX.
        Transform playerTransform = SceneProperties.playerTransform;
        // GameObject takeOffVFX = Instantiate(takeOffExplosion, playerTransform);
        GameObject takeOffVFX = Instantiate(takeOffExplosion, playerTransform.position, playerTransform.rotation);
        ParticleSystem takeOffParticleSystem = takeOffVFX.GetComponent<ParticleSystem>();
        takeOffParticleSystem.Play();
        yield return new WaitForSeconds(postTakeOffVFXTeleportTeleportDelay);

        // Move player and spawn arrival VFX.
        playerTransform.position = transform.position;
        GameObject arrivalVFX = Instantiate(arrivalExplosion, playerTransform.position, playerTransform.rotation);
        ParticleSystem arrivalParticleSystem = arrivalVFX.GetComponent<ParticleSystem>();
        arrivalParticleSystem.Play();

        // Destroy this object. This should work since the lingering explosion effect is its own object.
        Destroy(gameObject);
    }
}
