using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class Fleet : MonoBehaviour {

    [DontSaveMember] public int tileX;
    [DontSaveMember] public int tileZ;
    [DontSaveMember] private GameObject mapObj;
    [DontSaveMember] private GameObject nextTurnObj;
    [DontSaveMember] private NextTurnBTN2 nextTurnBTN2;
    [DontSaveMember] public TileMap map;
    [DontSaveMember] private int curGameRound;
    [DontSaveMember] private int nextGameRound;
    [DontSaveMember] public int fleetOfPlayer;
    [DontSaveMember] public GameObject mouseObj;
    [DontSaveMember] private MouseManager mouseManager;
    [DontSaveMember] private bool postAction = false;

    [DontSaveMember] public List<Node> currentPath = null;
    [DontSaveMember] float remainingMovement;

    [DontSaveMember] GameObject shipHolder;
    [DontSaveMember] List<GameObject> shipChildren;

    public int fleetNumber;
    private GameObject sluObj;
    private SaveLoadUtility slu;

    public virtual int movementSpeed()
    {
        return 2;
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            map.GenerateFleetPathTo(tileX, tileZ);
        }
    }

    private void Awake()
    {
        reassignGameObjs();
    }

    private void Start()
    {
        // Load in SaveLoad Utility
        if (slu == null)
        {
            sluObj = GameObject.Find("Persistent_SaveLoad_Obj_Menu");
            slu = sluObj.GetComponent<SaveLoadUtility>();
            if (slu == null)
            {
                Debug.Log("[SaveLoadMenu] Start(): Warning! SaveLoadUtility not assigned!");
            }
        }

        // Current Game Round = NextTurnBTN Gameround (1)
        curGameRound = nextTurnBTN2.GetComponent<NextTurnBTN2>().gameRound;
        tileX = (int)transform.position.x;
        tileZ = (int)transform.position.z;
    }

    private void Update()
    {        
        if (nextTurnObj == null)
        {
            reassignGameObjs();
        }
        // Current Game Round = NextTurnBTN Gameround (1), but keeps updating, if nextGameRound > curGameRound proceed
        nextGameRound = nextTurnBTN2.GetComponent<NextTurnBTN2>().gameRound;
        if (nextGameRound > curGameRound)
        {
            curGameRound++;
            NextTurn();
        }

        if (currentPath != null)
        {
            int curNode = 0;

            while (curNode < currentPath.Count - 1)
            {
                Vector3 start = map.TileCoordToWorldCoord(currentPath[curNode].x, currentPath[curNode].z) + new Vector3(0, 0.75f, 0);
                Vector3 end = map.TileCoordToWorldCoord(currentPath[curNode + 1].x, currentPath[curNode + 1].z) + new Vector3(0, 0.75f, 0);

                Debug.DrawLine(start, end, Color.green);

                curNode++;
            }
        }
        // Smoothly animate towards the correct map tile.
        transform.position = Vector3.Lerp(transform.position, map.TileCoordToWorldCoord(tileX, tileZ), 5f * Time.deltaTime);
    }

    #region -------------------------- ||| Fleet Collision ||| ----------------------------------
    //Fleet
    private void OnTriggerEnter(Collider col)
    {
        string curFleetName = gameObject.name;
        string colFleetName = col.gameObject.name;

        if (curFleetName.StartsWith("Fleet") && colFleetName.StartsWith("Fleet") && !postAction)
        {
            // Get Number of this fleet
            //string curFleet;
            int curFleetNum = fleetNumber;
            int colFleetNum = col.gameObject.GetComponent<Fleet>().fleetNumber;
            
            // Friendly Fleets or enemy Fleets
            if ((curFleetName.Contains("_P1") && colFleetName.Contains("_P1")) || (curFleetName.Contains("_P2") && colFleetName.Contains("_P2")))
            {
                // Friendly Fleets merge
                // Since I only want to destroy one fleet, this will only take action for the fleet with the lower number in the unity Hierachy (maybe change it to a private in inside the fleet script?)
                if (curFleetNum < colFleetNum)
                {                    
                    ShipChildrenList(out shipHolder, out shipChildren);

                    // while loop through the shipChildren Array and move everyone into the new fleet
                    int x = 0;
                    while (shipHolder.transform.childCount > 0)
                    {
                        shipChildren[x].transform.parent = col.transform.GetChild(1);
                        shipChildren[x].transform.position = col.transform.GetChild(1).transform.position;
                        x++;
                    }

                    // All ships have been moved into the new fleet, destroy the old one
                    Destroy(gameObject);
                }
            }
            else // Enemy Fleets, engage in combat
            {
                slu.SaveGame(slu.quickSaveName);
                // SceneManager.LoadScene(1);

                FleetCombat(curFleetName);
            }
        }
    }

    private void FleetCombat(string curFleetName)
    {
        string victorP = "P1"; // Placeholder
                               //print("COMBAT!");
        currentPath = null;

        if (curFleetName.Contains(victorP))
        {

        }
        else
        {
            //Destroy(gameObject);
            postAction = true;

            switch (victorP)
            {
                case "P1":
                    tileX += 1;
                    break;
                case "P2":
                    tileX -= 1;
                    break;
                default:
                    tileZ += 1;
                    break;
            }

            ShipChildrenList(out shipHolder, out shipChildren);

            int desFighter = 2;
            int desBomber = 1;
            int desCorvette = 1;

            for (int i = shipChildren.Count - 1; i >= 0; i--)
            {
                print(i);
                if (shipChildren[i].transform.name.Contains("Fighter") && desFighter > 0)
                {
                    desFighter--;
                    Destroy(shipChildren[i]);
                    shipChildren.RemoveAt(i);
                    continue;
                }

                if (shipChildren[i].transform.name.Contains("Bomber") && desBomber > 0)
                {
                    desBomber--;
                    Destroy(shipChildren[i]);
                    shipChildren.RemoveAt(i);
                    continue;
                }

                if (shipChildren[i].transform.name.Contains("Corvette") && desCorvette > 0)
                {
                    desCorvette--;
                    Destroy(shipChildren[i]);
                    shipChildren.RemoveAt(i);
                    continue;
                }
            }
            if (shipChildren.Count < 1)
            {
                Destroy(gameObject);
            }
        }
    }

    private void ShipChildrenList(out GameObject shipHolder, out List<GameObject> shipChildren)
    {
        shipHolder = gameObject.transform.GetChild(1).gameObject;
        GameObject shipChild;
        shipChildren = new List<GameObject>();

        // loop trhough Children and put them into an array (direclty moving them into another parent will change the childCount and fuck up
        for (int i = 0; i < shipHolder.transform.childCount; i++)
        {
            shipChild = shipHolder.transform.GetChild(i).gameObject;
            shipChildren.Add(shipChild);
        }
    }

    #endregion -------------------------- ||| Fleet Collision ||| ----------------------------------
    
    public void MoveOnTurn()
    {
        postAction = false;
            if (currentPath == null)
            {
                return;
            }

        if (remainingMovement <= 0)
        {
            return;
        }
                

            transform.position = map.TileCoordToWorldCoord(tileX, tileZ); // Update Unity World pos

            // Cost from current Tile to next Tile
            remainingMovement -= map.EnterTileCost(currentPath[0].x, currentPath[0].z, currentPath[1].x, currentPath[1].z);

            // Grab new current/first node
            tileX = currentPath[1].x;
            tileZ = currentPath[1].z;            

            // Remove old current Node from path
            currentPath.RemoveAt(0);

            if (currentPath.Count == 1)
            {
                // Next Tile ist the last one left in Path == Destination
                currentPath = null;
            }        
    }

    // The "Next Turn" button calls this.
    public void NextTurn()
    {
        // Reset our available movement points.
        remainingMovement = movementSpeed();
        // Make sure to wrap-up any outstanding movement left over.
        while (currentPath != null && remainingMovement > 0)
        {
            MoveOnTurn();
        }
                
    }

    private void reassignGameObjs()
    {
        nextTurnObj = GameObject.Find("NextTurnBTN2");
        nextTurnBTN2 = nextTurnObj.GetComponent<NextTurnBTN2>();

        mouseObj = GameObject.Find("MouseManager");
        mapObj = GameObject.Find("Map");
        mouseManager = mouseObj.GetComponent<MouseManager>();
        map = mapObj.GetComponent<TileMap>();
    }
}
