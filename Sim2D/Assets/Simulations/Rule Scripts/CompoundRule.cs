using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Group/Rule/Compound")]
public class CompoundRule : GroupRule
{
    public GroupRule[] rules;   // Array of rules
    public float[] weights;     // Weights for each rule

    /// <summary>
    /// Calculates a vector based on a combination of weighted rules.
    /// </summary>
    /// <param name="actor">Selected actor</param>
    /// <param name="neighbours">Objects near actor</param>
    /// <param name="group">Actor's group</param>
    /// <returns>A vector representing a movement which considers a combination of rules.</returns>
    public override Vector2 CalculateMove(GroupActor actor, List<Transform> neighbours, Group group)
    {
        // Handle error if there's more weights than rules and vice versa
        if (weights.Length != rules.Length)
        {
            Debug.LogError("Data mismatch in " + name, this);
            return Vector2.zero;
        }

        Vector2 move = Vector2.zero;

        // Iterate through each rule
        for (int i = 0; i < rules.Length; i++)
        {
            // Calculate vector using rule
            Vector2 partialMove = rules[i].CalculateMove(actor, neighbours, group) * weights[i];

            if (partialMove != Vector2.zero)
            {
                // If the magnitude of a rule's vector exceeds its weight,
                // set the magnitude to 1 (normalise) and multiply it by that weight
                if (partialMove.sqrMagnitude > weights[i] * weights[i])
                {
                    partialMove.Normalize();
                    partialMove *= weights[i];
                }

                // Add weighted rule's vector to compound vector
                move += partialMove;
            }
        }

        return move;
    }
}
