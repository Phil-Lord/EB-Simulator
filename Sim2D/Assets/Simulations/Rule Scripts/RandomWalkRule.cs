using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Group/Rule/Random Walk")]
public class RandomWalkRule : GroupRule
{
    /// <summary>
    /// Calculates the vector to move towards a stored target position.
    /// </summary>
    /// <param name="actor">Selected actor</param>
    /// <param name="neighbours">Objects near actor</param>
    /// <param name="group">Actor's group</param>
    /// <returns>Vector required to move towards a stored target position.</returns>
    public override Vector2 CalculateMove(GroupActor actor, List<Transform> neighbours, Group group)
    {
        // Get actor's group ID from its name
        int actorId = int.Parse(actor.name.Split(' ')[1]);

        // If target not reached, calculate and return vector required to move towards target
        if (group.memory.positionMemory.ContainsKey(actorId) && (group.memory.positionMemory[actorId] - (Vector2)actor.transform.position).magnitude > 0.1)
        {
            return group.memory.positionMemory[actorId] - (Vector2)actor.transform.position;
        }
        
        // Create new target position
        Vector2? targetPosition = null;

        // Retry until magnitude of distance between target and actor > 3
        // Spreads out targets
        while (!targetPosition.HasValue || ((Vector2)targetPosition - (Vector2)actor.transform.position).magnitude < 3)
        {
            // Create random x and y in range of screen size
            float x = Random.Range
                (Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x, Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x);
            float y = Random.Range
                (Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).y, Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y);

            targetPosition = new Vector2(x, y);
        }

        // Save target position to actor's value in memory dictionary
        group.memory.positionMemory[actorId] = (Vector2)targetPosition;

        // Calculate vector required to move towards new target
        targetPosition -= (Vector2)actor.transform.position;
        return (Vector2)targetPosition;
    }
}
