using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class ForestSpirit : MonoBehaviour
{
    public Transform headTransform;

    private GameObject player;
    private NavMeshAgent agent;
    private AIStateMachine stateMachine;
    private Animator animator;

    private string STALK_IN_CLEARING = "STALK_IN_CLEARING";
    private string STALK_IN_FOREST = "STALK_IN_FOREST";
    private string ATTACK = "ATTACK";
    private string GRAPPLE = "GRAPPLE";
    private string DESPAWN = "DESPAWN";

    void Start()
    {
        player = GameObject.Find("Player");
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        stateMachine = new AIStateMachine();
        stateMachine.AddState(STALK_IN_CLEARING);
        stateMachine.AddState(STALK_IN_FOREST);
        stateMachine.AddState(ATTACK);
        stateMachine.AddState(GRAPPLE);
        stateMachine.AddState(DESPAWN);
        stateMachine.AddTransition(STALK_IN_CLEARING, StalkInClearing_2_StalkInForest, STALK_IN_FOREST);
        stateMachine.AddTransition(STALK_IN_CLEARING, StalkInClearing_2_Attack, ATTACK);
        stateMachine.AddTransition(STALK_IN_CLEARING, NightEnded, DESPAWN);
        stateMachine.AddTransition(STALK_IN_FOREST, StalkInForest_2_StalkInClearing, STALK_IN_CLEARING);
        stateMachine.AddTransition(STALK_IN_FOREST, StalkInForest_2_Attack, ATTACK);
        stateMachine.AddTransition(STALK_IN_FOREST, NightEnded, DESPAWN);
        stateMachine.AddTransition(ATTACK, Attack_2_StalkInClearing, STALK_IN_CLEARING);
        stateMachine.AddTransition(ATTACK, Attack_2_Grapple, GRAPPLE);
        stateMachine.AddTransition(ATTACK, Attack_2_StalkInForest, STALK_IN_FOREST);
        stateMachine.AddTransition(ATTACK, NightEnded, DESPAWN);
        stateMachine.AddTransition(GRAPPLE, Grapple_2_Attack, ATTACK);
        stateMachine.AddTransition(GRAPPLE, NightEnded, DESPAWN);
        stateMachine.SetInitialState(STALK_IN_CLEARING);

        nextExtinguishInterval = 6; // Random.Range(minExtenguishInterval, maxExtenguishInterval);

        GetComponent<Health>().RegisterHealthChangeCallback(() => hasBeenDamagedByPlayer = true);
    }

    // Properties shared between states
    const float minExtenguishInterval = 6f;
    const float maxExtenguishInterval = 8f;
    float timeSinceLastExtinguish = 0f;
    float nextExtinguishInterval;
    void Update()
    {
        timeSinceLastExtinguish += Time.deltaTime;
        if (timeSinceLastExtinguish >= nextExtinguishInterval)
        {
            Debug.Log("Extinguishing.");
            List<LightSource> lightsNearPlayer = LightSource.worldLights.Where(light => light.PlayerIsInVicinity(20)).ToList();
            lightsNearPlayer.ForEach(light => light.FlickerOff(2, 16, 2));
            timeSinceLastExtinguish = 0f;
            nextExtinguishInterval = Random.Range(minExtenguishInterval, maxExtenguishInterval);
        }

        stateMachine.UpdateState();
        if (stateMachine.state == STALK_IN_CLEARING)
        {
            if (stateMachine.previousState != stateMachine.state)
            {
                Debug.Log($"Entering state: {stateMachine.state}");
                // Reset state-specific properties.
                hasSetTarget = false;
            }
            StalkInClearing();
        }
        else if (stateMachine.state == STALK_IN_FOREST)
        {
            if (stateMachine.previousState != stateMachine.state)
            {
                Debug.Log($"Entering state: {stateMachine.state}");
                // Reset state-specific properties.
                time_in_state = 0f;
                followPosition = new Vector3(); 

                animator.SetTrigger("WalkCycle");
            }
            StalkInForest();
        }
        else if (stateMachine.state == ATTACK)
        {
            if (stateMachine.previousState != stateMachine.state)
            {
                Debug.Log($"Entering state: {stateMachine.state}");
                // Reset state-specific properties.
                timeSpentWindingUp = 0f;
                startedRunning = false;
                inRunningCycle = false;
            }
            Attack();
        }
        else if (stateMachine.state == GRAPPLE)
        {
            if (stateMachine.previousState != stateMachine.state)
            {
                Debug.Log($"Entering state: {stateMachine.state}");
                Grapple();
            }
        }
        else if (stateMachine.state == DESPAWN)
        {
            if (stateMachine.previousState != stateMachine.state)
            {
                Debug.Log($"Entering state: {stateMachine.state}");

                retreatPosition = DespawnLocations.GetInstance().GetNearestDespawnLocation(transform.position);
                agent.SetDestination(retreatPosition);

                animator.SetTrigger("RunCycle");
                agent.speed = 14f;
            }

            Retreat();
        }
    }

    // Transition methods
    bool PlayerIsInLight() => LightSource.worldLights.Any(light => light.PlayerIsInRadius());
    bool PlayerIsInDarkness() => !PlayerIsInLight();
    bool PlayerIsInClearing() => SceneProperties.PlayerIsInClearing();
    bool PlayerIsInForest() => SceneProperties.PlayerIsInForest();
    bool PlayerIsWithinAttackRange() => Vector3.Distance(transform.position.ToXZ(), SceneProperties.playerTransform.position.ToXZ()) <= 40f;
    bool PlayerIsWithinGrappleRange() => Vector3.Distance(transform.position.ToXZ(), SceneProperties.playerTransform.position.ToXZ()) <= 5.5f;
    bool NightEnded() => Clock.globalClock.time % Clock.globalClock.dayDuration >= 6f && Clock.globalClock.time % Clock.globalClock.dayDuration <= 18f;

    bool StalkInClearing_2_StalkInForest()
    {
        return PlayerIsInForest() && !NightEnded();
    }

    bool StalkInClearing_2_Attack()
    {
        return (PlayerIsInDarkness() || hasBeenDamagedByPlayer) && !NightEnded();
    }

    bool StalkInForest_2_StalkInClearing()
    {
        return PlayerIsInClearing() && !NightEnded();
    }

    bool StalkInForest_2_Attack()
    {
        return (PlayerIsInDarkness() || PlayerIsWithinAttackRange() || hasBeenDamagedByPlayer) && !NightEnded();
    }

    bool Attack_2_StalkInClearing()
    {
        return (PlayerIsInClearing() && PlayerIsInLight() && !hasBeenDamagedByPlayer) && !NightEnded();
    }

    bool Attack_2_StalkInForest()
    {
        return (PlayerIsInForest() && !PlayerIsWithinAttackRange() && PlayerIsInLight() && !hasBeenDamagedByPlayer) && !NightEnded();
    }

    bool Attack_2_Grapple()
    {
        return (PlayerIsWithinGrappleRange()) && !NightEnded(); // Add condition to check that the cryptid is facing the player.
                                                                // Also, in the Grapple function:
                                                                //  1) freeze position in case the cryptid slides a little further
                                                                //  3) rotate the cryptid to face the player. The already must be mostly facing the player, but make it dead-on
    }

    bool Grapple_2_Attack() => false;

    // If the player is in the clearing, stay in the trees until they walk out of light.
    // Properties specific to StalkInClearing:
    private const float targetDistanceSetThreshold = 6f;
    private const float maxTimeSpentAtTarget = 4f;
    private bool hasSetTarget;
    private Vector3 currentTarget;
    private float timeSpentAtTarget = 0f;
    public void StalkInClearing()
    {
        agent.speed = 3f;

        if (!hasSetTarget)
        {
            animator.SetTrigger("WalkCycle");
        }

        if (!hasSetTarget || timeSpentAtTarget >= maxTimeSpentAtTarget)
        {
            // Calculate & use player's X position
            currentTarget = SceneProperties.playerTransform.position;
            // Choose a random Z location. Constrain the location to a narrow band just behind the forest's front Z position.
            currentTarget.z = Random.Range(SceneProperties.forestLineZPosition + 30f, SceneProperties.forestLineZPosition + 80f);
            agent.SetDestination(currentTarget);

            timeSpentAtTarget = 0f;
            hasSetTarget = true;
        }
        else
        {
            float distanceToTarget = (headTransform.position - currentTarget).magnitude;
            if (targetDistanceSetThreshold >= distanceToTarget)
            {
                timeSpentAtTarget += Time.deltaTime;
            }
        }
    }

    // If the player is in the forest, follow them more closely and wait less time to attack.
    // StalkInForest specific properties
    const float start_distance = 60f;
    const float closing_distance_per_second = 1.5f;
    const float requiredDistanceToTarget = 4f;
    float time_in_state = 0f;
    Vector3 followPosition = new Vector3(0f, 0f, 0f);
    public void StalkInForest()
    {
        agent.speed = 6f;
        time_in_state += Time.deltaTime;

        // Calculate next destination if it's the default destination or the spirit has reached current followPosition
        float distanceToNextPosition = followPosition.x * followPosition.z == 0
            ? 0
            : Vector3.Distance(transform.position.ToXZ(), followPosition.ToXZ());

        if (distanceToNextPosition <= requiredDistanceToTarget)
        {
            float followDistance = start_distance - closing_distance_per_second * time_in_state;
            Vector2 playerToSpiritNormal = (transform.position - SceneProperties.playerTransform.position).ToXZ().normalized;
            Vector2 followPositionXZ = SceneProperties.playerTransform.position.ToXZ() + playerToSpiritNormal * followDistance;
            float followPositionHeight = SceneProperties.TerrainHeightAtPosition(followPositionXZ);
            followPosition = new Vector3(followPositionXZ.x, followPositionHeight, followPositionXZ.y);
            agent.SetDestination(followPosition);
        }
    }

    // Attack specific properties
    const float windUpTime = 2f;
    float timeSpentWindingUp = 0f;
    const float runningStartDuration = 0.542f;
    float startRunningTime;
    float previousTime;
    bool inRunningCycle;
    bool startedRunning = false;
    bool hasBeenDamagedByPlayer;
    public void Attack()
    {
        // Charge after being in this state for 4 seconds
        if (timeSpentWindingUp <= windUpTime)
        {
            timeSpentWindingUp += Time.deltaTime;
            startRunningTime = Time.time;
            // Rotate towards player
            previousTime = Time.time;
            return;
        }
        else if (!startedRunning)
        {
            agent.speed = 14f;
            animator.SetTrigger("RunStart");
            startedRunning = true;
        }

        if (!inRunningCycle)
        {
            float runCycleStart = startRunningTime + runningStartDuration;
            if (previousTime < runCycleStart && Time.time >= runCycleStart)
            {
                animator.SetTrigger("RunCycle");
                inRunningCycle = true;
            }
        }

        // Refactor once animations exist. The spirit will ready a charge, begin its charge, then grapple the player with vines once in range.
        // If player reaches light DURING the charge, transition into a halt animation. Stare at the player for a couple seconds, then walk back into the woods.
        Vector3 destination = SceneProperties.playerTransform.position;
        agent.speed = 14f;
        agent.SetDestination(destination);

        previousTime = Time.time;
    }

    public void Grapple()
    {
        agent.speed = 0f;

        FPSMovement playerFPSMovement = SceneProperties.playerTransform.GetComponent<FPSMovement>();
        playerFPSMovement.enabled = false;

        animator.SetTrigger("EatPlayer");
        StartCoroutine(RotatePlayerTowardsCryptid());
        
        Vector3 cryptidDirectionToPlayer = SceneProperties.playerTransform.position - transform.position;
        Quaternion cryptidRotation = Quaternion.LookRotation(cryptidDirectionToPlayer, Vector3.up);
        transform.rotation = cryptidRotation;

        StartCoroutine(KeepPlayerInPlace());
    }

    private IEnumerator RotatePlayerTowardsCryptid()
    {
        const float rotationDuration = 2f;

        Transform playerTransform = SceneProperties.playerTransform;
        Vector3 playerDirectionToCryptid = headTransform.position - playerTransform.position;
        Quaternion rotation = Quaternion.LookRotation(playerDirectionToCryptid, Vector3.up);
        Quaternion initialPlayerRotation = playerTransform.rotation;

        float rotationTimer = 0f;
        while (rotationTimer <= rotationDuration)
        {
            playerTransform.rotation = Quaternion.Lerp(initialPlayerRotation, rotation, rotationTimer / rotationDuration);
            rotationTimer += Time.deltaTime;
            yield return null;
        }

        playerTransform.rotation = rotation;
    }

    private IEnumerator KeepPlayerInPlace()
    {
        const float holdDuration = 1f;
        float holdTimer = 0f;
        if (holdTimer <= holdDuration)
        {
            Transform playerTransform = SceneProperties.playerTransform;
            playerTransform.position = transform.position + transform.forward * 7.5f - Vector3.up * 0.5f;
            holdTimer += Time.time;

            yield return null;
        }

        yield return new WaitForSeconds(2f);

        FadeToBlack.instance.StartFading();
    }

    private Vector3 retreatPosition;
    private void Retreat()
    {
        if ((SceneProperties.playerTransform.position - transform.position).magnitude < 20)
        {
            retreatPosition = DespawnLocations.GetInstance().GetFurthestDespawnLocation(transform.position);
        }

        if ((retreatPosition - transform.position).magnitude < 5)
        {
            DropLoot dropLootScripts = GetComponent<DropLoot>();
            if (dropLootScripts != null)
            {
                Destroy(dropLootScripts);
            }

            Destroy(gameObject);
        }
    }
}
