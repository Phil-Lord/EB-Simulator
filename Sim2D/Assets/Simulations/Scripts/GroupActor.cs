using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class GroupActor : MonoBehaviour
{
    // Actor's group
    Group actorGroup;
    public Group ActorGroup { get { return actorGroup; } }

    // Actor's collider
    Collider2D actorCollider;
    public Collider2D ActorCollider { get { return actorCollider; } }

    void Start()
    {
        // Set actor's collider on start
        actorCollider = GetComponent<Collider2D>();
    }

    public void Initialise(Group group)
    {
        // Set actor's group when initialised
        actorGroup = group;
    }

    /// <summary>
    /// Move an object using a given velocity
    /// </summary>
    /// <param name="velocity">Directional speed of actor</param>
    public void Move(Vector2 velocity)
    {
        // Change actor's direction according to new velocity
        transform.right = velocity;

        // Move actor x and y using velocity
        transform.position += (Vector3)velocity * Time.deltaTime;
    }
}
