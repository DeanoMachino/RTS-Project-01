/// RTS-Project-01 -- Created by D. Sinclair, 2016
/// ================
/// WorldController.cs
/// Class used to handle the world within the scene (a sort of game manager)

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldController : MonoBehaviour {
    /// Variables

    public static WorldController Instance { get; protected set; }      // Static instance of the WorldController for accessing outwith the class
    public World world { get; protected set; }                          // The game world and world data
    public InteractionMode interactionMode;                             // What mode of interaction the player is currently int
    public List<Tile> changedTiles = new List<Tile>();                  // A list of the tiles which have had a change in the last frame

    public GameObject tile;     // TEMP
    public GameObject wall;     // TEMP

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

        interactionMode = InteractionMode.PlanningMode;
	
	}
	
	// Update is called once per frame
	void Update () {
        foreach (Tile t in changedTiles) {
            t.changed = false;
        }
        if (Input.GetKey(KeyCode.Alpha1)) {
            interactionMode = InteractionMode.PlanningMode;
        } else if (Input.GetKey(KeyCode.Alpha2)) {
            interactionMode = InteractionMode.BuildMode;
        } else if (Input.GetKey(KeyCode.Alpha3)) {
            interactionMode = InteractionMode.InteractMode;
        } else if (Input.GetKey(KeyCode.Alpha4)) {
            interactionMode = InteractionMode.InstallMode;
        }

        UpdateMouseRay();
	
	}

    void UpdateMouseRay() {
        RaycastHit hit;     // hit is the object which has been hit via the ray
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        int layermask = 1 << 8;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layermask)) {
            // If left mouse button has been pressed
            if (Input.GetMouseButtonDown(0)) {
                //Debug.Log(hit.transform.position);

                Tile tile = GetWorldTile(hit.transform);

                switch (interactionMode) {
                    case InteractionMode.PlanningMode:
                        tile.PlanObjectOnTile(ObjectType.Wall);
                        break;
                    case InteractionMode.BuildMode:
                        tile.QueueObjectOnTile(ObjectType.Wall);
                        break;
                    case InteractionMode.InteractMode:
                        // TODO: Show context menus with interaction options
                        break;
                    case InteractionMode.InstallMode:               // TEMP
                        tile.InstallObjectOnTile(ObjectType.Wall);
                        break;
                    default:
                        break;
                }
            }
        }
    }

    void OnGUI() {
        // TEMP -- Display text of current interaction mode
        GUI.Label(new Rect(10.0f, 10.0f, 100.0f, 25.0f), interactionMode.ToString());
    }

    private Tile GetWorldTile(Transform transform_) {
        return world.grid[(int)transform_.position.x, (int)transform_.position.z];
    }
}

public enum InteractionMode {
    PlanningMode,
    BuildMode,
    InstallMode,        // TEMP
    InteractMode
}