using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxCombatBehaviourExtensions : PlantEaterBehaviour
{
    // Neutral states
    private const string CREATE_DISTANCE = "CREATE_DISTANCE";
    private const string TURN_TO_PLAYER = "TURN_TO_PLAYER";
    // Approach states
    private const string WALK_APPROACH = "WALK_APPROACH";
    private const string TROT_APPROACH = "TROT_APPROACH";
    private const string RUN_APPROACH = "RUN_APPROACH";
    // Attack states
    private const string LUNGE = "LUNGE";
    private const string BITE = "BITE";
    private const string CLAW = "CLAW";

    private float distanceToPlayer => SceneProperties.distanceToPlayer(transform);

    // Interface functions
    public override int GetDamageForCurrentAttack()
    {
        return nextAttack == LUNGE
            ? lungeDamage
            : nextAttack == BITE
                ? biteDamage
                : clawDamage;
    }

    private void Start()
    {
        InitializeBaseStates();

        // Add fox states
        stateMachine.AddState(CREATE_DISTANCE);
        stateMachine.AddState(TURN_TO_PLAYER);
        stateMachine.AddState(WALK_APPROACH);
        stateMachine.AddState(TROT_APPROACH);
        stateMachine.AddState(RUN_APPROACH);
        stateMachine.AddState(LUNGE);
        stateMachine.AddState(BITE);
        stateMachine.AddState(CLAW);

        // Add fox transitions
        stateMachine.AddTransition(MOVE_TO_PLANT, EnterCombat, WALK_APPROACH);

        stateMachine.AddTransition(CREATE_DISTANCE, CreateDistance_2_TurnToPlayer, TURN_TO_PLAYER);
        stateMachine.AddTransition(TURN_TO_PLAYER, TurnToPlayer_2_WalkApproach, WALK_APPROACH);
        stateMachine.AddTransition(WALK_APPROACH, WalkApproach_2_TrotApproach, TROT_APPROACH);
        stateMachine.AddTransition(WALK_APPROACH, WalkApproach_2_RunApproach, RUN_APPROACH);
        stateMachine.AddTransition(TROT_APPROACH, TrotApproach_2_Bite, BITE);
        stateMachine.AddTransition(TROT_APPROACH, TrotApproach_2_Claw, CLAW);
        stateMachine.AddTransition(RUN_APPROACH, RunApproach_2_Lunge, LUNGE);
        stateMachine.AddTransition(LUNGE, Lunge_2_CreateDistance, CREATE_DISTANCE);
        stateMachine.AddTransition(LUNGE, Lunge_2_Bite, BITE);
        stateMachine.AddTransition(LUNGE, Lunge_2_Claw, CLAW);
        stateMachine.AddTransition(BITE, Bite_2_CreateDistance, CREATE_DISTANCE);
        stateMachine.AddTransition(BITE, Bite_2_Claw, CLAW);
        stateMachine.AddTransition(CLAW, Claw_2_CreateDistance, CREATE_DISTANCE);
        stateMachine.AddTransition(CLAW, Claw_2_Bite, BITE);

        stateMachine.AddTransition(CREATE_DISTANCE, ExitCombat, MOVE_TO_PLANT);
        stateMachine.AddTransition(WALK_APPROACH, ExitCombat, MOVE_TO_PLANT);
        stateMachine.AddTransition(TROT_APPROACH, ExitCombat, MOVE_TO_PLANT);
        stateMachine.AddTransition(RUN_APPROACH, ExitCombat, MOVE_TO_PLANT);

        InitializeBaseTransitions();

        stateMachine.SetInitialState(MOVE_TO_PLANT);

        // DELETE THIS! This is just here to test the behaviour.
        SetPlantTarget(GameObject.Find("Carrot").transform);
        SetTarget(GameObject.Find("Carrot").transform);
        SetPlantTarget(target);
    }

    protected override void HandleCombatState()
    {
        if (stateMachine.state == CREATE_DISTANCE)
        {
            MoveAwayFromPlayer(maxBeginApproachDistance);
            if (stateMachine.previousState != stateMachine.state)
            {
                animator.SetTrigger("Run");
            }
        }
        else if (stateMachine.state == TURN_TO_PLAYER)
        {
            MoveToPlayer();
            if (stateMachine.previousState != stateMachine.state)
            {
                animator.SetTrigger("TurnRight");
                finishedTurning = false;
                useRootRotation = true;
                StartCoroutine(TurnToPlayer());
            }
        }
        else if (stateMachine.state == WALK_APPROACH)
        {
            MoveToPlayer();
            if (stateMachine.previousState != stateMachine.state)
            {
                nextAttack = LUNGE;
                float attackRoll = Random.Range(0f, 1f);
                nextAttack = attackRoll <= 0.4f
                    ? LUNGE
                    : attackRoll <= 0.7f
                        ? CLAW
                        : BITE;
                animator.SetTrigger("Walk");
            }
        }
        else if (stateMachine.state == TROT_APPROACH)
        {
            MoveToPlayer();
            if (stateMachine.previousState != stateMachine.state)
            {
                animator.SetTrigger("Trot");
            }
        }
        else if (stateMachine.state == RUN_APPROACH)
        {
            MoveToPlayer();
            if (stateMachine.previousState != stateMachine.state)
            {
                animator.SetTrigger("Run");
            }
        }
        else if (stateMachine.state == LUNGE)
        {
            MoveToPlayer();
            if (stateMachine.previousState != stateMachine.state)
            {
                animator.SetTrigger("Jump");
                attackName = LUNGE;
                attackHasHitPlayer = false;
                animationEndTime = Time.time + attackAnimationDuration;
            }
        }
        else if (stateMachine.state == BITE)
        {
            MoveToPlayer();
            if (stateMachine.previousState != stateMachine.state)
            {
                animator.SetTrigger("BiteRight");
                attackName = BITE;
                attackHasHitPlayer = false;
                animationEndTime = Time.time + attackAnimationDuration;
            }
        }
        else if (stateMachine.state == CLAW)
        {
            MoveToPlayer();
            if (stateMachine.previousState != stateMachine.state)
            {
                animator.SetTrigger("Claw");
                attackName = CLAW;
                attackHasHitPlayer = false;
                animationEndTime = Time.time + attackAnimationDuration;
            }
        }
    }

    private IEnumerator TurnToPlayer()
    {
        yield return new WaitForSeconds(rotationAnimationDuration);
        finishedTurning = true;
        useRootRotation = false;
    }

    private void MoveAwayFromPlayer(float distanceFromPlayer)
    {
        Vector3 playerPosition = SceneProperties.playerTransform.position;
        Vector3 playerToAnimal = (transform.position - playerPosition).normalized;
        Vector3 playerToDestination = playerToAnimal * distanceFromPlayer;
        Vector3 newDestination = playerPosition + playerToDestination;

        SetDestinationByVector(newDestination);
    }

    private void MoveToPlayer()
    {
        SetTarget(SceneProperties.playerTransform);
        SetDestination();
    }

    public float aggroDistance = 12f;
    public float minBeginApproachDistance = 8f;
    public float maxBeginApproachDistance = 11f;
    public float deaggroRange = 15f;
    public float speedUpRange = 7f;
    public float biteRange = 2f;
    public float clawRange = 2f;
    public float lungeRange = 4f;
    public int biteDamage = 3;
    public int clawDamage = 2;
    public int lungeDamage = 4;

    private string nextAttack = LUNGE;
    private float attackAnimationDuration = 1f; // They all slightly vary, but I think we can treat them all as 1 second.
    private float rotationAnimationDuration = 0.8f;
    private bool finishedTurning;

    // State logic
    private bool CreateDistance_2_TurnToPlayer() => distanceToPlayer >= minBeginApproachDistance && distanceToPlayer <= maxBeginApproachDistance;
    private bool TurnToPlayer_2_WalkApproach() => finishedTurning;
    private bool WalkApproach_2_TrotApproach() => distanceToPlayer <= speedUpRange && (nextAttack == BITE || nextAttack == CLAW);
    private bool WalkApproach_2_RunApproach() => distanceToPlayer <= speedUpRange && nextAttack == LUNGE;
    private bool TrotApproach_2_Bite() => distanceToPlayer <= biteRange;
    private bool TrotApproach_2_Claw() => distanceToPlayer <= clawRange;
    private bool RunApproach_2_Lunge() => distanceToPlayer <= lungeRange;
    private bool Lunge_2_CreateDistance() => !InAttackAnimaion() && !(distanceToPlayer <= biteRange || distanceToPlayer <= clawRange);
    private bool Lunge_2_Bite() => !InAttackAnimaion() && distanceToPlayer <= biteRange;
    private bool Lunge_2_Claw() => !InAttackAnimaion() && distanceToPlayer <= clawRange;
    private bool Bite_2_CreateDistance() => !InAttackAnimaion() && distanceToPlayer >= clawRange;
    private bool Bite_2_Claw() => !InAttackAnimaion() && distanceToPlayer <= clawRange;
    private bool Claw_2_CreateDistance() => !InAttackAnimaion() && distanceToPlayer >= biteRange;
    private bool Claw_2_Bite() => !InAttackAnimaion() && distanceToPlayer <= biteRange;

    private bool EnterCombat() => distanceToPlayer <= aggroDistance;
    private bool ExitCombat() => distanceToPlayer >= deaggroRange;
}
