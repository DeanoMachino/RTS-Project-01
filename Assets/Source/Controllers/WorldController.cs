/// RTS-Project-01 -- Created by D. Sinclair, 2016
/// ================
/// WorldController.cs
/// Class used to handle the world within the scene (a sort of game manager)

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class WorldController : MonoBehaviour {
    /// Variables

    public static WorldController Instance { get; protected set; }      // Static instance of the WorldController for accessing outwith the class
    public World world { get; protected set; }                          // The game world and world data
    public InteractionMode interactionMode;                             // What mode of interaction the player is currently int
    public List<Tile> changedTiles = new List<Tile>();                  // A list of the tiles which have had a change in the last frame

    private Tile hoveringTile;                                          // The tile which the mouse is currently hovering over
    private List<GameObject> selectionObjects = new List<GameObject>(); // List of build indicator GameObjects

    public GameObject tile;             // TEMP
    public GameObject wall;             // TEMP
    public GameObject buildIndicator;   // TEMP

    private bool orthoCamera;

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

        orthoCamera = false;
        Camera.main.orthographic = orthoCamera;

        interactionMode = InteractionMode.PlanningMode;
	
	}
	
	// Update is called once per frame
    void Update() {

        // Update changed tiles
        foreach (Tile t in changedTiles) {
            t.changed = false;
        }

        // Change interaction mode
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            interactionMode = InteractionMode.PlanningMode;
        } else if (Input.GetKeyDown(KeyCode.Alpha2)) {
            interactionMode = InteractionMode.BuildMode;
        } else if (Input.GetKeyDown(KeyCode.Alpha3)) {
            interactionMode = InteractionMode.InteractMode;
        } else if (Input.GetKeyDown(KeyCode.Alpha4)) {
            interactionMode = InteractionMode.InstallMode;
        }

        // Change camera type
        if (Input.GetKeyDown(KeyCode.Tab)) {
            orthoCamera = !orthoCamera;
            Camera.main.orthographic = orthoCamera;
        }

        UpdateMouseRay();
        if (interactionMode != InteractionMode.InteractMode) {
            UpdateDragSelection();    // TODO: Check what type of object is being placed, and whether a perimeter or area drag should be used
        }
    }

    void UpdateMouseRay() {
        RaycastHit hit;     // hit is the object which has been hit via the ray
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        int layermask = 1 << 8;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layermask)) {
            hoveringTile = GetWorldTile(hit.transform);
        }
    }

    void InteractWithTiles(Vector2 start_, Vector2 end_) {

        for (int x = (int)start_.x; x <= end_.x; x++) {
            for (int y = (int)start_.y; y <= end_.y; y++) {
                Tile tile = GetWorldTile(x, y);

                if (tile != null) {
                    switch (interactionMode) {
                        case InteractionMode.PlanningMode:
                            tile.PlanObjectOnTile(ObjectType.Wall);
                            break;
                        case InteractionMode.BuildMode:
                            tile.QueueObjectOnTile(ObjectType.Wall);
                            break;
                        case InteractionMode.InteractMode:
                            // NOTE: Should never get in here, due to earlier input validation
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
    }

    // MOVE
    Vector2 dragStartPosition;
    // /MOVE

    void UpdateDragSelection() {
        if (hoveringTile != null) {
            Vector2 currentPosition = hoveringTile.positionV2;

            // Check if we're over a UI element
            /*if (EventSystem.current.IsPointerOverGameObject()) {
                return;
            }*/

            if (Input.GetMouseButtonDown(0)) {
                // Set the start point of the drag
                dragStartPosition = hoveringTile.positionV2;
                Debug.Log(dragStartPosition.ToString());
            }

            Vector2 start = dragStartPosition;
            Vector2 end = currentPosition;

            // Swap values if dragging in the wrong direction
            if (end.x < start.x) {
                int temp = (int)end.x;
                end.x = start.x;
                start.x = temp;
            }
            if (end.y < start.y) {
                int temp = (int)end.y;
                end.y = start.y;
                start.y = temp;
            }

            // Clean up old drag previews
            while (selectionObjects.Count > 0) {
                GameObject go = selectionObjects[0];
                selectionObjects.RemoveAt(0);
                Destroy(go);
            }

            if (Input.GetMouseButton(0)) {
                // Display a drag preview
                for (int x = (int)start.x; x <= end.x; x++) {
                    for (int y = (int)start.y; y <= end.y; y++) {
                        Tile t = GetWorldTile(x, y);
                        if (t != null) {
                            // Display hint on tile
                            GameObject go = Instantiate(buildIndicator);
                            go.transform.position = new Vector3(x, 0f, y);
                            go.transform.SetParent(this.transform, true);
                            selectionObjects.Add(go);
                        }
                    }
                }
            }


            if (Input.GetMouseButtonUp(0)) {
                InteractWithTiles(start, end);
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
    private Tile GetWorldTile(int x_, int y_) {
        return world.grid[x_, y_];
    }
}

public enum InteractionMode {
    PlanningMode,
    BuildMode,
    InstallMode,        // TEMP
    InteractMode
}