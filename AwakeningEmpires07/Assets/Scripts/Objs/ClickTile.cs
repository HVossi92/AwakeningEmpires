using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Responsible for generating Fleet path and depending on its type, construction
public class ClickTile : MonoBehaviour {

    public int tileX;
    public int tileZ;
    private GameObject mapObj;
    public TileMap map;
    public GameObject nextTurnBtn2;
    private int activePlayer;
    private GameObject fleet;
    private GameObject fighter;
    private bool buildingOnTileFlag = false;
    private GameObject buildControllerObj;
    private BuildController buildController;
    private GameObject playerControllerObj;
    private PlayerController playerController;
    private int currentMinerals;

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            map.GenerateFleetPathTo(tileX, tileZ);
        }
    }

    private void Start()
    {
        buildControllerObj = GameObject.Find("BuildController");
        buildController = buildControllerObj.GetComponent<BuildController>();

        playerControllerObj = GameObject.Find("PlayerController");
        playerController = playerControllerObj.GetComponent<PlayerController>();        
    }

    private void Update()
    {
        if(map == null)
        {
            reassignGameObjs();
        }
        currentMinerals = playerController.getMineralBalance();
    }

    private void GetPlayer()
    {
        GameObject thePlayer = GameObject.Find("NextTurnBTN2");
        NextTurnBTN2 nextTurnBtn2 = thePlayer.GetComponent<NextTurnBTN2>();
        activePlayer = nextTurnBtn2.activePlayer;
    }

    private void OnMouseUpAsButton()
    {
        GetPlayer();
        if (!buildingOnTileFlag)
        {            
            if (gameObject.name == "TileShipYardPlayer1" && activePlayer == 1)
            {
                buildController.SelectTile(this);
            }
            else if (gameObject.name == "TileShipYardPlayer2" && activePlayer == 2)
            {
                buildController.SelectTile(this);
            }
            else if (gameObject.name == "TileConstruction_P1" && activePlayer == 1)
            {
                buildController.SelectTile(this);
            }
            else if (gameObject.name == "TileConstruction_P2" && activePlayer == 2)
            {
                buildController.SelectTile(this);
            }
        }
        else
        {
            print("Tile already occupied");
        }
    }

    // Build Fleet and Fighter
    public void BuildFighterOnTile(int rotationPlayer = 0)
    {
        if(currentMinerals >= buildController.FighterCost)
        {
            GameObject fighterBuild = BuildController.instance.GetFighterBuild();
            BuildSpaceShip(rotationPlayer, fighterBuild);
        }        
    }

    // Bomber
    public void BuildBomberOnTile(int rotationPlayer = 0)
    {
        if (currentMinerals >= buildController.BomberCost)
        {
            GameObject bomberBuild = BuildController.instance.GetBomberBuild();
            BuildSpaceShip(rotationPlayer, bomberBuild);
        }
    }

    // Corvette
    public void BuildCorvetteOnTile(int rotationPlayer = 0)
    {
        if (currentMinerals >= buildController.CorvetteCost)
        {
            GameObject corvetteBuild = BuildController.instance.GetCorvetteBuild();
            BuildSpaceShip(rotationPlayer, corvetteBuild);
        }
    }


    private void BuildSpaceShip(int rotationPlayer, GameObject spaceShip)
    {
        GameObject fleetBuild = BuildController.instance.GetFleetBuild();
        fleet = (GameObject)Instantiate(fleetBuild, transform.position, transform.rotation);
        fleet.name = fleet.name.Remove(fleet.name.Length - 7); // Remove "Clone"

        GameObject shipHolder = Instantiate(Resources.Load("Spaceships/Ships")) as GameObject;
        shipHolder.transform.parent = fleet.transform;
        Vector3 shipHolderPos = new Vector3(fleet.transform.position.x, fleet.transform.position.y + 1f, fleet.transform.position.z);
        shipHolder.transform.position = shipHolderPos;
        
        fighter = (GameObject)Instantiate(spaceShip, shipHolderPos, transform.rotation);
        fighter.transform.parent = shipHolder.transform;

        shipHolder.transform.rotation = Quaternion.Euler(0, rotationPlayer, 0);
    }

    public void BuildFactoryOnTile()
    {
        if (currentMinerals >= buildController.FactoryCost)
        {
            GameObject factoryBuild = BuildController.instance.GetFactoryBuild();
            fleet = (GameObject)Instantiate(factoryBuild, transform.position, transform.rotation);
            buildingOnTileFlag = true;
        }
    }

    public void BuildTurretOnTile()
    {
        if (currentMinerals >= buildController.TurretCost)
        {
            GameObject turretBuild = BuildController.instance.GetTurretBuild();
            fleet = (GameObject)Instantiate(turretBuild, transform.position, transform.rotation);
            buildingOnTileFlag = true;
        }
    }

    public void BuildSolarOnTile()
    {
        if (currentMinerals >= buildController.SolarCost)
        {
            GameObject solarBuild = BuildController.instance.GetSolarBuild();
            Vector3 solarPosition = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
            Quaternion solarRot = Quaternion.Euler(-90f, 0, 0);
            fleet = (GameObject)Instantiate(solarBuild, solarPosition, solarRot);
            buildingOnTileFlag = true;
        }
    }

    private void reassignGameObjs()
    {
        mapObj = GameObject.Find("Map");
        map = mapObj.GetComponent<TileMap>();
    }
}
