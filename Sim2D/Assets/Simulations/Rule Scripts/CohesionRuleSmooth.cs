using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Group/Rule/Cohesion Smooth")]
public class CohesionRuleSmooth : FilterGroupRule
{
    Vector2 currentVelocity;
    public float actorSmoothTime = 0.5f;

    /// <summary>
    /// Calculates an offset average position of an actor's neighbours (with smooth dampening).
    /// </summary>
    /// <param name="actor">Selected actor</param>
    /// <param name="neighbours">Objects near actor</param>
    /// <param name="group">Actor's group</param>
    /// <returns>The average position of an actor's neighbours, offset based on its position.</returns>
    public override Vector2 CalculateMove(GroupActor actor, List<Transform> neighbours, Group group)
    {
        // If no neighbours, return actor's current vector
        if (neighbours.Count == 0)
        {
            return Vector2.zero;
        }

        Vector2 cohesionMove = Vector2.zero;

        // Filter neighbours
        List<Transform> neighboursFiltered = (filter == null) ? neighbours : filter.Filter(actor, neighbours);

        // Sum neighbouring object positions
        foreach (Transform neighbour in neighboursFiltered)
        {
            cohesionMove += (Vector2)neighbour.position;
        }

        // Find average position
        cohesionMove /= neighbours.Count;

        // Offset average position using actor's current position and smooth damper for smoothness
        cohesionMove -= (Vector2)actor.transform.position;
        cohesionMove = Vector2.SmoothDamp(actor.transform.right, cohesionMove, ref currentVelocity, actorSmoothTime);
        return cohesionMove;
    }
}
