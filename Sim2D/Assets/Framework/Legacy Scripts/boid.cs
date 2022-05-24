using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple boid screen wrapping implementation (unused)
/// </summary>
public class boid : MonoBehaviour
{
    Renderer[] renderers;
    bool isWrappingX = false;
    bool isWrappingY = false;
    public float movementSpeed = 5.0f;

    void Start()
    {
        // Get boid renderers
        renderers = GetComponentsInChildren<Renderer>();
    }

    void Update()
    {
        // Move boid in straight line
        transform.position += transform.right * Time.deltaTime * movementSpeed;
        ScreenWrap();
    }

    bool CheckRenderers()
    {
        // Check if boid renderers are visible to the camera
        foreach(var renderer in renderers)
        {
            if (renderer.isVisible)
            {
                return true;
            }
        }

        return false;
    }

    void ScreenWrap()
    {
        bool isVisible = CheckRenderers();

        // If renderer is visible, no wrapping needed
        if (isVisible)
        {
            isWrappingX = false;
            isWrappingY = false;
            return;
        }

        if(isWrappingX && isWrappingY)
        {
            return;
        }
        
        // Get camera and boid position
        var cam = Camera.main;
        var viewportPosition = cam.WorldToViewportPoint(transform.position);
        var newPosition = transform.position;

        // Reposition boids to the other side of the screen when wrapping
        if (!isWrappingX && (viewportPosition.x > 1 || viewportPosition.x < 0))
        {
            newPosition.x = -newPosition.x;

            isWrappingX = true;
        }

        if (!isWrappingY && (viewportPosition.y > 1 || viewportPosition.y < 0))
        {
            newPosition.y = -newPosition.y;

            isWrappingY = true;
        }

        transform.position = newPosition;
    }
}
