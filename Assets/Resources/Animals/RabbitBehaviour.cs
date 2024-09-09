using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RabbitBehaviour : MonoBehaviour, IAnimalBehaviour
{
    public Transform plantTarget;
    public float retreatTime;

    private NavMeshAgent agent;
    private Health health;
    private Health targetHealth;
    private float currentTime => Clock.globalClock.time;
    private Vector3 lastDestination;
    private int damage = 1;
    private float eatRange = 1.2f;
    private float attackInterval = 2f;
    private float lastAttackTime;
    private AIStateMachine stateMachine;

    private float distanceToPlant => (transform.position - plantTarget.position).magnitude;
    private bool inEatRange => distanceToPlant <= eatRange;


    private string MOVE_TO_PLANT = "MOVE_TO_PLANT";
    private string EAT_PLANT = "EAT_PLANT";
    private string RETREAT = "RETREAT";

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        health = GetComponent<Health>();

        stateMachine = new AIStateMachine();
        stateMachine.AddState(MOVE_TO_PLANT);
        stateMachine.AddState(EAT_PLANT);
        stateMachine.AddState(RETREAT);
        stateMachine.AddTransition(MOVE_TO_PLANT, MoveToPlant_2_EatPlant, EAT_PLANT);
        stateMachine.AddTransition(MOVE_TO_PLANT, MoveToPlant_2_Retreat, RETREAT);
        stateMachine.AddTransition(EAT_PLANT, EatPlant_2_Retreat, RETREAT);
        stateMachine.AddTransition(RETREAT, Retreat_2_MoveToPlant, MOVE_TO_PLANT);
        stateMachine.SetInitialState(MOVE_TO_PLANT);
    }

    bool MoveToPlant_2_EatPlant() => plantTarget != null && inEatRange;
    bool MoveToPlant_2_Retreat() => plantTarget == null;
    bool EatPlant_2_Retreat() => plantTarget == null;
    bool Retreat_2_MoveToPlant() => plantTarget != null;

    void Update()
    {
        stateMachine.UpdateState();
        if (stateMachine.state == MOVE_TO_PLANT)
        {
            if (stateMachine.previousState != stateMachine.state); // Reset state specific variables.
            MoveToPlant();
        }
        else if (stateMachine.state == EAT_PLANT)
        {
            if (stateMachine.previousState != stateMachine.state); // Reset state specific variables.
            EatPlant();
        }
        else if (stateMachine.state == RETREAT)
        {
            if (stateMachine.previousState != stateMachine.state); // Reset state specific variables.
            Retreat();
        }
    }

    public void SetTarget(Transform newTarget)
    {
        plantTarget = newTarget;
        targetHealth = plantTarget?.GetComponent<Health>();

        if (targetHealth == null)
        {
            plantTarget = null;
            Debug.Log("Failed to set the Rabbit's target since the target had no Health component");
        }
    }

    private void MoveToPlant()
    {
        if (plantTarget != null && plantTarget.position != lastDestination)
        {
            agent.SetDestination(plantTarget.position);
            lastDestination = plantTarget.position;
        }
    }

    private void EatPlant()
    {
        float timeSinceLastAttack = Time.time - lastAttackTime;
        if (timeSinceLastAttack >= attackInterval)
        {
            targetHealth.TakeDamage(damage);
            lastAttackTime = Time.time;
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
}
