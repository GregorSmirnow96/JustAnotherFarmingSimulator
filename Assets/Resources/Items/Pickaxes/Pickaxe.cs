using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pickaxe : MonoBehaviour, IUsable
{
    public float swingCooldown = 0.8f;
    public int damage = 3;
    public float minRockDamage = 1;
    public float maxRockDamage = 3;
    public int pickaxeTier = 1;

    private int scaledDamage => PlayerProperties.GetScaledPlayerDamage(damage, DamageType.Physical);

    private float nextSwingTime = 0f;
    private bool isSwinging = false;
    private bool swingCollided = false;
    private Coroutine swingCoroutine;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Use()
    {
        if (Time.time >= nextSwingTime && !isSwinging)
        {
            swingCoroutine = StartCoroutine(SwingPickaxe());
        }
    }

    private IEnumerator SwingPickaxe()
    {
        animator.SetTrigger("Swing");
        swingCollided = false;
        isSwinging = true;
        nextSwingTime = Time.time + swingCooldown;

        yield return new WaitForSeconds(0.717f); // Hard coded animation length of swing. This should probably be gotten automatically.

        isSwinging = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        HandleHit(collision);

        // Not sure if I even want to do this. Letting the animation fully play even on early collision looks less awkward than a QUICK collision
        // Axe hit something, abort the animation
        //StopCoroutine(swingCoroutine);
        //StartCoroutine(HaltAxeSwing());
    }

    private void OnTriggerEnter(Collider otherCollider)
    {
        Vector3 closestPoint = otherCollider.ClosestPoint(transform.position);
        Debug.Log($"THIS SHOULDN'T BE HAPPENING!!: Trigger collision. Collision point: {closestPoint}");
        // HandleHit(otherCollider.gameObject, closestPoint);
    }

    private void HandleHit(Collision collision)
    {
        GameObject hitObject = collision.gameObject;

        if (!isSwinging || hitObject.tag == "Player" || swingCollided) return;

        IMinable minable = hitObject.GetComponent<IMinable>();
        if (minable != null)
        {
            swingCollided = true;
            float rockDamage = Random.Range(minRockDamage, maxRockDamage + 0.001f);
            minable.Mine(rockDamage, pickaxeTier);

            ContactPoint contact = collision.contacts.First();
            GameObject collisionParticleSystem = minable.GetCollisionParticleSystem();
            Instantiate(collisionParticleSystem, contact.point, Quaternion.identity);
        }

        Health health = hitObject.GetComponent<Health>();
        if (health != null)
        {
            // Do the damage thing
            health.TakeDamage(scaledDamage, DamageType.Physical);
        }
    }
}
