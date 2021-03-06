﻿using System.Collections;
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
    public GameObject solarPrefabP1;
    public GameObject solarPrefabP2;
    public GameObject fighterPrefab;
    public GameObject bomberPrefab;
    public GameObject corvettePrefab;

    private GameObject fleetBuildP1;
    private GameObject fleetBuildP2;
    private GameObject factoryBuildP1;
    private GameObject factoryBuildP2;
    private GameObject turretBuildP1;
    private GameObject turretBuildP2;
    private GameObject solarBuildP1;
    private GameObject solarBuildP2;
    private GameObject fighterBuild;
    private GameObject bomberBuild;
    private GameObject corvetteBuild;

    public GameObject playerControllerObj;
    private PlayerController playerController;

    private int fleetCount = 1;
    private string fleetNameP1 = "Fleet_P1";
    private string fleetNameP2 = "Fleet_P2";

    private int _factoryCost = 25;
    private int _turretCost = 50;
    private int _solarCost = 25;
    private int _fighterCost = 25;
    private int _bomberCost = 50;
    private int _corvetteCost = 75;

    private ClickTile selectedTile;

    private int currentMinerals;

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
        solarBuildP1 = solarPrefabP1;
        solarBuildP2 = solarPrefabP2;
        fighterBuild = fighterPrefab;
        bomberBuild = bomberPrefab;
        corvetteBuild = corvettePrefab;

        playerController = playerControllerObj.GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (buildingTileUI == null || shipyardTileUI == null)
        {
            reassignGameObjs();
        }

        GetPlayer();
        currentMinerals = playerController.getMineralBalance();
    }

    public void reassignGameObjs()
    {
        GameObject buildingTileUIObj = GameObject.Find("BuildingTileUI");
        buildingTileUI = buildingTileUIObj.GetComponent<BuildingTileUI>();

        GameObject shipyardTileUIObj = GameObject.Find("ShipyardTileUI");
        shipyardTileUI = shipyardTileUIObj.GetComponent<ShipyardTileUI>();
    }

    // Setters for Building Costs
    public int FactoryCost
    {
        get { return _factoryCost; }
    }

    public int TurretCost
    {
        get { return _turretCost; }
    }

    public int SolarCost
    {
        get { return _solarCost; }
    }

    public int FighterCost
    {
        get { return _fighterCost; }
    }

    public int BomberCost
    {
        get { return _bomberCost; }
    }

    public int CorvetteCost
    {
        get { return _corvetteCost; }
    }

    // Providing the builds
    public GameObject GetFleetBuild()
    {
        if(activePlayer == 1)
        {
            fleetBuildP1.transform.name = fleetNameP1;
            Fleet fleet = fleetBuildP1.GetComponent<Fleet>();
            fleet.fleetNumber = fleetCount;
            fleetCount++;
            return fleetBuildP1;
        }else //if(activePlayer == 2)
        {
            fleetBuildP2.transform.name = fleetNameP2;
            Fleet fleet = fleetBuildP2.GetComponent<Fleet>();
            fleet.fleetNumber = fleetCount;
            fleetCount++;
            return fleetBuildP2;
        }           
    }

    public GameObject InstantiateFleet(Transform fleetPos)
    {
        GameObject fleetBuild = GetFleetBuild();
        GameObject fleet = (GameObject)Instantiate(fleetBuild, fleetPos);
        fleet.name = fleet.name.Remove(fleet.name.Length - 7); // Remove "Clone"

        GameObject shipHolder = Instantiate(Resources.Load("Spaceships/Ships")) as GameObject;
        shipHolder.transform.parent = fleet.transform;
        Vector3 shipHolderPos = new Vector3(fleet.transform.position.x, fleet.transform.position.y + 1f, fleet.transform.position.z);
        shipHolder.transform.position = shipHolderPos;

        return fleet;
    }

    public GameObject GetFighterBuild()
    {
     playerController.mineralExpense(_fighterCost);
     return fighterBuild;
    }

    public GameObject GetBomberBuild()
    {
        playerController.mineralExpense(_bomberCost);
        return bomberBuild;
    }

    public GameObject GetCorvetteBuild()
    {
        playerController.mineralExpense(_corvetteCost);
        return corvetteBuild;
    }

    public GameObject GetFactoryBuild()
    {
        if (activePlayer == 1)
        {
            playerController.mineralExpense(_factoryCost);
            return factoryBuildP1;
        }
        else //if(activePlayer == 2)
        {
            playerController.mineralExpense(_factoryCost);
            return factoryBuildP2;
        }
    }

    public GameObject GetTurretBuild()
    {
        if (activePlayer == 1)
        {
            playerController.mineralExpense(_turretCost);
            return turretBuildP1;
        }
        else //if(activePlayer == 2)
        {
            playerController.mineralExpense(_turretCost);
            return turretBuildP2;
        }
    }

    public GameObject GetSolarBuild()
    {
        if (activePlayer == 1)
        {
            playerController.mineralExpense(_solarCost);
            return solarBuildP1;
        }
        else //if(activePlayer == 2)
        {
            playerController.mineralExpense(_solarCost);
            return solarBuildP2;
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
