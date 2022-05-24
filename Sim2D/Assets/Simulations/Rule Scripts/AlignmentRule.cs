using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Group/Rule/Alignment")]
public class AlignmentRule : FilterGroupRule
{
    /// <summary>
    /// Calculates the average vector of an actor's neighbours.
    /// </summary>
    /// <param name="actor">Selected actor</param>
    /// <param name="neighbours">Objects near actor</param>
    /// <param name="group">Actor's group</param>
    /// <returns>The average vector of the selected actor's neighbouring actors.</returns>
    public override Vector2 CalculateMove(GroupActor actor, List<Transform> neighbours, Group group)
    {
        // Finds middlepoint between neighbours, maintain current alignment

        // If no neighbours, return actor's current vector
        if (neighbours.Count == 0)
        {
            return actor.transform.right;
        }

        Vector2 alignmentMove = Vector2.zero;

        // Filter neighbours
        List<Transform> neighboursFiltered = (filter == null) ? neighbours : filter.Filter(actor, neighbours);

        // Sum neighbouring object vectors
        foreach (Transform neighbour in neighboursFiltered)
        {
            alignmentMove += (Vector2)neighbour.transform.right;
        }

        // Find average vector
        alignmentMove /= neighbours.Count;
        return alignmentMove;
    }
}
