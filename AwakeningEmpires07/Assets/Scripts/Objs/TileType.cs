using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileType {

    public string name;
    public GameObject tilePrefab;
    public bool isPassable = true;
    public float movementCost = 1;
}
