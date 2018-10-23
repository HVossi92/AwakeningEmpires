using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TileMap : MonoBehaviour {

    private GameObject selectedFleet;
    public TileType[] tileTypes;
    private MouseManager mouseManager;
    public GameObject mouseObject;
    int[,] tiles;
    Node[,] graph;
    private GameObject parentFolder;
    public GameObject homeBaseP1;
    public GameObject homeBaseP2;

    int mapSizeX = 19;
    int mapSizeZ = 19;

    // Get other Scripts before void Start
    private void Awake()
    {
        mouseManager = mouseObject.GetComponent<MouseManager>();        
    }

    // Use this for initialization (Setting up the tile map)
    void Start ()
    {
        parentFolder = GameObject.Find("TileMap_Tiles");

        if (!parentFolder)
        {
            parentFolder = new GameObject("TileMap_Tiles");
        }

        GenerateMapData();
        GeneratePathFindingGraph();
        GenerateMapVisuals();
    }    

    // Update is called once per frame
    void Update()
    {
        if (mouseManager.selectedObject != null && mouseManager.selectedObject.transform.childCount > 0)
        {
            if (mouseManager.selectedObject.transform.GetChild(0).name == "FleetMesh")
            {
                GameObject activeObj = mouseManager.selectedObject;
                selectedFleet = activeObj;
            }
        }
        else
        {
            selectedFleet = null;
        }        
    }    

    #region -------------------------- ||| Generate Map ||| ----------------------------------
    // Actual Map Data
    private void GenerateMapData()
    {
        // Allocate map tiles
        tiles = new int[mapSizeX, mapSizeZ];

        int x, z;
        
        // Create and position Homebases
        homeBaseP1 = (GameObject)Instantiate(homeBaseP1, new Vector3(-3, -1, mapSizeZ / 2 - .5f), Quaternion.identity);
        homeBaseP2 = (GameObject)Instantiate(homeBaseP2, new Vector3(mapSizeX + 2, -1, mapSizeZ / 2 - .5f), Quaternion.identity);

        #region -------------------------- ||| Map Tiles layout ||| ----------------------------------
        // Initialize tiles as space
        for (x = 0; x < mapSizeX; x++)
        {
            for (z = 0; z < mapSizeZ - 3; z++)
            {
                tiles[x, z] = 0;
            }
        }

        // Player 1 Construction Slots
        for (x = 0; x <= 1; x++)
        {
            for (z = 2; z <= 15; z++)
            {
                tiles[x, z] = 2;
            }
        }

        // Player 2 Construction Slots
        for (x = mapSizeX - 2; x < mapSizeX; x++)
        {
            for (z = 2; z <= mapSizeZ - 3; z++)
            {
                tiles[x, z] = 4;
            }
        }

        // Player 1 Shipyard Slots
        for (x = 2; x <= 2; x++)
        {
            for (z = 2; z <= 15; z++)
            {
                tiles[x, z] = 5;
            }
        }

        // Player 2 Shipyard Slots
        for (x = mapSizeX - 3; x < mapSizeX - 2; x++)
        {
            for (z = 2; z <= mapSizeZ - 3; z++)
            {
                tiles[x, z] = 6;
            }
        }

        // Asteroids Field
        for (x=8; x <= 10; x++)
        {
            for(z=5; z <= 13; z++)
            {
                tiles[x, z] = 1;
            }
        }
        // Make Impassible BlackSpace
        x = 9;
        z = mapSizeZ - 2;
        while(z > mapSizeZ - 5)
        {
            tiles[x, z] = 3;
            z--;
        }

        x = 9;
        z = 1;
        while (z < 4)
        {
            tiles[x, z] = 3;
            z++;
        }
    }
    #endregion -------------------------- ||| Map Tiles layout ||| ----------------------------------

    public float EnterTileCost(int sourceX, int sourceZ, int targetX, int targetZ)
    {
        TileType tt = tileTypes[tiles[targetX, targetZ]];

        if (!FleetCanEnterTile(targetX, targetZ))
        {
            return Mathf.Infinity;
        }

        float cost = tt.movementCost;

        if (sourceX != targetX && sourceZ != targetZ)
        {
            cost += 0.001f;
        }

        return cost;
    }

    // Spawn visual tiles
    private void GenerateMapVisuals()
    {
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int z = 0; z < mapSizeZ; z++)
            {
                TileType tt = tileTypes[tiles[x, z]];

                GameObject go = (GameObject) Instantiate(tt.tilePrefab, new Vector3(x, 0, z), Quaternion.identity);
                go.transform.parent = parentFolder.transform;                                                           

                ClickTile ct = go.GetComponent<ClickTile>();
                ct.tileX = x;
                ct.tileZ = z;
                ct.map = this;
            }
        }
    }
     // If the World Coords are different, e.g. map movement weirdness
    public Vector3 TileCoordToWorldCoord(int x, int z)
    {
        return new Vector3(x, 0, z);
    }
    #endregion -------------------------- ||| Generate Map ||| ----------------------------------

    #region -------------------------- ||| Path Finding stuff ||| ----------------------------------
    // 'Dijkstra-Algorithm' to find the shortest path

    public bool FleetCanEnterTile(int x, int z)
    {
        // Possbility to test different Fleets against different Tiles, e.g. special abilities to traverse impassible terrain (teleport?)
        return tileTypes[tiles[x, z]].isPassable;
    }

    // Fleet Movement to Mouse Click
    public void GenerateFleetPathTo(int x, int z)
    {
        if (selectedFleet != null)
        {
            //Clear Fleet's old path
            selectedFleet.GetComponent<Fleet>().currentPath = null;

            if (!FleetCanEnterTile(x, z))
            {
                // Clicked on impassible Tile
                return;
            }

            Dictionary<Node, float> dist = new Dictionary<Node, float>();
            Dictionary<Node, Node> prev = new Dictionary<Node, Node>();

            // nodes not yet checked
            List<Node> unvisited = new List<Node>();

            Node source = graph[selectedFleet.GetComponent<Fleet>().tileX,
                                selectedFleet.GetComponent<Fleet>().tileZ];

            Node target = graph[x, z];

            dist[source] = 0;
            prev[source] = null;

            // Initialize everythign with infinity distance for the beginning. Everything is out of reach
            foreach (Node v in graph)
            {
                if (v != source)
                {
                    dist[v] = Mathf.Infinity;
                    prev[v] = null;
                }

                unvisited.Add(v);
            }

            while (unvisited.Count > 0)
            {

                // u is the unvisited node with the smallest distance
                Node u = null;

                foreach (Node possibleU in unvisited)
                {
                    if (u == null || dist[possibleU] < dist[u])
                    {
                        u = possibleU;
                    }
                }

                if (u == target)
                {
                    break; // Exit while loop
                }

                unvisited.Remove(u);

                foreach (Node v in u.neighbours)
                {
                    // float alt = dist[u] + u.DistanceTo(v);
                    float alt = dist[u] + EnterTileCost(v.x, v.z, u.x, u.z);
                    if (alt < dist[v])
                    {
                        dist[v] = alt;
                        prev[v] = u;
                    }
                }
            }

            // At this point the shortest way has been found, or no valid way exists
            if (prev[target] == null)
            {
                // No valid way found
            }

            List<Node> currentPath = new List<Node>();
            Node curr = target;

            // Step through prev chain and add to path
            while (curr != null)
            {
                currentPath.Add(curr);
                curr = prev[curr];
            }

            // currentPath == way from target to source
            currentPath.Reverse();

            selectedFleet.GetComponent<Fleet>().currentPath = currentPath;
        }
    }
    private void GeneratePathFindingGraph()
    {
        // Initialize array
        graph = new Node[mapSizeX, mapSizeZ];

        // initialize Node for each array item        
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int z = 0; z < mapSizeZ; z++)
            {
                graph[x, z] = new Node();
                graph[x, z].x = x;
                graph[x, z].z = z;
            }
        }
        // calc neighbours of all nodes
        for (int x = 0; x < mapSizeX; x++)
            {
            for (int z = 0; z < mapSizeZ; z++)
                {
                
                // 4 side connected tiles
                if (x > 0)
                    graph[x, z].neighbours.Add(graph[x - 1, z]);
                if (x < mapSizeX - 1)
                    graph[x, z].neighbours.Add(graph[x + 1, z]);

                if (z > 0)
                    graph[x, z].neighbours.Add(graph[x, z - 1]);
                if (z < mapSizeZ - 1)
                    graph[x, z].neighbours.Add(graph[x, z + 1]);

            }
        }
    }
    #endregion -------------------------- ||| Path Finding stuff ||| ----------------------------------    
}
