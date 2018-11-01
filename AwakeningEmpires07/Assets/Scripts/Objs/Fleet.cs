using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class Fleet : MonoBehaviour {

    public int tileX;
    public int tileZ;
    [DontSaveMember] private GameObject mapObj;
    [DontSaveMember] private GameObject nextTurnObj;
    [DontSaveMember] private NextTurnBTN2 nextTurnBTN2;
    [DontSaveMember] private GameObject fleetCombatInfoObj;
    [DontSaveMember] private FleetCombatInfo fleetCombatInfo;
    [DontSaveMember] public TileMap map;
    [DontSaveMember] private int curGameRound;
    [DontSaveMember] private int nextGameRound;
    [DontSaveMember] public int fleetOfPlayer;
    [DontSaveMember] public GameObject mouseObj;
    [DontSaveMember] private MouseManager mouseManager;
    private bool postAction = false;

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
        // Smoothly animate towards the correct map tile
        //transform.position = Vector3.Lerp(transform.position, map.TileCoordToWorldCoord(tileX, tileZ), 5 * Time.deltaTime);
        transform.position = Vector3.MoveTowards(transform.position, map.TileCoordToWorldCoord(tileX, tileZ), 5f * Time.deltaTime);
    }

    #region -------------------------- ||| Fleet Collision ||| ----------------------------------
    //Fleet Collision, register Box Fleet Colliders, then decide whether it's frendlies or foes
    private void OnTriggerEnter(Collider col)
    {
        // Current and Collider Fleet Names and Numbers
        string curFleetName = gameObject.name;        
        int curFleetNum = fleetNumber;
        string colFleetName = col.gameObject.name;        

        if (curFleetName.StartsWith("Fleet") && colFleetName.StartsWith("Fleet") && !postAction)
        {
            int colFleetNum = col.gameObject.GetComponent<Fleet>().fleetNumber;
            // Friendly Fleets or enemy Fleets
            if ((curFleetName.Contains("_P1") && colFleetName.Contains("_P1")) || (curFleetName.Contains("_P2") && colFleetName.Contains("_P2")))
            {
                FriendlyFleetMerge(col, curFleetNum, colFleetNum);
            }
            else // Enemy Fleets, engage in combat
            {
                postAction = true;                
                fleetCombatInfo.fightersP1 = 2;
                slu.SaveGame(slu.quickSaveName); // <<<<<<------------------------------------------------------------------------------------------------------
                SceneManager.LoadScene(1);
            }
        }else if(postAction)
        {
            FleetCombat(curFleetName);
        }
    }

    private void FriendlyFleetMerge(Collider col, int curFleetNum, int colFleetNum)
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

    // "Touching" Fleets are enemies, Combat results will be executed. Needs to be called after Loading back map
    private void FleetCombat(string curFleetName)
    {
        string victorP = "P1"; // Placeholder
                               //print("COMBAT!");
        currentPath = null;

        if (curFleetName.Contains(victorP))
        {
            return;
        }
        else
        {
            //Destroy(gameObject);            

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

            int desFighter = 0;
            int desBomber = 0;
            int desCorvette = 0;

            for (int i = shipChildren.Count - 1; i >= 0; i--)
            {
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

        // loop trhough Children and put them into a List (direclty moving them into another parent will change the childCount and fuck up
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

        fleetCombatInfoObj = GameObject.Find("FleetCombatInfo");
        fleetCombatInfo = fleetCombatInfoObj.GetComponent<FleetCombatInfo>();
    }
}
