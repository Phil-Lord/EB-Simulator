using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple random walk implementation (unused)
/// </summary>
public class Walker : MonoBehaviour
{
    // Specify target object and speed
    [SerializeField] private GameObject target;
    [SerializeField] private float speed = 1.5f;

    void Start()
    {
        NewTargetPosition();
    }

    void Update()
    {
        // Move towards target position
        transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
    }

    /// <summary>
    /// Set target object's position to a new random vector
    /// </summary>
    private void NewTargetPosition()
    {
        bool targetSpawned = false;
        while (!targetSpawned)
        {
            // Find random position
            Vector3 targetPosition = new Vector3(Random.Range(-8f, 8f), Random.Range(-4.6f, 4.6f), 0f);
            
            // Find new random position if magnitude of distance between
            // target and actor is < 3 to spread out targets
            if ((targetPosition - transform.position).magnitude < 3)
            {
                continue;
            }
            else
            {
                target.transform.position = targetPosition;
                targetSpawned = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Change target position when the actor collides with it
        Debug.Log("Collision");
        NewTargetPosition();
    }
}
