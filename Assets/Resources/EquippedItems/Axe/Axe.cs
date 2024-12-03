using System;
using System.Collections;
using UnityEngine;

public class Axe : MonoBehaviour, IUsable
{
    public Animator animator;
    public float swingCooldown = 0.8f;
    private float nextSwingTime = 0f;
    private bool isSwinging = false;
    public Collider axeCollider;
    private Coroutine swingCoroutine;
    private bool swingCollided = false;
    private int damage = 3;
    
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

        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        isSwinging = false;
        animator.SetBool("IsSwinging", false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        HandleHit(collision.gameObject);

        // Not sure if I even want to do this. Letting the animation fully play even on early collision looks less awkward than a QUICK collision
        // Axe hit something, abort the animation
        //StopCoroutine(swingCoroutine);
        //StartCoroutine(HaltAxeSwing());
    }

    private void OnTriggerEnter(Collider otherCollider)
    {
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
            if (hitObject.tag == "Tree")
            {
                LODGroup lodGroup = hitObject.GetComponent<LODGroup>();
                GameObject treeObject = lodGroup == null
                    ? hitObject.transform.parent.gameObject
                    : hitObject;
                Shake shakeScript = treeObject.GetComponent<Shake>();
                shakeScript.ShakeTree();
                Choppable choppableScript = treeObject.GetComponent<Choppable>();
                choppableScript.Chop(scaledDamage);
            }

            swingCollided = true;
            chopScript.Chop(scaledDamage);
            return;
        }

        /* TODO: Pick up here next. This seems to be hitting the carrots. It's failing to hit the rabbit. Why?
            Also, it's kinda a fun idea that you can destroy your own crops if you swing at them. Keep this in
            so animals getting too close is devistating. */
        Health healthScript = hitObject.gameObject.GetComponent<Health>();
        if (healthScript == null)
        {
            healthScript = hitObject.gameObject.transform.parent.gameObject.GetComponentInParent<Health>();
        }
        if (healthScript != null)
        {
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