/// RTS-Project-01 -- Created by D. Sinclair, 2016
/// ================
/// Tile.cs
/// Class used to hold information about a tile within the world grid

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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

    public ObjectType plannedObjectType;                // This is an object which is being planned, w/o the units being told to build it
    public ObjectType queuedObjectType;                 // This is an object which has been queued to be build, and is awaiting installation
    public ObjectType installedObjectType;              // This is an object which has been built into the world, and is fully accessible

    public bool changed;

    private GameObject plannedGameObject;
    private GameObject queuedGameObject;
    private GameObject installedGameObject;

    /// Constructors
    
    public Tile(World world_, Vector2 position_) {
        // Initialise tile attributes
        this.world = world_;
        this.positionV2 = position_;

        plannedObjectType = ObjectType.None;
        queuedObjectType = ObjectType.None;
        installedObjectType = ObjectType.None;
    }

    /// Methods

    public bool PlanObjectOnTile(ObjectType objectType_) {
        if (plannedGameObject != null) {
            // Destroy existing plannedGameObject
            Object.Destroy(plannedGameObject);
        }

        // Set new plannedGameObject
        plannedGameObject = WorldController.Instantiate(WorldController.Instance.wall);         // TEMP
        plannedGameObject.AddComponent<PlannedObject>().Initialise(world, this);              // TEMP -- change to use PlannedObject in future
        plannedGameObject.name = objectType_.ToString() + " (Planned) " + positionV2.ToString();

        plannedObjectType = objectType_;

        // Tell the game the tile has changed and should be updated
        Changed();

        return true;
    }

    public bool QueueObjectOnTile(ObjectType objectType_) {
        if (queuedObjectType != ObjectType.None) {
            // There is already a queued object
            Debug.LogError("Cannot queue " + objectType_.ToString() + "at " + positionV2.ToString() + " -- Queued object already exists");
            return false;
        } else if (installedObjectType != ObjectType.None) {
            // There is already an installed object
            Debug.LogError("Cannot queue " + objectType_.ToString() + " at " + positionV2.ToString() + " -- Installation already exists");
            return false;
        }

        // Set new queuedGameObject
        queuedGameObject = WorldController.Instantiate(WorldController.Instance.wall);          // TEMP
        queuedGameObject.AddComponent<QueuedObject>().Initialise(world, this);               // TEMP
        queuedGameObject.name = objectType_.ToString() + " (Queued) " + positionV2.ToString();

        queuedObjectType = objectType_;

        // Tell the game the tile has changed and should be updated
        Changed();

        return true;
    }

    public bool InstallObjectOnTile(ObjectType objectType_) {
        if (installedObjectType != ObjectType.None) {
            // There is already an installed object
            Debug.LogError("Cannot install " + objectType_.ToString() + " at " + positionV2.ToString() + " -- Installation already exists");
            return false;
        } else if (queuedObjectType == ObjectType.None) {
            // There is no queued object
            Debug.LogError("Cannot install " + objectType_.ToString() + " at " + positionV2.ToString() + " -- There is no queued object");
            return false;
        }

        queuedObjectType = ObjectType.None;

        // Instantiate new gameObject installation
        // NOTE: This assumes that the installedGameObject has been destroyed
        installedGameObject = WorldController.Instantiate(WorldController.Instance.wall);           // TEMP
        installedGameObject.AddComponent<InstalledObject>().Initialise(world, this);
        installedGameObject.name = objectType_.ToString() + " (Installed) " + positionV2.ToString();

        installedObjectType = objectType_;

        // Tell the game the tile has changed and should be updated
        Changed();

        return true;
    }

    private void Changed() {
        changed = true;
        WorldController.Instance.changedTiles.Add(this);
    }

}

public enum ObjectType {
    None,
    Demolition,
    Wall
}