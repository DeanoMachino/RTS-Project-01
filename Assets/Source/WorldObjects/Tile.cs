/// RTS-Project-01 -- Created by D. Sinclair, 2016
/// ================
/// Tile.cs
/// Class used to hold information about a tile within the world grid

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tile {
    /// Variables

    public Vector2 positionV2 {
        get;
        protected set;
    }

    public Vector3 positionV3 {
        get {
            return new Vector3(positionV2.x, 0f, positionV2.y);
        }
    }

    private World world;
    private List<TileStatus> tileStatuses;

    /// Constructors
    
    public Tile(World world_, Vector2 position_) {
        // Initialise tile attributes
        this.world = world_;
        this.positionV2 = position_;

        // Create and empty tile status list
        tileStatuses = new List<TileStatus>();
    }

    /// Methods

    public bool HasStatus(TileStatus status_) {
        return tileStatuses.Contains(status_);
    }

    public void InstallObjectOnTile(GameObject go_) {
        GameObject go = WorldController.Instantiate(go_);
        go.GetComponent<InstalledObject>().Initialise(world, this);
        tileStatuses.Add(TileStatus.HasInstallation);
    }

}

public enum TileStatus {
    Empty,
    HasInstallation
}