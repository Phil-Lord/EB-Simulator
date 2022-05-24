using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Group/Rule/Boundary Radius")]
public class BoundaryRadiusRule : GroupRule
{
    public Vector2 centre;      // Centre of the circle to stay within, 0, 0 by default
    public float radius = 15f;  // Radius of circle to stay within

    /// <summary>
    /// Calculates a vector required to keep an actor within a radius.
    /// </summary>
    /// <param name="actor">Selected actor</param>
    /// <param name="neighbours">Objects near actor</param>
    /// <param name="group">Actor's group</param>
    /// <returns>A vector to keep the actor within a defined radius from a defined point.</returns>
    public override Vector2 CalculateMove(GroupActor actor, List<Transform> neighbours, Group group)
    {
        // Calculate the opposite x and y of the actor's offset from the centre
        // E.g. centre = 0,0, actor = 2,3, opposite = -2,-3
        Vector2 centreOffset = centre - (Vector2)actor.transform.position;

        // Divide offset position by radius
        // If t = 0, actor is in the centre; if t = 1, actor is at the edge of the circle
        float t = centreOffset.magnitude / radius;

        // If actor is > 10% of the radius away from the circle's edge, make no change
        if (t < 0.9f)
        {
            return Vector2.zero;
        }

        // Calculate offset * percentage of radius from centre ^2
        // A vector guiding actor away from the circle's edge
        return centreOffset * t * t;
    }
}
