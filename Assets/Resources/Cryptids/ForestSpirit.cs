using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class ForestSpirit : MonoBehaviour
{
    private GameObject player;
    private NavMeshAgent agent;
    private AIStateMachine stateMachine;

    private string STALK_IN_CLEARING = "STALK_IN_CLEARING";
    private string STALK_IN_FOREST = "STALK_IN_FOREST";
    private string ATTACK = "ATTACK";

    void Start()
    {
        player = GameObject.Find("Player");
        agent = GetComponent<NavMeshAgent>();

        stateMachine = new AIStateMachine();
        stateMachine.AddState(STALK_IN_CLEARING);
        stateMachine.AddState(STALK_IN_FOREST);
        stateMachine.AddState(ATTACK);
        stateMachine.AddTransition(STALK_IN_CLEARING, StalkInClearing_2_StalkInForest, STALK_IN_FOREST);
        stateMachine.AddTransition(STALK_IN_CLEARING, StalkInClearing_2_Attack, ATTACK);
        stateMachine.AddTransition(STALK_IN_FOREST, StalkInForest_2_StalkInClearing, STALK_IN_CLEARING);
        stateMachine.AddTransition(STALK_IN_FOREST, StalkInForest_2_Attack, ATTACK);
        stateMachine.AddTransition(ATTACK, Attack_2_StalkInClearing, STALK_IN_CLEARING);
        // stateMachine.AddTransition(ATTACK, Attack_2_StalkInForest, STALK_IN_FOREST);
        stateMachine.SetInitialState(STALK_IN_CLEARING);

        nextExtinguishInterval = 6; // Random.Range(minExtenguishInterval, maxExtenguishInterval);
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
            lightsNearPlayer.ForEach(light => light.FlickerOff(2, 20, 2));
            timeSinceLastExtinguish = 0f;
            nextExtinguishInterval = Random.Range(minExtenguishInterval, maxExtenguishInterval);
        }

        stateMachine.UpdateState();
        if (stateMachine.state == STALK_IN_CLEARING)
        {
            if (stateMachine.previousState != stateMachine.state)
            {
                destination = new Vector3(0f, 0f, 0f);
                intermediateDestination = new Vector3(0f, 0f, 0f);
                hasIntermediateDestination = false;
                movingToTangent = false;
                timeSinceDestinationSet = 9999f;
                timeSpentStill = 0f;
                lastPosition = new Vector3(0f, 0f, 0f);
            }
            StalkInClearing();
        }
        else if (stateMachine.state == STALK_IN_FOREST)
        {
            if (stateMachine.previousState != stateMachine.state)
            {
                time_in_state = 0f;
                followPosition = new Vector3(0f, 0f, 0f);
            }
            StalkInForest();
        }
        else if (stateMachine.state == ATTACK)
        {
            if (stateMachine.previousState != stateMachine.state)
            {
                timeSpentWindingUp = 0f;
            }
            Attack();
        }
    }

    // Transition methods
    bool PlayerIsInDarkness() => !PlayerIsInLight();
    bool PlayerIsInLight() => LightSource.worldLights.Any(light => light.PlayerIsInRadius());
    bool PlayerIsInClearing() => SceneProperties.playerDistanceFromCenter <= SceneProperties.clearingRadius;
    bool PlayerIsInForest() => SceneProperties.playerDistanceFromCenter > SceneProperties.clearingRadius;
    bool PlayerIsWithin40Units() => Vector3.Distance(transform.position.ToXZ(), SceneProperties.playerTransform.position.ToXZ()) <= 40f;

    bool StalkInClearing_2_StalkInForest()
    {
        return PlayerIsInForest();
    }

    bool StalkInClearing_2_Attack()
    {
        return PlayerIsInDarkness();
    }

    bool StalkInForest_2_StalkInClearing()
    {
        return PlayerIsInClearing();
    }

    bool StalkInForest_2_Attack()
    {
        return PlayerIsInDarkness() || PlayerIsWithin40Units();
    }

    bool Attack_2_StalkInClearing()
    {
        return PlayerIsInClearing() && PlayerIsInLight();
    }

    bool Attack_2_StalkInForest()
    {
        return PlayerIsInForest() && !PlayerIsWithin40Units() && PlayerIsInLight();
    }

    // If the player is in the clearing, stay in the trees until they walk out of light.
    // Properties specific to StalkInClearing:
    private const float minDestinationChangeInterval = 1f;
    private Vector3 destination = new Vector3(0f, 0f, 0f);
    private Vector3 intermediateDestination = new Vector3(0f, 0f, 0f);
    private bool hasIntermediateDestination = false;
    private bool movingToTangent = false;
    private float timeSinceDestinationSet = 9999f;
    private float timeSpentStill = 0f;
    private Vector3 lastPosition = new Vector3(0f, 0f, 0f);
    public void StalkInClearing()
    {
        agent.speed = 6f;
        // Calculate desired end position
        Vector2 centerToPlayerNormalized = (SceneProperties.playerXZPosition - SceneProperties.sceneCenter.ToXZ()).normalized;
        float distanceBeyondClearing = 25f;
        Vector2 treeLinePositionXZ = centerToPlayerNormalized * (SceneProperties.clearingRadius + distanceBeyondClearing) + SceneProperties.sceneCenter.ToXZ();
        float terrainHeight = SceneProperties.TerrainHeightAtPosition(treeLinePositionXZ);
        Vector3 desiredPosition = new Vector3(treeLinePositionXZ.x, terrainHeight, treeLinePositionXZ.y);

        // Check if a new destination needs to be set. If not, return.
        timeSpentStill = transform.position == lastPosition
            ? timeSpentStill += Time.deltaTime
            : timeSpentStill;
        lastPosition = transform.position;
        bool stoodStillForTooLong = timeSpentStill > 3f;

        bool finalDestinationSignificantlyChanged = Vector3.Distance(desiredPosition.ToXZ(), destination.ToXZ()) >= 10f;

        float distanceToIntermediateDestination = (transform.position - intermediateDestination).magnitude;
        bool reachedIntermediateDestination = distanceToIntermediateDestination < 1.5f;

        bool shouldUpdateDestination = finalDestinationSignificantlyChanged || reachedIntermediateDestination || stoodStillForTooLong;
        if (!shouldUpdateDestination) return;

        // Determine whether or not to set the next destination is the desired end position, or a tangential path
        // If the previous "next destination" was tangential, don't calculate a new "next destination" unless the final destination has significantly changed
        float patrolRadius = 10f;
        float angle = Random.Range(0f, 360f);
        float rad = angle * Mathf.Deg2Rad;
        float radius = Random.Range(0, patrolRadius);
        float patrolX = desiredPosition.x + Mathf.Cos(rad) * radius;
        float patrolZ = desiredPosition.z + Mathf.Sin(rad) * radius;
        destination = new Vector3(patrolX, desiredPosition.y, patrolZ);
        bool isAlreadyInClearing = SceneProperties.IsInClearing(transform.position);
        bool willMoveThroughClearing = !isAlreadyInClearing && SceneProperties.LineIntersectsClearing(transform.position.ToXZ(), destination.ToXZ());
        if (willMoveThroughClearing)
        {
            intermediateDestination = SceneProperties.GetTangentPointNearestToDestination(transform.position, distanceBeyondClearing, destination);
            float intermediateTerrainHeight = SceneProperties.TerrainHeightAtPosition(intermediateDestination.ToXZ());
            intermediateDestination.y = intermediateTerrainHeight;
            movingToTangent = true;
        }
        else
        {
            intermediateDestination = destination; // This is a bit of a code-smell. AI will always move to intermediateDestination, but this can be equal to the final destination
            movingToTangent = false;
        }

        agent.SetDestination(intermediateDestination);
        destination = desiredPosition;
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
    public void Attack()
    {
        // Charge after being in this state for 4 seconds
        if (timeSpentWindingUp <= windUpTime)
        {
            timeSpentWindingUp += Time.deltaTime;
            // Rotate towards player
            return;
        }

        // Refactor once animations exist. The spirit will ready a charge, begin its charge, then grapple the player with vines once in range.
        // If player reaches light DURING the charge, transition into a halt animation. Stare at the player for a couple seconds, then walk back into the woods.
        destination = SceneProperties.playerTransform.position;
        agent.speed = 14f;
        agent.SetDestination(destination);
    }
}
