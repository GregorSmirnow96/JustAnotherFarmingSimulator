using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class PlantEaterBehaviour : MonoBehaviour, IAnimalBehaviour, IAttacker
{
    // Paremeters
    public float retreatTime;
    // Movement parameters
    public float rotationSpeed = 5f;
    // Eat parameters
    public int eatDamage = 1;
    public float eatRange = 1f;
    public float eatInterval = 2f;

    protected Transform target;
    protected NavMeshAgent agent;
    protected Animator animator;
    protected Health health;
    protected Health targetHealth;
    protected Transform plantTarget;
    protected Vector3 lastDestination;
    protected float lastEatTime;
    protected bool wasDamagedLastFrame;
    protected AIStateMachine stateMachine;
    protected bool attackHasHitPlayer;
    protected float animationEndTime;
    protected bool useRootRotation;
    protected string attackName;

    protected float distanceToPlant => (transform.position - target.position).magnitude;
    protected bool inEatRange => distanceToPlant <= eatRange;
    protected bool inAttackAnimation => Time.time <= animationEndTime;
    protected bool inCombatState =>
        !stateMachine.state.Equals(MOVE_TO_PLANT) &&
        !stateMachine.state.Equals(EAT_PLANT) &&
        !stateMachine.state.Equals(RETREAT);

    public const string MOVE_TO_PLANT = "MOVE_TO_PLANT";
    public const string EAT_PLANT = "EAT_PLANT";
    public const string RETREAT = "RETREAT";

    protected void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = 0; // DO NOT CHANGE THIS. This script uses root motion.

        animator = GetComponent<Animator>();

        health = GetComponent<Health>();
        health.RegisterCallback(() => wasDamagedLastFrame = true);

        stateMachine = new AIStateMachine();
    }

    protected void InitializeBaseStates()
    {
        stateMachine.AddState(MOVE_TO_PLANT);
        stateMachine.AddState(EAT_PLANT);
        stateMachine.AddState(RETREAT);
    }

    protected void InitializeBaseTransitions()
    {
        stateMachine.AddTransition(MOVE_TO_PLANT, MoveToPlant_2_EatPlant, EAT_PLANT);
        stateMachine.AddTransition(MOVE_TO_PLANT, MoveToPlant_2_Retreat, RETREAT);
        stateMachine.AddTransition(EAT_PLANT, EatPlant_2_Retreat, RETREAT);
        stateMachine.AddTransition(RETREAT, Retreat_2_MoveToPlant, MOVE_TO_PLANT);
    }

    protected bool MoveToPlant_2_EatPlant() => target != null && inEatRange;
    protected bool MoveToPlant_2_Retreat() => target == null;
    protected bool EatPlant_2_Retreat() => target == null;
    protected bool Retreat_2_MoveToPlant() => target != null;

    protected void Update()
    {
        stateMachine.UpdateState();

        HandleRotation();

        if (stateMachine.state == MOVE_TO_PLANT)
        {
            if (stateMachine.previousState != stateMachine.state)
            {
                SetTarget(plantTarget);
                animator.SetTrigger("Walk");
            }
            MoveToPlant();
        }
        else if (stateMachine.state == EAT_PLANT)
        {
            if (stateMachine.previousState != stateMachine.state)
            {
                animator.SetTrigger("Eat");
            }
            EatPlant();
        }
        else if (stateMachine.state == RETREAT)
        {
            if (stateMachine.previousState != stateMachine.state)
            {
                animator.SetTrigger("Run");
            }
            Retreat();
        }
        else
        {
            HandleCombatState();
        }

        // Handle vertical positional adjustments. This is not done automatically since the NavMeshAgent's movement is disabled.
        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position, out hit, 1.0f, NavMesh.AllAreas))
        {
            Vector3 newPosition = transform.position;
            newPosition.y = hit.position.y;
            transform.position = newPosition;
        }

        wasDamagedLastFrame = false;
    }

    public void HandleRotation()
    {
        if (!useRootRotation)
        {
            Vector3 direction = agent.steeringTarget - transform.position;
            direction.y = 0;

            float distanceToSteeringTarget = direction.magnitude;

            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
            }
        }
    }

    public void SetPlantTarget(Transform newTarget)
    {
        plantTarget = newTarget;
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        targetHealth = target?.GetComponent<Health>();

        if (targetHealth == null)
        {
            target = null;
            Debug.Log("Failed to set the target since the target had no Health component");
        }
    }

    protected void SetDestination()
    {
        agent.SetDestination(target.position);
        lastDestination = target.position;
    }

    protected void SetDestinationByVector(Vector3 destination)
    {
        agent.SetDestination(destination);
        lastDestination = destination;
    }

    protected void MoveToPlant()
    {
        if (target != null && target.position != lastDestination)
        {
            animator.SetTrigger("Walk");
            lastDestination = target.position;
            SetDestination();
        }
    }

    protected void EatPlant()
    {
        float timeSinceLastEat = Time.time - lastEatTime;
        if (timeSinceLastEat >= eatInterval)
        {
            targetHealth.TakeDamage(eatDamage);
            animator.SetTrigger("Eat");
            lastEatTime = Time.time;
        }
    }

    protected void Retreat()
    {
        Vector3 nearestDespawnPosition = DespawnLocations.GetInstance().GetNearestDespawnLocation(transform.position);
        if (nearestDespawnPosition != lastDestination)
        {
            agent.SetDestination(nearestDespawnPosition);
            lastDestination = nearestDespawnPosition;
        }
    }

    protected abstract void HandleCombatState();

    public string GetCurrentAttackName()
    {
        return attackName;
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

    public abstract int GetDamageForCurrentAttack();
}
