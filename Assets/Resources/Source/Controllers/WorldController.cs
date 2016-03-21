/// RTS-Project-01 -- Created by D. Sinclair, 2016
/// ================
/// WorldController.cs
/// Class used to handle the world within the scene

using UnityEngine;
using System.Collections;

public class WorldController : MonoBehaviour {
    /// Variables

    public static WorldController Instance { get; protected set; }      // Static instance of the WorldController for accessing outwith the class
    public World world { get; protected set; }                          // The game world and world data

    public GameObject tile;     // TEMP
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
                tile_go.transform.position = new Vector3(world.grid[x, y].position.x, 0, world.grid[x, y].position.y);
            }
        }

	
	}
	
	// Update is called once per frame
	void Update () {

        UpdateMouseRay();
	
	}

    void UpdateMouseRay() {
        RaycastHit hit;     // hit is the object which has been hit via the ray
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 100f)) {
            // If left mouse button has been pressed
            if (Input.GetMouseButton(0)) {
                Debug.Log(hit.transform.position);

                // TODO: Check what mode we are in, and whether to show context menu or ...

            }
        }
    }
}
