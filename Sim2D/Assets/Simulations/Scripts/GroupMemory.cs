using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupMemory : ScriptableObject
{
    public IDictionary<int, Vector2> positionMemory = new Dictionary<int, Vector2>();
}