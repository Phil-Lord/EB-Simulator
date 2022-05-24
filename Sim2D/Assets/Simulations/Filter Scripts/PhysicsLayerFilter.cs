using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Group/Filter/Physics Layer")]
public class PhysicsLayerFilter : NeighbourhoodFilter
{
    public LayerMask mask;

    /// <summary>
    /// Filters out objects on layers other than the predefined one.
    /// </summary>
    /// <param name="actor">Selected actor</param>
    /// <param name="original">Original list of transforms</param>
    /// <returns>Filtered list of object transforms on predefined layer.</returns>
    public override List<Transform> Filter(GroupActor actor, List<Transform> original)
    {
        List<Transform> filtered = new List<Transform>();

        // Add each item from original transform list to filtered list if
        // the item has the predefined mask
        foreach (Transform item in original)
        {
            if (mask == (mask | (1 << item.gameObject.layer)))
            {
                filtered.Add(item);
            }
        }

        return filtered;
    }
}
