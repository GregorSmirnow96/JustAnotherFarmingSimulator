using System;
using System.Collections;
using UnityEngine;

public class Axe : MonoBehaviour, IUsable
{
    public Animator animator;
    public int damage = 2;
    public float swingCooldown = 0.8f;
    public int axeTier = 1;
    public Collider axeCollider;

    private float nextSwingTime = 0f;
    private bool isSwinging = false;
    private Coroutine swingCoroutine;
    private bool swingCollided = false;
    
    private int scaledDamage => PlayerProperties.GetScaledPlayerDamage(damage, DamageType.Physical);

    float previousSwingTime = 0f;

    public void Use()
    {
        if (Time.time >= nextSwingTime && !isSwinging)
        {
            swingCoroutine = StartCoroutine(SwingAxe());
        }
    }

    private IEnumerator SwingAxe()
    {
        swingCollided = false;
        isSwinging = true;
        animator.SetBool("IsSwinging", true);

        animator.SetTrigger("Swing");
        nextSwingTime = Time.time + swingCooldown;

        float swingDuration = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(0.4f);
        axeCollider.enabled = true;
        yield return new WaitForSeconds(swingDuration - 0.4f);

        isSwinging = false;
        animator.SetBool("IsSwinging", false);
        axeCollider.enabled = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision");
        HandleHit(collision.gameObject);

        // Not sure if I even want to do this. Letting the animation fully play even on early collision looks less awkward than a QUICK collision
        // Axe hit something, abort the animation
        //StopCoroutine(swingCoroutine);
        //StartCoroutine(HaltAxeSwing());
    }

    private void OnTriggerEnter(Collider otherCollider)
    {
        Debug.Log("Trigger");
        HandleHit(otherCollider.gameObject);
    }

    private void HandleHit(GameObject hitObject)
    {
        if (!isSwinging || hitObject.tag == "Player" || swingCollided) return;

        IChoppable chopScript = hitObject.gameObject.GetComponent<IChoppable>();
        if (chopScript == null)
        {
            chopScript = hitObject?.gameObject?.transform?.parent?.gameObject?.GetComponentInParent<IChoppable>();
        }
        if (chopScript != null)
        {
            Shake shakeScript = hitObject.GetComponent<Shake>();
            if (shakeScript != null)
            {
                shakeScript.ShakeTree();
            }

            swingCollided = true;
            chopScript.Chop(scaledDamage, axeTier);
            return;
        }

        /* TODO: Pick up here next. This seems to be hitting the carrots. It's failing to hit the rabbit. Why?
            Also, it's kinda a fun idea that you can destroy your own crops if you swing at them. Keep this in
            so animals getting too close is devistating. */
        Health healthScript = hitObject.gameObject.GetComponent<Health>();
        if (healthScript == null)
        {
            healthScript = hitObject.gameObject.transform?.parent.gameObject.GetComponentInParent<Health>();
        }
        if (healthScript != null)
        {
            swingCollided = true;
            healthScript.TakeDamage(scaledDamage, DamageType.Physical);
        }
    }

    private IEnumerator HaltAxeSwing()
    {
        animator.speed = 0;
        yield return new WaitForSeconds(0.1f);
        animator.speed = 1;

        animator.SetBool("IsSwinging", false);
        isSwinging = false;
        animator.Play("Idle");
    }
}