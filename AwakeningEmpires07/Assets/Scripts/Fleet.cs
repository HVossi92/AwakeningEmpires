using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class Fleet : MonoBehaviour {

    public int tileX;
    public int tileZ;
    private GameObject mapObj;
    private GameObject nextTurnObj;
    private NextTurnBTN2 nextTurnBTN2;
    public TileMap map;
    private int curGameRound;
    private int nextGameRound;
    public int fleetOfPlayer;
    public GameObject mouseObj;
    private MouseManager mouseManager;
    private bool postAction = false;

    public List<Node> currentPath = null;   
    float remainingMovement;

    GameObject shipHolder;
    GameObject[] shipChildren;

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

    #region -------------------------- ||| Fleet Collision ||| ----------------------------------
    //Fleet
    private void OnTriggerEnter(Collider col)
    {
        string curFleetName = gameObject.name;
        string colFleetName = col.gameObject.name;

        if (curFleetName.StartsWith("Fleet") && colFleetName.StartsWith("Fleet") && !postAction)
        {
            // Get Number of this fleet
            string curFleet;
            int curFleetNum;
            if (curFleetName.Length < 18)
            {
                curFleet = gameObject.name.Substring(gameObject.name.Length - 8);
                curFleet = curFleet.Remove(1);
                curFleetNum = Int32.Parse(curFleet); 
            }
            else
            {
                curFleet = gameObject.name.Substring(gameObject.name.Length - 9);
                curFleet = curFleet.Remove(2);
                curFleetNum = Int32.Parse(curFleet);
            }

            // Get number of colliding fleet
            string colFleet;
            int colFleetNum;
            if (colFleetName.Length < 18)
            {
                colFleet = col.gameObject.name.Substring(col.gameObject.name.Length - 8);
                colFleet = colFleet.Remove(1);
                colFleetNum = Int32.Parse(colFleet);
            }
            else
            {
                colFleet = col.gameObject.name.Substring(col.gameObject.name.Length - 9);
                colFleet = colFleet.Remove(2);
                colFleetNum = Int32.Parse(colFleet);
            }
            // Friendly Fleets or enemy Fleets
            if ((curFleetName.Contains("_P1_") && colFleetName.Contains("_P1_")) || (curFleetName.Contains("_P2_") && colFleetName.Contains("_P2_")))
            {
                // Friendly Fleets merge
                // Since I only want to destroy one fleet, this will only take action for the fleet with the lower number in the unity Hierachy (maybe change it to a private in inside the fleet script?)
                if (curFleetNum < colFleetNum)
                {                    
                    ShipChildrenArray(out shipHolder, out shipChildren);

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
                SceneManager.LoadScene(1, LoadSceneMode.Additive);

                string victorP = "P1"; // Placeholder
                print("COMBAT!");
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

                    ShipChildrenArray(out shipHolder, out shipChildren);

                    // while loop through the shipChildren Array and move everyone into the new fleet
                    int desFighter = 2;
                    int desBomber = 1;
                    int desCorvette = 1;
                    print(shipHolder.transform.childCount);
                    for (int i = 0; i <= shipHolder.transform.childCount; i++)
                    {
                        if(shipChildren[i].transform.name.Contains("Fighter") && desFighter > 0)
                        {
                            desFighter--;
                            Destroy(shipChildren[i]);
                        }else if (shipChildren[i].transform.name.Contains("Bomber") && desBomber > 0)
                        {
                            desBomber--;
                            Destroy(shipChildren[i]);
                        }
                        else if (shipChildren[i].transform.name.Contains("Corvette") && desCorvette > 0)
                        {
                            desCorvette--;
                            Destroy(shipChildren[i]);
                        }
                        print(shipHolder.transform.childCount);
                    }
                    int x = 0;
                    while (shipHolder.transform.childCount > 0)
                    {
                        shipChildren[x].transform.parent = col.transform.GetChild(1);
                        shipChildren[x].transform.position = col.transform.GetChild(1).transform.position;
                        x++;
                    }

                }
            }
        }
    }

    private void ShipChildrenArray(out GameObject shipHolder, out GameObject[] shipChildren)
    {
        shipHolder = gameObject.transform.GetChild(1).gameObject;
        GameObject shipChild;
        shipChildren = new GameObject[50];

        // loop trhough Children and put them into an array (direclty moving them into another parent will change the childCount and fuck up
        for (int i = 0; i < shipHolder.transform.childCount; i++)
        {
            shipChild = shipHolder.transform.GetChild(i).gameObject;
            shipChildren[i] = shipChild;
        }
    }

    #endregion -------------------------- ||| Fleet Collision ||| ----------------------------------
    private void Awake()
    {
        mouseObj = GameObject.Find("MouseManager");
        mapObj = GameObject.Find("Map");
        nextTurnObj = GameObject.Find("NextTurnBTN2");
        mouseManager = mouseObj.GetComponent<MouseManager>();
        map = mapObj.GetComponent<TileMap>();
        nextTurnBTN2 = nextTurnObj.GetComponent<NextTurnBTN2>();
    }

    private void Start()
    {
        // Current Game Round = NextTurnBTN Gameround (1)
        curGameRound = nextTurnBTN2.GetComponent<NextTurnBTN2>().gameRound;
        tileX = (int) transform.position.x;
        tileZ = (int) transform.position.z;
    }

    private void Update()
    {
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

            while(curNode < currentPath.Count - 1)
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
}
