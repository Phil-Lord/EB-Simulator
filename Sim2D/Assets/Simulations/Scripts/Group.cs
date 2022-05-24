using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Group : MonoBehaviour
{
    // Group actor, rule, and memory objects
    public GroupActor actorPrefab;
    List<GroupActor> actors = new List<GroupActor>();
    public GroupRule rule;
    public GroupMemory memory;

    // Variables accessible during run
    [Range(0, 1000)]
    public int startingCount = 200;
    [Range(1f, 100f)]
    public float driveFactor = 10f; // Increases movement when less affected by rules
    [Range(1f, 100f)]
    public float maxSpeed = 5f;
    [Range(1f, 10f)]
    public float neighbourRadius = 1.5f;
    [Range(0f, 1f)]
    public float avoidanceRadiusMultiplier = 0.5f; // 0 = no avoidance, 1 = avoid all objects in neighbour radius

    // Squared values for calculations
    float squareMaxSpeed;
    float squareNeighbourRadius;
    float squareAvoidanceRadius;

    /// <summary>
    /// Radius of a circle within the neighbourhood that specifies objects to avoid.
    /// </summary>
    public float SquareAvoidanceRadius { get { return squareAvoidanceRadius; } }

    public void Start()
    {
        // Use squared values in calculations to reduce expensive root operations
        squareMaxSpeed = maxSpeed * maxSpeed;
        squareNeighbourRadius = neighbourRadius * neighbourRadius;
        squareAvoidanceRadius = squareNeighbourRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;

        actors.Clear();

        // Create list of group actors
        for (int i = 0; i < startingCount; i++)
        {
            // Find random position in range of screen size
            Vector2 spawnPosition = new Vector2(
                Random.Range(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x, Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x),
                Random.Range(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).y, Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y)
                );

            // Create new actor
            GroupActor newActor = Instantiate(
                actorPrefab,
                spawnPosition,
                Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f)), // Random rotation
                transform
                );

            // Name and add actor to group
            newActor.name = "Actor " + i;
            newActor.Initialise(this);
            actors.Add(newActor);
        }
    }

    void Update()
    {
        foreach (GroupActor actor in actors)
        {
            // Create list of objects near actor
            List<Transform> neighbours = GetNearbyObjects(actor);

            // Calculate vector using neighbours and rule
            Vector2 move = rule.CalculateMove(actor, neighbours, this);
            move *= driveFactor;

            // Prevent actors from moving faster than max speed
            if (move.sqrMagnitude > squareMaxSpeed)
            {
                move = move.normalized * maxSpeed;
            }

            // Make move using new velocity
            actor.Move(move);
        }
    }

    /// <summary>
    /// Gets a list of nearby objects.
    /// </summary>
    /// <param name="actor">Selected actor</param>
    /// <returns>List of nearby object transforms.</returns>
    List<Transform> GetNearbyObjects(GroupActor actor)
    {
        List<Transform> neighbours = new List<Transform>();
        Collider2D[] neighbourColliders = Physics2D.OverlapCircleAll(actor.transform.position, neighbourRadius); // Array of colliders within neighbour radius

        // Add transforms to list
        foreach (Collider2D c in neighbourColliders)
        {
            // Ignore actual actor
            if (c != actor.ActorCollider)
            {
                neighbours.Add(c.transform);
            }
        }

        return neighbours;
    }
}
