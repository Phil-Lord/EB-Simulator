using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

/// <summary>
/// Gets all objects and organises them into lists (unused)
/// </summary>
public class AllObjects : ScriptableObject
{
    public List<GameObject> allObjects = new List<GameObject>();
    public List<GameObject> simulationObjects = new List<GameObject>();
    public List<GameObject> obstacleObjects = new List<GameObject>();
    public List<GameObject> interfaceObjects = new List<GameObject>();

    public AllObjects()
    {
        bool objectOnDisk = false;

        // Create list of all objects in the scene
        foreach (GameObject go in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
        {
#if UNITY_EDITOR
            // If running in unity (not build), check for disk objects (assets etc.)
            objectOnDisk = EditorUtility.IsPersistent(go.transform.root.gameObject);
#endif
            // Only look at objects in the scene
            if (!objectOnDisk && !(go.hideFlags == HideFlags.NotEditable || go.hideFlags == HideFlags.HideAndDontSave))
            {
                allObjects.Add(go);

                // Organise objects into lists
                if (go.transform.name == "Simulations")
                {
                    simulationObjects.Add(go);
                }
                else if (go.transform.name == "Obstacles")
                {
                    obstacleObjects.Add(go);
                }
                else if (go.transform.name == "Interface Canvas")
                {
                    interfaceObjects.Add(go);
                }
            }
        }
    }
}
