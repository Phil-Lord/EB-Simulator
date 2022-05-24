using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FilterGroupRule : GroupRule
{
    /// <summary>
    /// The rule's filter.
    /// </summary>
    public NeighbourhoodFilter filter;
}
