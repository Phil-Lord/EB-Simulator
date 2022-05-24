using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    public GameObject controlPrefab;
    GameObject controlParent;
    Transform selectedSim;
    Group simGroup;

    // Sliders
    Slider sliderCount;
    Slider sliderDrive;
    Slider sliderSpeed;
    Slider sliderNeighbourRadius;
    Slider sliderAvoidRadius;
    
    // Slider value text
    Text countValueText;
    Text driveValueText;
    Text speedValueText;
    Text neighbourValueText;
    Text avoidValueText;

    /// <summary>
    /// Loads a simulation when its button is pressed.
    /// </summary>
    public void OnButtonPress()
    {
        bool objectOnDisk = false;

        // Loop through all game objects
        foreach (GameObject go in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
        {
#if UNITY_EDITOR
            // If running in unity (not build), check for disk objects (assets etc.)
            objectOnDisk = EditorUtility.IsPersistent(go.transform.root.gameObject);
#endif
            // Only look at objects in the scene
            if (!objectOnDisk && !(go.hideFlags == HideFlags.NotEditable || go.hideFlags == HideFlags.HideAndDontSave))
            {
                // Control menu active state
                if (go.transform.name == "Interface Canvas")
                {
                    foreach (Transform inter in go.transform)
                    {
                        // Set simulation control menu to active and the rest to inactive
                        if (inter.name == "Sim Menu Panel")
                        {
                            // Destroy previously listed controls
                            foreach (Transform child in inter.GetChild(0).GetChild(1))
                            {
                                Destroy(child.gameObject);
                            }

                            inter.gameObject.SetActive(true);
                            controlParent = inter.GetChild(0).GetChild(1).gameObject; // Set parent to sim menu body UI object
                            break;
                        }
                        else
                        {
                            inter.gameObject.SetActive(false);
                        }
                    }
                }
                // Set simulation to active
                else if (go.transform.name == "Simulations")
                {
                    foreach (Transform sim in go.transform)
                    {
                        // Check for simulation with the same name as the button's parent row
                        if (sim.gameObject.name == gameObject.transform.parent.name.Split(new[] { ' ' }, 2)[1])
                        {
                            sim.gameObject.SetActive(true);
                            selectedSim = sim;
                            break;
                        }
                    }
                }
            }
        }

        ListControls();
    }

    void ListControls()
    {
        // Get simulation group
        simGroup = (Group)selectedSim.GetChild(0).GetComponent(typeof(MonoBehaviour));

        // Starting count control
        GameObject controlCount = Instantiate(controlPrefab);                                           // Instantiate control prefab object
        controlCount.transform.SetParent(controlParent.transform);                                      // Set control parent to predefined menu body
        controlCount.name = "Control startCount";                                                       // Set control object name
        controlCount.GetComponentInChildren<Text>().text = "Starting Count:";                            // Set control object text
        countValueText = controlCount.transform.GetChild(1).GetComponentInChildren<Text>();             // Set value text object to text object beneath slider
        sliderCount = controlCount.GetComponentInChildren<Image>().GetComponentInChildren<Slider>();    // Set slider object to prefab slider
        sliderCount.wholeNumbers = true;                                                                // Set slider to whole numbers only
        sliderCount.minValue = 0;                                                                       // Set min and max values according to group variable range
        sliderCount.maxValue = 1000;
        sliderCount.value = simGroup.startingCount;                                                     // Set slider value and text beneath to default variable value
        countValueText.text = sliderCount.value.ToString();
        sliderCount.onValueChanged.AddListener(delegate { AdjustCount(); });                            // Add listener to slider to invoke adjust method on value change

        // Drive factor control
        GameObject controlDrive = Instantiate(controlPrefab);
        controlDrive.transform.SetParent(controlParent.transform);
        controlDrive.name = "Control driveFactor";
        controlDrive.GetComponentInChildren<Text>().text = "Drive Factor:";
        driveValueText = controlDrive.transform.GetChild(1).GetComponentInChildren<Text>();
        sliderDrive = controlDrive.GetComponentInChildren<Image>().GetComponentInChildren<Slider>();
        sliderDrive.minValue = 1;
        sliderDrive.maxValue = 100;
        sliderDrive.value = simGroup.driveFactor;
        driveValueText.text = sliderDrive.value.ToString("0.00");
        sliderDrive.onValueChanged.AddListener(delegate { AdjustDrive(); });
        
        // Max speed control
        GameObject controlSpeed = Instantiate(controlPrefab);
        controlSpeed.transform.SetParent(controlParent.transform);
        controlSpeed.name = "Control maxSpeed";
        controlSpeed.GetComponentInChildren<Text>().text = "Max Speed:";
        speedValueText = controlSpeed.transform.GetChild(1).GetComponentInChildren<Text>();
        sliderSpeed = controlSpeed.GetComponentInChildren<Image>().GetComponentInChildren<Slider>();
        sliderSpeed.minValue = 1;
        sliderSpeed.maxValue = 100;
        sliderSpeed.value = simGroup.maxSpeed;
        speedValueText.text = sliderSpeed.value.ToString("0.00");
        sliderSpeed.onValueChanged.AddListener(delegate { AdjustSpeed(); });
        
        // Neighbour radius control
        GameObject controlNeighbourRadius = Instantiate(controlPrefab);
        controlNeighbourRadius.transform.SetParent(controlParent.transform);
        controlNeighbourRadius.name = "Control neighbourRadius";
        controlNeighbourRadius.GetComponentInChildren<Text>().text = "Neighbour Radius:";
        neighbourValueText = controlNeighbourRadius.transform.GetChild(1).GetComponentInChildren<Text>();
        sliderNeighbourRadius = controlNeighbourRadius.GetComponentInChildren<Image>().GetComponentInChildren<Slider>();
        sliderNeighbourRadius.minValue = 1;
        sliderNeighbourRadius.maxValue = 10;
        sliderNeighbourRadius.value = simGroup.neighbourRadius;
        neighbourValueText.text = sliderNeighbourRadius.value.ToString("0.00");
        sliderNeighbourRadius.onValueChanged.AddListener(delegate { AdjustNeighbourRadius(); });
        
        // Avoidance radius multiplier control
        GameObject controlAvoidRadius = Instantiate(controlPrefab);
        controlAvoidRadius.transform.SetParent(controlParent.transform);
        controlAvoidRadius.name = "Control avoidanceRadiusMultiplier";
        controlAvoidRadius.GetComponentInChildren<Text>().text = "Avoidance Radius Multiplier:";
        avoidValueText = controlAvoidRadius.transform.GetChild(1).GetComponentInChildren<Text>();
        sliderAvoidRadius = controlAvoidRadius.GetComponentInChildren<Image>().GetComponentInChildren<Slider>();
        sliderAvoidRadius.minValue = 1;
        sliderAvoidRadius.maxValue = 10;
        sliderAvoidRadius.value = simGroup.avoidanceRadiusMultiplier;
        avoidValueText.text = sliderAvoidRadius.value.ToString("0.00");
        sliderAvoidRadius.onValueChanged.AddListener(delegate { AdjustAvoidRadius(); });
    }

    // Methods for slider adjustments
    public void AdjustCount()
    {
        countValueText.text = sliderCount.value.ToString();
        simGroup.startingCount = (int)sliderCount.value;
    }
    
    public void AdjustDrive()
    {
        driveValueText.text = sliderDrive.value.ToString("0.00");
        simGroup.driveFactor = sliderDrive.value;
    }
    
    public void AdjustSpeed()
    {
        speedValueText.text = sliderSpeed.value.ToString("0.00");
        simGroup.maxSpeed = sliderSpeed.value;
    }

    public void AdjustNeighbourRadius()
    {
        neighbourValueText.text = sliderNeighbourRadius.value.ToString("0.00");
        simGroup.neighbourRadius = sliderNeighbourRadius.value;
    }

    public void AdjustAvoidRadius()
    {
        avoidValueText.text = sliderAvoidRadius.value.ToString("0.00");
        simGroup.avoidanceRadiusMultiplier = sliderAvoidRadius.value;
    }
}
