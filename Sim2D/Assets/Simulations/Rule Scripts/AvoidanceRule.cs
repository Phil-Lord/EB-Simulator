using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Group/Rule/Avoidance")]
public class AvoidanceRule : FilterGroupRule
{
    /// <summary>
    /// Calculates the vector to avoid an actor's neighbours.
    /// </summary>
    /// <param name="actor">Selected actor</param>
    /// <param name="neighbours">Objects near actor</param>
    /// <param name="group">Actor's group</param>
    /// <returns>The average vector required to avoid all neighbours within avoidance radius.</returns>
    public override Vector2 CalculateMove(GroupActor actor, List<Transform> neighbours, Group group)
    {
        // If no neighbours, return actor's current vector
        if (neighbours.Count == 0)
        {
            return Vector2.zero;
        }

        Vector2 avoidanceMove = Vector2.zero;
        int nAvoid = 0;

        // Filter neighbours
        List<Transform> neighboursFiltered = (filter == null) ? neighbours : filter.Filter(actor, neighbours);

        foreach (Transform neighbour in neighboursFiltered)
        {
            // If the distance between the actor and the current neighbour is < the avoidance radius,
            // Add the distance to the actor's new vector
            if (Vector2.SqrMagnitude(neighbour.position - actor.transform.position) < group.SquareAvoidanceRadius)
            {
                nAvoid++;
                avoidanceMove += (Vector2)(actor.transform.position - neighbour.position);
            }
        }

        // If there's any neighbours to avoid, calculate average vector
        // required to avoid all neighbours
        if (nAvoid > 0)
        {
            avoidanceMove /= nAvoid;
        }

        return avoidanceMove;
    }
}
