/// RTS-Project-01 -- Created by D. Sinclair, 2016
/// ================
/// QueuedObject.cs
/// Class detailed an object in the world which is queued and awaiting installation

using UnityEngine;
using System.Collections;

public class QueuedObject : MonoBehaviour {
    /// Variables
    public World world { get; protected set; }
    public Tile tile { get; protected set; }

    /// Methods

    public void Initialise(World world_, Tile tile_) {
        // Initialise the object attributes
        this.world = world_;
        this.tile = tile_;

        //float yOffset = gameObject.GetComponent<Renderer>().bounds.size.y / 2;
        gameObject.transform.position = new Vector3(tile.positionV3.x, tile.positionV3.y, tile.positionV3.z);

        gameObject.GetComponent<Renderer>().material.color = new Color(0f, 0f, 0.7f, 0.5f);
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (tile.changed) {
            if (tile.queuedObjectType == ObjectType.None) {
                Destroy(gameObject);
            }
        }
	}
}
