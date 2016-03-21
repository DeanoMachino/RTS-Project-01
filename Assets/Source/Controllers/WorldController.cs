/// RTS-Project-01 -- Created by D. Sinclair, 2016
/// ================
/// WorldController.cs
/// Class used to handle the world within the scene (a sort of game manager)

using UnityEngine;
using System.Collections;

public class WorldController : MonoBehaviour {
    /// Variables

    public static WorldController Instance { get; protected set; }      // Static instance of the WorldController for accessing outwith the class
    public World world { get; protected set; }                          // The game world and world data
    public InteractionMode interactionMode;                             // What mode of interaction the player is currently int

    public GameObject tile;     // TEMP
    public GameObject wall;     // TEMP
    /// Constructors

    /// Methods
    
	// Use this for initialization
	void Start () {
        // Check there isn't already a WorldController
        if (Instance != null) {
            Debug.LogError("There should only ever be one WorldController.", this);
        }

        // Set the static Instance of WorldController
        Instance = this;

        // Initialise the game world and create a GameObject for each of the tiles
        world = new World(100, 100);

        for (int x = 0; x < world.width; x++) {
            for (int y = 0; y < world.height; y++) {
                // TEMP: Create tile GameObjects
                GameObject tile_go = Instantiate(tile) as GameObject;
                tile_go.transform.position = world.grid[x, y].positionV3;
            }
        }

        interactionMode = InteractionMode.BuildMode;
	
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKey(KeyCode.Alpha1)) {
            interactionMode = InteractionMode.BuildMode;
        } else if (Input.GetKey(KeyCode.Alpha2)) {
            interactionMode = InteractionMode.InteractMode;
        }

        UpdateMouseRay();
	
	}

    void UpdateMouseRay() {
        RaycastHit hit;     // hit is the object which has been hit via the ray
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 100f)) {
            // If left mouse button has been pressed
            if (Input.GetMouseButton(0)) {
                Debug.Log(hit.transform.position);

                if (interactionMode == InteractionMode.BuildMode) {
                    if (!world.grid[(int)hit.transform.position.x, (int)hit.transform.position.z].HasStatus(TileStatus.HasInstallation)) {
                        world.grid[(int)hit.transform.position.x, (int)hit.transform.position.z].InstallObjectOnTile(wall);
                    }
                }
                // TODO: Check what mode we are in, and whether to show context menu or ...

            }
        }
    }

    void OnGUI() {

        GUI.Label(new Rect(10.0f, 10.0f, 100.0f, 20.0f), interactionMode.ToString());
    }
}

public enum InteractionMode {
    BuildMode,
    InteractMode
}