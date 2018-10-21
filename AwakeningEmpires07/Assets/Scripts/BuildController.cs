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

    public GameObject playerControllerObj;
    private PlayerController playerController;

    private int fleetCountP1 = 1;
    private int fleetCountP2 = 1;
    private string fleetNameP1 = "Fleet_P1_";
    private string fleetNameP2 = "Fleet_P2_";

    private int _factoryCost = 25;
    private int _turretCost = 50;
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
        fighterBuild = fighterPrefab;
        bomberBuild = bomberPrefab;
        corvetteBuild = corvettePrefab;

        playerController = playerControllerObj.GetComponent<PlayerController>();
    }

    private void Update()
    {
        GetPlayer();
        currentMinerals = playerController.getMineralBalance();
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
