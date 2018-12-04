using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class Fleet : PlayerPawn
{

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
    bool fleetPostAction;
    [DontSaveMember] public List<Node> currentPath = null;
    [DontSaveMember] float remainingMovement;
    [DontSaveMember] ShipChildrenCollect shipChildrenCollect;
    [DontSaveMember] CombatCalc combatCalc;
    [DontSaveMember] GameObject shipHolder;
    public bool justSeparated = false;
    private int gameRoundSeparated;
    // [DontSaveMember] List<GameObject> shipChildren;

    public int fleetNumber;
    private GameObject sluObj;
    private SaveLoadUtility slu;

    public virtual int movementSpeed()
    {
        return 2;
    }

    private void Awake()
    {
        PlayerPawnStartInit();
        reassignGameObjs();
    }

    private void Start()
    {
        gameRoundSeparated = curGameRound;
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

        curGameRound = nextTurnBTN2.GetComponent<NextTurnBTN2>().gameRound;
        tileX = (int)transform.position.x;
        tileZ = (int)transform.position.z;
    }

    private void Update()
    {
        ActivateColliderAfterSeparation();

        fleetPostAction = fleetCombatInfo.fleetPostAction;
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

    private void ActivateColliderAfterSeparation()
    {
        print(curGameRound);
        print(gameRoundSeparated + 2);
        if (justSeparated && curGameRound == gameRoundSeparated + 2)
        {
            print("after");
            Collider myCol = gameObject.GetComponent<Collider>();
            myCol.enabled = true;
        }
    }

    //Fleet Collision, register Box Fleet Colliders, then decide whether it's frendlies or foes
    private void OnTriggerEnter(Collider col)
    {
        print("Fleet Trigger Enter");
        // Current and Collider Fleet Names and Numbers
        StartCoroutine("WaitTime", col); // Coroutine is used to delay the next function
    }

    private void OnTriggerExit(Collider col)
    {
        print("Fleet Trigger Exit");
        // Current and Collider Fleet Names and Numbers
        CallCollision(col);
    }

    IEnumerator WaitTime(Collider col) // this Coroutine waits a second, before continuing with the combat, the the fleets collide (important for after the fight)
    {
        print("wait");
        yield return new WaitForSeconds(1); //Count is the amount of time in seconds that you want to wait.
        CallCollision(col);
        print("CallCol");
        yield return null;
    }

    private void CallCollision(Collider col) // Calling the script handling collisions and combat
    {
        if (col != null)
        {
            string curFleetName = gameObject.name;
            int curFleetNum = fleetNumber;
            string colFleetName = col.gameObject.name;

            fleetCollision.FleetCollider(col, gameObject, curFleetName, curFleetNum, colFleetName, fleetPostAction, currentPath, tileX, tileZ);
        }
    }

    public void MoveOnTurn()
    {
        fleetCombatInfo.fleetPostAction = false;
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

        shipChildrenCollect = playerController.GetComponent<ShipChildrenCollect>();
        combatCalc = playerController.GetComponent<CombatCalc>();
    }
}
