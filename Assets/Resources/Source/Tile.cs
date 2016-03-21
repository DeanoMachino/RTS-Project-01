/// RTS-Project-01 -- Created by D. Sinclair, 2016
/// ================
/// Tile.cs
/// Class used to hold information about a tile within the world grid

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tile {
    /// Variables
    
    public Vector2 position { get; protected set; }

    private World world;
    private List<TileStatus> tileStatuses;

    /// Constructors
    
    public Tile(World world_, Vector2 position_) {
        // Initialise tile attributes
        this.world = world_;
        this.position = position_;

        // Create and empty tile status list
        tileStatuses = new List<TileStatus>();
    }

    /// Methods


}

public enum TileStatus {
    Empty,

}