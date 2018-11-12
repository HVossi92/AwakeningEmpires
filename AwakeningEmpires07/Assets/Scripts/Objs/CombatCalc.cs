using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatCalc : MonoBehaviour {

    [DontSaveMember] GameObject playerController;
    [DontSaveMember] ShipChildrenCollect shipChildrenCollect;

    private void Awake()
    {
        reassignGameObjs();
    }

    // "Touching" Fleets are enemies, Combat results will be executed. Needs to be called after Loading back map
    public void FleetCombat(GameObject curFleet, string curFleetName, Collider collider, string colliderName,List<Node> currentPath, int tileX, int tileZ)
    {
        Fleet myFleet = curFleet.GetComponent<Fleet>();

        string victorP = "P2"; // Placeholder
        myFleet.currentPath = null;

        if (!colliderName.Contains("Building"))
        {
            FleetVsFleet(curFleet, curFleetName, collider, colliderName, myFleet, victorP);
        }
        else
        {
            FleetVsBuilding(curFleet, curFleetName, collider, colliderName, myFleet, victorP);
        }
    }

    private void FleetVsFleet(GameObject curFleet, string curFleetName, Collider collider, string colliderName, Fleet myFleet, string victorP)
    {
        if (curFleetName.Contains(victorP))
        {
            return;
        }
        else if (curFleetName.Contains("Fleet")) // Check that the loser is a Fleet
        {
            MoveLosingFleet(myFleet, curFleet, victorP);

            GameObject shipHolder = curFleet.transform.GetChild(1).gameObject;
            List<GameObject> shipChildren = shipChildrenCollect.ShipChildrenList(shipHolder);
            CalculateCombatLosses(shipChildren);

            // If the losing Fleet is left with no ships, the Fleet Game Piece gets destroyed
            if (shipChildren.Count < 1)
            {
                Destroy(curFleet);
            }
        }
        else if (colliderName.Contains("Building")) // Losing Side is a Building
        {
            Destroy(collider);
        }
    }

    private void FleetVsBuilding(GameObject curFleet, string curFleetName, Collider collider, string colliderName, Fleet myFleet, string victorP)
    {
        if(curFleetName.Contains("Fleet") && !curFleetName.Contains(victorP)) // Check that the loser is a Fleet
        {
            MoveLosingFleet(myFleet, curFleet, victorP);

            GameObject shipHolder = curFleet.transform.GetChild(1).gameObject;
            List<GameObject> shipChildren = shipChildrenCollect.ShipChildrenList(shipHolder);
            CalculateCombatLosses(shipChildren);

            // If the losing Fleet is left with no ships, the Fleet Game Piece gets destroyed
            if (shipChildren.Count < 1)
            {
                Destroy(curFleet);
            }
        }
        else if (colliderName.Contains("Building")) // Losing Side is a Building
        {
            Destroy(collider.gameObject);
        }
    }

    // Delete single ships inside the shipHolder after Combat
    private static void CalculateCombatLosses(List<GameObject> shipChildren)
    {
        int desFighter = 0;
        int desBomber = 1;
        int desCorvette = 0;

        for (int i = shipChildren.Count - 1; i >= 0; i--)
        {
            if (shipChildren[i].transform.name.Contains("Fighter") && desFighter > 0)
            {
                desFighter--;
                Destroy(shipChildren[i]);
                shipChildren.RemoveAt(i);
                print("Fighter Des");
                continue;
            }

            if (shipChildren[i].transform.name.Contains("Bomber") && desBomber > 0)
            {
                desBomber--;
                Destroy(shipChildren[i]);
                shipChildren.RemoveAt(i);
                print("Bomber Des");
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
    }

    // Move the losing Fleet back a Tile on the gameboard
    private static void MoveLosingFleet(Fleet myFleet, GameObject curFleet, string victorP)
    {
        print("Loser Retreats");
        switch (victorP)
        {            
            case "P1":
                myFleet.tileX += 1;
                break;
            case "P2":
                myFleet.tileX -= 1;
                break;
            default:
                myFleet.tileZ += 1;
                break;
        }
    }

    private void reassignGameObjs()
    {
        playerController = GameObject.Find("PlayerController");
        shipChildrenCollect = playerController.GetComponent<ShipChildrenCollect>();
    }
}
