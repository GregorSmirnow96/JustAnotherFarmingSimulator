using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPlant : MonoBehaviour
{
    public GameObject target;
    private float speed = 3.0f;
    private float attackRange = 1.2f;
    private float attackInterval = 1.0f;
    private float timeSinceLastAttack = 0.0f;
    public int damage = 5;
    private bool targetWasKilled = false;

    private bool hasTarget => target != null;
    private Vector3 targetPosition => target.transform.position;
    private Vector3 position => gameObject.transform.position;
    private float distanceToTarget =>
        hasTarget
        ? DistanceToObject(target)
        : 99999;
    private Vector3 vectorTowardsTarget => (new Vector3(targetPosition.x, 0f, targetPosition.z) - new Vector3(position.x, 0f, position.z));
    private Vector3 vectorTowardsEscape => (new Vector3(escapePosition.x, 0f, escapePosition.z) - new Vector3(position.x, 0f, position.z));
    private bool inAttackRange => distanceToTarget <= attackRange;
    private Vector3 escapePosition;

    void Start()
    {
        escapePosition = gameObject.transform.position;
    }

    void Update()
    {
        timeSinceLastAttack += Time.deltaTime;

        if (inAttackRange)
        {
            AttackTarget();
        }
        else if (!targetWasKilled)
        {
            MoveTowardsTarget();
        }
        else
        {
            MoveTowardsEscape();
            if (vectorTowardsEscape.magnitude <= 0.2f)
            {
                Destroy(gameObject);
            }
        }
    }

    float DistanceToObject(GameObject gameObject) =>
        (position - gameObject.transform.position).magnitude;

    void MoveTowardsTarget()
    {
        Vector3 movementVector = vectorTowardsTarget.normalized * speed * Time.deltaTime;
        gameObject.transform.position += movementVector;
    }

    void MoveTowardsEscape()
    {
        Vector3 movementVector = vectorTowardsEscape.normalized * speed * Time.deltaTime;
        gameObject.transform.position += movementVector;
    }

    void AttackTarget()
    {
        if (timeSinceLastAttack >= attackInterval)
        {
            Health health = target.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(damage, DamageType.Physical);
                targetWasKilled = health.health <= 0;
                if (targetWasKilled)
                {
                    target = null;
                    Debug.Log("Killed target");
                }
            }

            timeSinceLastAttack = 0.0f;
            Debug.Log("Attacked target");
        }
    }
}
