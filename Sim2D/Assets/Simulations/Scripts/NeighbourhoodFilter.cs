using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NeighbourhoodFilter : ScriptableObject
{
    /// <summary>
    /// Abstract method for filtering object transforms from an original list.
    /// </summary>
    /// <param name="actor">Selected actor</param>
    /// <param name="original">Original list of transforms</param>
    /// <returns>Filtered list of transforms.</returns>
    public abstract List<Transform> Filter(GroupActor actor, List<Transform> original);
}
