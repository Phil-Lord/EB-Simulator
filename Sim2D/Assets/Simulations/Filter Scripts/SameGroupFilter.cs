using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Group/Filter/Same Group")]
public class SameGroupFilter : NeighbourhoodFilter
{
    /// <summary>
    /// Filters out objects from different groups to the selected actor.
    /// </summary>
    /// <param name="actor">Selected actor</param>
    /// <param name="original">Original list of transforms</param>
    /// <returns>Filtered list of object transforms within the selected actor's group.</returns>
    public override List<Transform> Filter(GroupActor actor, List<Transform> original)
    {
        List<Transform> filtered = new List<Transform>();
        
        foreach(Transform item in original)
        {
            GroupActor itemActor = item.GetComponent<GroupActor>();

            // Add object to filered list if is in the selected actor's group
            if (itemActor != null && itemActor.ActorGroup == actor.ActorGroup)
            {
                filtered.Add(item);
            }
        }

        return filtered;
    }
}
