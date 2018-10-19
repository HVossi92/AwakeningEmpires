using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildController : MonoBehaviour {

    private int activePlayer;
    public static BuildController instance;
    public BuildingTileUI buildingTileUI;
    public ShipyardTileUI shipyardTileUI;

    public GameObject fleetPrefabP1;
    public GameObject fleetPrefabP2;
    public GameObject factoryPrefabP1;
    public GameObject factoryPrefabP2;
    public GameObject turretPrefabP1;
    public GameObject turretPrefabP2;
    public GameObject fighterPrefab;
    public GameObject bomberPrefab;
    public GameObject corvettePrefab;

    private GameObject fleetBuildP1;
    private GameObject fleetBuildP2;
    private GameObject factoryBuildP1;
    private GameObject factoryBuildP2;
    private GameObject turretBuildP1;
    private GameObject turretBuildP2;
    private GameObject fighterBuild;
    private GameObject bomberBuild;
    private GameObject corvetteBuild;

    private int fleetCountP1 = 1;
    private int fleetCountP2 = 1;
    private string fleetNameP1 = "Fleet_P1_";
    private string fleetNameP2 = "Fleet_P2_";

    private int factoryCost = 25;
    private int turretCost = 50;
    private int fighterCost = 25;
    private int bomberCost = 50;
    private int corvetteCost = 75;

    private ClickTile selectedTile;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Multiple BuildControllers");
            return;
        }
        instance = this;
    }
    
    private void Start()
    {
        fleetBuildP1 = fleetPrefabP1;
        fleetBuildP2 = fleetPrefabP2;
        factoryBuildP1 = factoryPrefabP1;
        factoryBuildP2 = factoryPrefabP2;
        turretBuildP1 = turretPrefabP1;
        turretBuildP2 = turretPrefabP2;
        fighterBuild = fighterPrefab;
        bomberBuild = bomberPrefab;
        corvetteBuild = corvettePrefab;
    }

    private void Update()
    {
        GetPlayer();
    }

    // Providing the builds
    public GameObject GetFleetBuild()
    {
        if(activePlayer == 1)
        {
            fleetBuildP1.transform.name = fleetNameP1 + fleetCountP1.ToString();
            fleetCountP1++;
            return fleetBuildP1;
        }else //if(activePlayer == 2)
        {
            fleetBuildP2.transform.name = fleetNameP2 + fleetCountP2.ToString();
            fleetCountP2++;
            return fleetBuildP2;
        }           
    }

    public GameObject GetFighterBuild()
    {
        return fighterBuild;
    }

    public GameObject GetBomberBuild()
    {
        return bomberBuild;
    }

    public GameObject GetCorvetteBuild()
    {
        return corvetteBuild;
    }

    public GameObject GetFactoryBuild()
    {
        if (activePlayer == 1)
        {
            return factoryBuildP1;
        }
        else //if(activePlayer == 2)
        {
            return factoryBuildP2;
        }
    }

    public GameObject GetTurretBuild()
    {
        if (activePlayer == 1)
        {
            return turretBuildP1;
        }
        else //if(activePlayer == 2)
        {
            return turretBuildP2;
        }
    }

    public void SelectTile (ClickTile getTile)
    {
        selectedTile = getTile;
        buildingTileUI.SetTarget(selectedTile);
        shipyardTileUI.SetTarget(selectedTile);
    }

    private void GetPlayer()
    {
        GameObject thePlayer = GameObject.Find("NextTurnBTN2");
        NextTurnBTN2 nextTurnBtn2 = thePlayer.GetComponent<NextTurnBTN2>();
        activePlayer = nextTurnBtn2.activePlayer;
    }
}
