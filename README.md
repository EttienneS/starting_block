# starting_block
Project is intended to be a starting point for grid based games.  This project is updated as I learn and complete parts from other projects to help accelerate future projects to not have to re-invent the wheel each time.

## Getting Started

### Part 1: Get a clean URP Unity project

To not have thousands of version specific files that Unity uses in this template we need to have a base template for a URP project to work to do that follow these steps:

- Create a new Unity Project with the same name using the Unity Hub with the Universal Render Pipeline template
- Wait for the process to complete 
- After Unity has loaded up delete everything in the Assets folder save the project and close Unity

### Part 2: Get template

Now we can copy use the template and get the files onto the box:

- Create a copy of this template (Use this template button in this repository)
- Give your version a name
- Pull your repository so that the files exist on your machine somewhere

### Part 3: Combine

Next we combine the template with the Unity files to have a working instance of Starting_block

- Copy all the folders from your URP template project (Library, Packages, ProjectSettings, UserSettings etc) to the root of the pulled template (this takes a while)
- After this is complete open Unity Hub again and now Add a project selecting the root of the pulled project folder

### Part 4: Finalize

Unity does not always link up everything as we wanted, to troubleshoot the TestScene:

- Ensure the BootStrapper object has a linked script
- Ensure the SimpleMapGen object has a linked script and has a link to the ColorMaterial
- Ensure the CameraRig object has a linked script and has a reference to the Camera sub object
- If everything is magenta open the Project Settings, go to Graphics and at the top choose a render pipeline (HighQuality should be fine), 
- Ensure that all the pipelines in the Settings folder has the ForwardRenderer selected in the list
- Ensure materials are linked to the actual graphs

- Configure build by uploading activation sercret (see below)

## Completed Features

### Custom chunk based terrain engine (from Karthus project).  

Generate 3D meshes based on input.  Meshes are automatically split into chunks and offer a 3D square cell look.  Implemented with a single ChunkManager MonoBehavior that automatically manages the chunks based on the input data.  The ChunkManager also links the cells and creates a singleton pathfinder that can be used for easy pathfinding.

Chunks detect cells and raise an event in the CellEventManager that can be subscribed to for interaction with the mesh.  Chunks can also be refreshed on the fly  for map modifications.

### Shaders for chunks (from Karthus project)

Two complete shaders are included, a simple one to just show the assigned color of the cell.  The other is a more complicated ChunkShader that takes 5 textures and blends them based on ARGB.

### A* pathfinder (from stolen-lands project)

Generic A* pathfinder that can be applied to any object that implements the IPathableCell interface.  The Pathfinder can also be created as a singleton MonoBehaviour that calculates paths in background tasks (only one at a time) and offers them as they complete while keeping a constant framerate.  

This stops hitching on large maps or when paths no longer exist.

### Strategy game camera (from Karthus project)

An implementation (with minor enhancements) of the [Game Dev Guide](https://www.youtube.com/watch?v=rnqF6S7PfFA) strategy game controller.  This camera controller that appears to rotate the world instead of rotating the camera like a FPS.

### Service Locator (from Karthus project)

Doing proper dependency injection in Unity is very hard as the state is managed in a weird way.  A way to get around this is to use the Service Locator pattern, using this method a set of services can be defined (and instantiated in sequence) at start time, the Locator can then be invoked to find the service without the FindComponent<> overhead that Unity has. 

Still not as clean as a proper DI framework but much better than having to manage singleton states in every component.  

NOTE: When using this it is better to use the IGameService's Initialize method instead of Start and Awake as you are ensured that required stuff will be loaded by that time.

### GitHub Build

Preconfigured GitHub build workflow.  To configure this, edit the main.yml file (if needed) and run the activation.yml, you will need to upload the file generated there to https://license.unity3d.com/manual and get a manual acitvation file that will be stored as a secret in your GitHub repo (UNITY_LICENSE)

## Upcoming Features

- Enhancements to the cell event manager for more selection options
- Mobile support for the game camera
- Dynamic LOD for chunks based on distance to camera

## Future Plans

- Enhance locator to work more like a DI by actually having it construct objects and injecting state.
- Make implementing the template simpler.
