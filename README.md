# Emergent Behaviour Simulator (CMP3753M)
This repository serves as a submission of my dissertation project artefact for UoL. It contains the Unity project, code, and build at the point of hand-in, copied from a private repository which has been used for VC throughout the project. This allows future developments to be made in the private repository, while retaining the submitted version of the project.

## Documentation
### Introduction
This project provides an object-oriented framework for developing and visualising simulations of [emergent behaviour](https://www.researchgate.net/publication/233883398_Autonomous_Systems_with_Emergent_Behaviour). It is intended as an educational tool to aid students and lecturers throughout the delivery of the newly proposed “Natural Computing” module. This will enable students to analyse and understand complex actor-based systems, as well as providing lecturers with an open-source framework for implementing newly relevant simulations. This documentation explains how to make use of the framework, from interacting with the application to developing simulations.

### Requirements
The minimum system requirements for the application depend on the complexity of simulations implemented. While a powerful CPU and dedicated GPU are beneficial, these are not necessary to run most simulations until several hundred actors are involved. As for developing simulations, the framework has been developed with Unity 2020 and written in C#. Therefore, Unity ([system requirements](https://docs.unity3d.com/2020.1/Documentation/Manual/system-requirements.html)) is required, as well as an IDE that supports C#, such as Visual Studio. The framework has been developed in and for Windows, and is yet to be tested in other operating systems.

### Setup
Cloning this repository will provide the Unity project and a working build containing two premade simulations: [boids](https://www.red3d.com/cwr/boids/) and [random walk](https://en.wikipedia.org/wiki/Random_walk). Run the build .exe file to get an understanding of how the application works, then open the Unity project begin developing simulations.

### Using the Application
On start, the application shows a main menu which lists all implemented simulations. Pressing the 'Load' button of a simulation in the list will load it, as well as a panel for adjusting predefined parameters. The simulation can be restarted with the 'Restart' button, and exited with the 'Home' button, allowing another simulation to be selected.

### Developing Simulations
#### Groups
#### Prefabs
#### Rules
#### Memories
#### Parameters

### Building
