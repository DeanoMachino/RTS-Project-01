/// RTS-Project-01 -- Created by D. Sinclair, 2016
/// ================
/// World.cs
/// Class used to collate all world data, including world size and attributes

using UnityEngine;
using System.Collections;

public class World {
    /// Variables
    
    public int width {
        get;
        protected set;
    }

    public int height {
        get;
        protected set;
    }

    public Tile[,] grid {
        get;
        protected set;
    }

    /// Constructors
    
    public World(int width_, int height_) {
        // Initialise world size
        this.width = width_;
        this.height = height_;

        // Initialise world grid and create new tiles
        grid = new Tile[width, height];

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                grid[x, y] = new Tile(this, new Vector2(x, y));
            }
        }

    }

    /// Methods
	

}
