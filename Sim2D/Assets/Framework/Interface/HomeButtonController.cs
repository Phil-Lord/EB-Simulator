using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class HomeButtonController : MonoBehaviour
{
    public StartController startController;
    public GameObject obstacles;

    public void OnHomeButtonPress()
    {
        startController.Start();
    }

    public void OnRestartButtonPress()
    {
        Group oldGroup;

        // Loop through each simulation to find active one
        foreach (Transform sim in GameObject.Find("Simulations").transform)
        {
            if (sim.gameObject.activeInHierarchy)
            {
                // Restart each active group in the simulation
                foreach (Transform group in sim)
                {
                    if (group.gameObject.activeInHierarchy)
                    {
                        // Destroy child actors
                        foreach (Transform actor in group)
                        {
                            Destroy(actor.gameObject);
                        }

                        // Restart group
                        oldGroup = (Group)group.GetComponent(typeof(MonoBehaviour));
                        oldGroup.Start();
                    }
                }
                break; // Break after active sim found
            }
        }
    }

    public void OnObstaclesButtonPress()
    {
        bool obstaclesActive = obstacles.activeInHierarchy;
        obstacles.SetActive(!obstaclesActive);
        GameObject.Find("btnObstacles").GetComponentInChildren<Text>().text = obstaclesActive ? "Show Obstacles" : "Hide Obstacles";
    }
}
