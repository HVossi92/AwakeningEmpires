using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FleetCollision : MonoBehaviour
{
    [DontSaveMember] private GameObject fleetCombatInfoObj;
    [DontSaveMember] private FleetCombatInfo fleetCombatInfo;
    [DontSaveMember] ShipChildrenCollect shipChildrenCollect;
    [DontSaveMember] CombatCalc combatCalc;
    [DontSaveMember] GameObject shipHolder;
    private GameObject playerControllerObj;
    private PlayerController playerController;
    private GameObject sluObj;
    private SaveLoadUtility slu;
    
    private void Start()
    {
        reassignGameObjs();
    }

    public void FleetCollider(Collider col, GameObject curFleet, string curFleetName, int curFleetNum, string colFleetName, bool fleetPostAction, List<Node> currentPath, int tileX, int tileZ)
    {
        if (curFleetName.StartsWith("Fleet") && colFleetName.StartsWith("Fleet") && !fleetPostAction)
        {
            int colFleetNum = col.gameObject.GetComponent<Fleet>().fleetNumber;
            // Friendly Fleets or enemy Fleets
            if ((curFleetName.Contains("_P1") && colFleetName.Contains("_P1")) || (curFleetName.Contains("_P2") && colFleetName.Contains("_P2")))
            {
                FriendlyFleetMerge(curFleet, col, curFleetNum, colFleetNum);
            }
            else // Enemy Fleets, engage in combat
            {
                StartCombat();
            }
        }
        else if (curFleetName.StartsWith("Fleet") && colFleetName.StartsWith("Fleet") && fleetPostAction)
        {
            combatCalc.FleetCombat(curFleet, curFleetName, col, colFleetName, currentPath, tileX, tileZ);
        }
        else if (curFleetName.StartsWith("Fleet") && colFleetName.StartsWith("Building") && !fleetPostAction)
        {
            if ((curFleetName.Contains("_P1") && colFleetName.Contains("P1")) || (curFleetName.Contains("_P2") && colFleetName.Contains("P2")))
            {
                // Think of a functionality?
                print("Friendlies in Orbit");
            }
            else // Enemy Fleets, engage in combat
            {
                print("Start Building Combat");
                StartCombat();
            }
        }
        else if (curFleetName.StartsWith("Fleet") && colFleetName.StartsWith("Building") && fleetPostAction)
        {
            combatCalc.FleetCombat(gameObject, curFleetName, col, colFleetName, currentPath, tileX, tileZ);
        }
    }

    private void StartCombat()
    {
        fleetCombatInfo.fleetPostAction = true;
        fleetCombatInfo.fightersP1 = 2;
        slu.SaveGame(slu.quickSaveName); // <<<<<<------------------------------------------------------------------------------------------------------
        SceneManager.LoadScene(1);
    }

    private void FriendlyFleetMerge(GameObject curFleet, Collider col, int curFleetNum, int colFleetNum = 0)
    {
        // Friendly Fleets merge
        // Since I only want to destroy one fleet, this will only take action for the fleet with the lower number in the unity Hierachy (maybe change it to a private in inside the fleet script?)
        if (curFleetNum < colFleetNum)
        {
            GameObject shipHolder = curFleet.transform.GetChild(1).gameObject;
            List<GameObject> shipChildren = shipChildrenCollect.ShipChildrenList(shipHolder);
            // while loop through the shipChildren Array and move everyone into the new fleet
            int x = 0;
            while (shipHolder.transform.childCount > 0)
            {
                shipChildren[x].transform.parent = col.transform.GetChild(1);
                shipChildren[x].transform.position = col.transform.GetChild(1).transform.position;
                x++;
            }

            // All ships have been moved into the new fleet, destroy the old one
            Destroy(curFleet);
        }
    }

    private void reassignGameObjs()
    {
        playerControllerObj = GameObject.Find("PlayerController");
        playerController = playerControllerObj.GetComponent<PlayerController>();

        fleetCombatInfoObj = GameObject.Find("FleetCombatInfo");
        fleetCombatInfo = fleetCombatInfoObj.GetComponent<FleetCombatInfo>();

        shipChildrenCollect = playerController.GetComponent<ShipChildrenCollect>();
        combatCalc = playerController.GetComponent<CombatCalc>();

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
    }
}
