using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RabbitBehaviour : MonoBehaviour, IAnimalBehaviour, IAttacker
{
    public Transform target;
    public float retreatTime;

    private NavMeshAgent agent;
    private Animator animator;
    private Health health;
    private Health targetHealth;
    private Transform plantTarget;
    private float currentTime => Clock.globalClock.time;
    private Vector3 lastDestination;
    private int eatDamage = 1;
    private float eatRange = 0.5f;
    private float eatInterval = 2f;
    private float lastEatTime;
    private int kickDamage = 4;
    private float attackRange = 0.8f;
    private float attackInterval = 1.8f;
    private float kickDuration = 1.6f;
    private float lastAttackTime;
    private float aggroRange = 5f;
    private bool wasDamagedLastFrame;
    private AIStateMachine stateMachine;
    private bool attackHasHitPlayer;

    private float distanceToPlant => (transform.position - target.position).magnitude;
    private bool inEatRange => distanceToPlant <= eatRange;
    private bool inAttackAnimation => Time.time - lastAttackTime <= kickDuration;

    private string MOVE_TO_PLANT = "MOVE_TO_PLANT";
    private string CHARGE = "CHARGE";
    private string ATTACK = "ATTACK";
    private string EAT_PLANT = "EAT_PLANT";
    private string RETREAT = "RETREAT";

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        health = GetComponent<Health>();
        health.RegisterHealthChangeCallback(() => wasDamagedLastFrame = true);

        stateMachine = new AIStateMachine();
        stateMachine.AddState(MOVE_TO_PLANT);
        stateMachine.AddState(CHARGE);
        stateMachine.AddState(ATTACK);
        stateMachine.AddState(EAT_PLANT);
        stateMachine.AddState(RETREAT);
        stateMachine.AddTransition(MOVE_TO_PLANT, MoveToPlant_2_Charge, CHARGE);
        stateMachine.AddTransition(MOVE_TO_PLANT, MoveToPlant_2_EatPlant, EAT_PLANT);
        stateMachine.AddTransition(MOVE_TO_PLANT, MoveToPlant_2_Retreat, RETREAT);
        stateMachine.AddTransition(CHARGE, Charge_2_Attack, ATTACK);
        stateMachine.AddTransition(CHARGE, Charge_2_MoveToPlant, MOVE_TO_PLANT);
        stateMachine.AddTransition(ATTACK, Attack_2_Charge, CHARGE);
        stateMachine.AddTransition(EAT_PLANT, EatPlant_2_Charge, CHARGE);
        stateMachine.AddTransition(EAT_PLANT, EatPlant_2_Retreat, RETREAT);
        stateMachine.AddTransition(RETREAT, Retreat_2_MoveToPlant, MOVE_TO_PLANT);
        stateMachine.SetInitialState(MOVE_TO_PLANT);

        // DELETE THIS! This is just here to test the rabbit behaviour.
        SetTarget(GameObject.Find("Carrot").transform);
        plantTarget = target;
    }

    bool MoveToPlant_2_Charge() => (SceneProperties.distanceToPlayer(transform) <= aggroRange || wasDamagedLastFrame);
    bool MoveToPlant_2_EatPlant() => target != null && inEatRange;
    bool MoveToPlant_2_Retreat() => target == null;
    bool Charge_2_Attack() => SceneProperties.distanceToPlayerXZ(transform) <= attackRange;
    bool Charge_2_MoveToPlant() => SceneProperties.distanceToPlayer(transform) >= (aggroRange * 1.5f);
    bool Attack_2_Charge() => SceneProperties.distanceToPlayerXZ(transform) > attackRange && !inAttackAnimation;
    bool EatPlant_2_Charge() => wasDamagedLastFrame;
    bool EatPlant_2_Retreat() => target == null;
    bool Retreat_2_MoveToPlant() => target != null;

    void Update()
    {
        stateMachine.UpdateState();

        if (stateMachine.state == MOVE_TO_PLANT)
        {
            if (stateMachine.previousState != stateMachine.state)
            {
                SetTarget(plantTarget);
                animator.SetTrigger("Walk");
                Debug.Log("Start walking");
                agent.speed = 2;
            }
            MoveToPlant();
        }
        else if (stateMachine.state == CHARGE)
        {
            if (stateMachine.previousState != stateMachine.state)
            {
                SetTarget(SceneProperties.playerTransform);
                animator.SetTrigger("Walk");
                agent.updateRotation = true;
                agent.speed = 2;
            }
            Charge();
        }
        else if (stateMachine.state == ATTACK)
        {
            if (stateMachine.previousState != stateMachine.state)
            {
                agent.updateRotation = false;
                agent.speed = 0;
            }
            Attack();
        }
        else if (stateMachine.state == EAT_PLANT)
        {
            if (stateMachine.previousState != stateMachine.state)
            {
                animator.SetTrigger("Eat");
                agent.speed = 0;
            }
            EatPlant();
        }
        else if (stateMachine.state == RETREAT)
        {
            if (stateMachine.previousState != stateMachine.state)
            {
                animator.SetTrigger("Run");
                agent.speed = 5;
            }
            Retreat();
        }

        wasDamagedLastFrame = false;
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        targetHealth = target?.GetComponent<Health>();

        if (targetHealth == null)
        {
            target = null;
            Debug.Log("Failed to set the Rabbit's target since the target had no Health component");
        }
    }

    private void SetDestination()
    {
        agent.SetDestination(target.position);
        lastDestination = target.position;
    }

    private void MoveToPlant()
    {
        if (target != null && target.position != lastDestination)
        {
            animator.SetTrigger("Walk");
            lastDestination = target.position;
            SetDestination();
        }
    }

    private void Charge()
    {
        if (target != null && target.position != lastDestination)
        {
            lastDestination = target.position;
            SetDestination();
        }
    }

    private void Attack()
    {
        const float rotationSpeed = 7f;

        Vector3 directionToPlayer = (SceneProperties.playerTransform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(directionToPlayer.x, 0, directionToPlayer.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        
        float timeSinceLastAttack = Time.time - lastAttackTime;
        if (timeSinceLastAttack >= eatInterval)
        {
            lastAttackTime = Time.time;
            attackHasHitPlayer = false;
            animator.SetTrigger("Kick");
        }
    }

    private void EatPlant()
    {
        float timeSinceLastEat = Time.time - lastEatTime;
        if (timeSinceLastEat >= eatInterval)
        {
            targetHealth.TakeDamage(eatDamage, DamageType.Physical);
            animator.SetTrigger("Eat");
            lastEatTime = Time.time;
        }
    }

    private void Retreat()
    {
        Vector3 nearestDespawnPosition = DespawnLocations.GetInstance().GetNearestDespawnLocation(transform.position);
        if (nearestDespawnPosition != lastDestination)
        {
            agent.SetDestination(nearestDespawnPosition);
            lastDestination = nearestDespawnPosition;
        }
    }

    public string GetCurrentAttackName()
    {
        return "Kick";
    }

    public bool InAttackAnimaion()
    {
        return inAttackAnimation;
    }

    public bool GetAttackHasHit()
    {
        return attackHasHitPlayer;
    }

    public void SetAttackHasHit()
    {
        attackHasHitPlayer = true;
    }

    public int GetDamageForCurrentAttack()
    {
        return kickDamage;
    }
}
