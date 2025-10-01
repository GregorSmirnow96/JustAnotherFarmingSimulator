using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour, IUsable
{
    public Collider swordCollider;
    public float swingCooldown = 1.0f;
    public int damage = 1;

    private Animator animator;
    private float previousSwingTime = 0f;
    private float nextSwingTime = 0f;
    private bool isSwinging = false;
    private List<GameObject> collidedObjects;
    private float canCollideDelay = 0.2f;
    private bool canCollide = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Use()
    {
        if (Time.time >= nextSwingTime && !isSwinging)
        {
            StartCoroutine(SwingSword());
        }
    }

    private IEnumerator SwingSword()
    {
        collidedObjects = new List<GameObject>();
        isSwinging = true;
        canCollide = false;

        animator.SetTrigger("Swing");
        
        yield return new WaitForSeconds(canCollideDelay);
        canCollide = true;
        yield return new WaitForSeconds(0.85f - canCollideDelay * 2);
        canCollide = false;
        yield return new WaitForSeconds(canCollideDelay);

        isSwinging = false;
    }

    private int scaledDamage => PlayerProperties.GetScaledPlayerDamage(damage, DamageType.Physical);

    private void OnTriggerEnter(Collider otherCollider)
    {
        if (canCollide)
        {
            bool alreadyCollided = collidedObjects.Contains(otherCollider.gameObject);
            bool collidedWithPlayer = otherCollider.gameObject.transform == SceneProperties.playerTransform;
            if (alreadyCollided || collidedWithPlayer)
            {
                return;
            }
            collidedObjects.Add(otherCollider.gameObject);

            Health health = otherCollider.gameObject.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(scaledDamage, DamageType.Physical);
            }
        }
    }
}
