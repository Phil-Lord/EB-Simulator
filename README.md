# Emergent Behaviour Simulator (CMP3753M)
## Preamble
This repository serves as a submission of my dissertation project artefact for UoL. It contains the Unity project, code, and build at the point of hand-in, copied from a private repository which has been used for VC throughout the project. This allows future developments to be made in the private repository, while retaining the submitted version of the project.

## Introduction
This project provides an object-oriented framework for developing and visualising simulations of [emergent behaviour](https://www.researchgate.net/publication/233883398_Autonomous_Systems_with_Emergent_Behaviour). It is intended as an educational tool to aid students and lecturers throughout the delivery of the newly proposed “Natural Computing” module. This will enable students to analyse and understand complex actor-based systems, as well as providing lecturers with an open-source framework for implementing newly relevant simulations. This documentation explains how to make use of the framework, from interacting with the application to developing simulations.

## Requirements
The minimum system requirements for the application depend on the complexity of simulations implemented. While a powerful CPU and dedicated GPU are beneficial, these are not necessary to run most simulations until several hundred actors are involved. As for developing simulations, the framework has been developed with Unity 2020 and written in C#. Therefore, Unity ([system requirements](https://docs.unity3d.com/2020.1/Documentation/Manual/system-requirements.html)) is required, as well as an IDE that supports C#, such as Visual Studio. The framework has been developed in and for Windows, and is yet to be tested in other operating systems.

## Installation and Application Use
Cloning or downloading this repository will provide the Unity project and a working build containing two premade simulations: [boids](https://www.red3d.com/cwr/boids/) and [random walk](https://en.wikipedia.org/wiki/Random_walk). Run the 'EB Simulator.exe' file in the 'Build' folder to get an understanding of how the application works. On start, the application shows a main menu which lists all implemented simulations. Pressing the 'Load' button of a simulation in the list will load it, as well as a panel for adjusting predefined parameters and a button for showing and hiding obstacles. The simulation can be restarted with the 'Restart' button, and exited with the 'Home' button, allowing another simulation to be selected.

## Developing Simulations
The process for adding new simulations to the application is simple, but consists of several moving parts. This section explains these parts and aids you through the development of simulations without going too in-depth on the framework's ins and outs; there is ample commenting within the code for this. Start developing simulations by opening the project in Unity (the first time may take a minute), navigating to 'Framework > Scenes' in the project assets, and double click the 'Simulator' scene.

### Groups
Each simulation consists of at least one group `GameObject`; these groups are populated with actor prefabs upon instantiation. To implement a new simulation and group, follow the steps below.

  1. Right click the 'Simulations' object in the hierarchy and select 'Create Empty'; this is your new simulation, so name it appropriately (e.g. Boids).
  2. Right click your simulation object and select 'Create Empty'; this will be a group within your simulation, name it appropriately (e.g. Flock).
  3. In the project assets, head to 'Simulations > Scripts', then drag and drop the 'Group' script onto your new group object.

This group script allows the group object to be given an actor prefab, rules, memory, and parameters. As well as functionalities for creating encapsulated actors (`Group.Start()`) and moving them (`Group.Update()`).

### Prefabs
A group is populated by instantiating a specific prefab within itself a number of times; in order to work properly, each prefab must contain a `Collider2D` component and a `GroupActor` script component. To create an actor prefab to be used in your group, follow the steps below.

  1. In the project assets, go to 'Simulations > Actor Prefabs'.
  2. Right click and select 'Create > Prefab', then name it appropriately (e.g. CircleActor).
  3. Open the new prefab, add a 2D sprite object to it, and customise the sprite how you please.
  4. Add a suitable `Collider2D` component to your shape (e.g. `Circle Collider 2D`).
  5. Next, go to 'Simulations > Scripts' and drag and drop the 'GroupActor' script onto the new prefab via the inspector.

This group actor script provides instantiated versions of this prefab with the functionality required to move and follow rules. Drag and drop the new prefab onto the 'Actor Prefab' field in your group's inspector; this will now be the prefab which is instantiated a number of times when the simulation starts.

### Rules
Group rules define the behaviours of the group actors and are the most complex part of the process. To make a new rule and prepare it for development, follow the steps below.

  1. Create a script in the project assets under 'Simulations > Rule Scripts' and name it appropriately (e.g. 'AvoidanceRule').
  2. Open the script and delete the `Start()` and `Update` methods.
  3. Change the base class from `MonoBehaviour` to `GroupRule`.
  4. Add the following line above the class declaration, replacing X with your rule name: `[CreateAssetMenu(menuName = "Group/Rule/X")]`.
  5. Declare and override the abstract method `CalculateMove` inherited from the `GroupRule` parent.
  6. Return a 2D vector of zeros for now.

The script should now contain the following code:

```C#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Group/Rule/X")]
public class X : GroupRule
{
    public override Vector2 CalculateMove(GroupActor actor, List<Transform> neighbours, Group group)
    {
        // Rule code here
        return Vector2.zero;
    }
}
```

Now that the rule has been made, its time to attach it to the group. The 'Rule' field of the group script on the group object requires an object of type 'GroupRule'. The `CreateAssetMenu()` line we implemented allows for the creation of a 'GroupRule' object prefab in the project assets. Go to 'Simulations > 'Rule Objects' in the assets, right click, then select 'Create > Group > Rule > RULE NAME'. Drag and drop this newly created 'GroupRule' prefab onto the 'Rule' field of your group. Your group now has everything it needs to run properly, try playing the project in unity and navigating to your simulation in the main menu. The screen should fill with randomly instantiations of your actor prefab.

The reason why the actors are stationary is because the vector returned by the polymorphically overriden `CalculateMove()` method in the rule script is used by the 'Group' object to determine how far to move each 'GroupActor' object per tick and in which direction. As the method currently returns the vector `(0, 0)`, the actors will remain still. It is up to you to define how this vector is calculated when provided with the actor, its neighbours, and its group as parameters. For inspiration, check out the pre-implemented boid and random walk simulation rule scripts.

#### Compound Rules
A compound rule is a rule that has been implemented as previously described, but contains a public array of `GroupRule` objects which it combines the calculated movements of into a single vector. Each rule within the compound rule is assigned a weight which it is multiplied by, determining how much each rule impacts the overall movement. In order to implement a compound rule, go to 'Simulations > Rule Objects' in the project assets, right click, then select 'Create > Group > Rule > Compound' and name it appropriately. Add and remove rule fields with the buttons in the inspector, then drag and drop rules into them and adjust the weights. To see a working compound rule implementation, select 'Purple Flock Rule'; this compound rule includes the rules required to simulate a flock of boids. Finally, drag and drop the compound rule object onto the group's 'Rule' field, as you would any rule.

### Filters
Filters allow a rule to filter through the neighbours it is passed and select which ones to ignore. In order to make use of filters, the parent in a rule's script must be changed from `GroupRule` to its child: `FilterGroupRule`. This child class contains a public object of type `NeighbourhoodFilter`; objects of this class contain the abstract method `Filter()`, which takes an actor and an original list of actor `Transforms` (which will be the actor's neighbours) to filter through as parameters. In order to create and apply a filter to your rule, follow the steps below.

  1. Change your rule's parent to `FilterGroupRule` in its script.
  2. Go to 'Simulations > Filter Scripts' and create a new script; this will contain the filter logic.
  3. Remove `Start()` and `Update()`, change the class' parent to `NeighbourhoodFilter`, and add `[CreateAssetMenu(menuName = Group/Filter/X)]`.
  4. Declare and override the inherited abstract `Filter()` method and add `return original;`.

The script should now contain the following code:

```C#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Group/Filter/X")]
public class X : NeighbourhoodFilter
{
    public override List<Transform> Filter(GroupActor actor, List<Transform> original)
    {
        return original;
    }
}
```

Now that the filter has been implemented, create an object of it under 'Simulations > Filter Objects' in the program assets with 'Create > Group > Filter > X', then drag and drop this object onto the 'Filter' field of your rule. In its current state, the filter will do nothing because it simply returns the list of transforms it was given; you can decide how the filter filters the original list within the filter script, for a working example see either `PhysicsLayerFilter` or `SameGroupFilter`. In order to make use of the filter within the rule, use the following line of code in the rule script:

```C#
neighbours = (filter == null) ? neighbours : filter.Filter(actor, neighbours);
```

### Memory
A group's memory allows actors to store values for future use, such as the random walk actors needing to remember which point they are walking to. To give your group memory, go to 'Simulations > Memory Objects' in the project assets then drag and drop the 'Group Memory' object onto your group's 'Memory' field. Next, go to 'Simulations > Scripts' and open the 'GroupMemory.cs' script; this is where public attributes can be added to provide storage for specific memories. `positionMemory`, a variable of type `IDictionary<int, Vector2>` is already included, see how this is used in the 'RandomWalkRule.cs' script. Access any variables you make in 'GroupMemory' from within rule scripts using 'group.memory.variableName'.

### Parameters
Finally, groups have five public variables which serve as parameters for adjusting actors; these parameters can be altered from both the Unity Editor and within the application while it runs.

  - Starting Count: The number of actor prefabs instantiated within a group upon its start.
  - Drive Factor: Adjusts the level of actor movement when less affected by rules.
  - Max Speed: The maximum speed that the actors will aim to move at
  - Neighbour Radius: The radius of each actor's circle for detecting other nearby objects (neighbours)
  - Avoidance Radius Multiplier: The percentage size of each actor's neighbour radius that it should avoid objects within (0 = no avoidance, 1 = avoid all neighbours).

## Building
Once a simulation has been added (along with groups, rules, memory, and filters), the project can be built and ran, ready to be distributed to students, other lecturers, or anyone who may find it useful.


## Video
The link below leads to a short video showing the implementation of the random walk simulation from start to finish (without rule code typing).

https://youtu.be/UNwQYEj1rmQ
