using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shovel : MonoBehaviour
{
    private Animator animator;
    private int damage = 2;

    private int scaledDamage => PlayerProperties.GetScaledPlayerDamage(damage, DamageType.Physical);

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        bool isIdle = animator.GetCurrentAnimatorStateInfo(0).IsName("Idle");
        if (Input.GetMouseButtonDown(1) && isIdle)
        {
            Debug.Log("Swinging");
            animator.SetTrigger("SwingShovel");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"Collision! Tag: {collision.gameObject.name}");
        if (collision.gameObject.tag == "Enemy")
        {
            Health enemyHealth = collision.gameObject.GetComponent<Health>();
            enemyHealth.TakeDamage(scaledDamage, DamageType.Physical);
        }
    }
}
