using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UI;

public class StartController : MonoBehaviour
{
    public GameObject listParent;
    public GameObject listPrefab;
    public List<GameObject> allObjects = new List<GameObject>();

    public void Start()
    {
        allObjects.Clear();
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
            }
        }

        // Set 'Simulations' and 'Interface Canvas' parents to active and 'Obstacles' to unactive
        allObjects.Find(x => x.name == "Obstacles").SetActive(false);
        allObjects.Find(x => x.name == "Simulations").SetActive(true);
        allObjects.Find(x => x.name == "Interface Canvas").SetActive(true);

        // Set 'Interface Canvas' children other than main menu to unactive
        foreach (Transform inter in allObjects.Find(x => x.name == "Interface Canvas").transform)
        {
            if (inter.name == "Main Menu Panel")
            {
                // Destroy previously listed simulations
                foreach (Transform child in inter.GetChild(0).GetChild(1))
                {
                    Destroy(child.gameObject);
                }

                inter.gameObject.SetActive(true);
            }
            else
            {
                inter.gameObject.SetActive(false);
            }
        }

        // List sims and set them to unactive
        ListSims();
    }

    /// <summary>
    /// Populates UI menu body with a row for each group child of 'Simulations' and sets actual simulations to unactive.
    /// </summary>
    void ListSims()
    {
        //Loop through each child transform of Simulations object
        foreach (Transform sim in allObjects.Find(x => x.name == "Simulations").transform)
        {
            GameObject listEntry = Instantiate(listPrefab);             // Instantiate list entry
            listEntry.transform.SetParent(listParent.transform);        // Set parent to menu body
            listEntry.name = "List " + sim.name;                        // Set object name
            listEntry.GetComponentInChildren<Text>().text = sim.name;   // Set prefab's child text box to the sim name
            sim.gameObject.SetActive(false);                            // Set simulation objects to inactive
        }
    }
}
