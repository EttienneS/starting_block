# starting_block
Project is intended to be a starting point for grid based games.  This project is updated as I learn and complete parts from other projects to help accelerate future projects to not have to re-invent the wheel each time.

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

