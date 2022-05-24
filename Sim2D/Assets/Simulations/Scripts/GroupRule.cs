using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GroupRule : ScriptableObject
{
    /// <summary>
    /// Abstract method for calculating an actor's vector, override in specific rule classes.
    /// </summary>
    /// <param name="actor">Selected actor</param>
    /// <param name="neighbours">Objects near actor</param>
    /// <param name="group">Actor's group</param>
    /// <returns>A new vector for the selected actor adjusted using rule.</returns>
    public abstract Vector2 CalculateMove(GroupActor actor, List<Transform> neighbours, Group group);
}
