using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Responsible for generating Fleet path and depending on its type, construction
public class ClickTile : MonoBehaviour {

    public int tileX;
    public int tileZ;
    public TileMap map;
    public GameObject nextTurnBtn2;
    private int activePlayer;
    private GameObject fleet;
    private GameObject fighter;
    private bool buildingOnTileFlag = false;
    private GameObject buildControllerObj;
    private BuildController buildController;

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
    }

    private void Update()
    {
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
            if (gameObject.name == "TileShipYardPlayer1(Clone)" && activePlayer == 1)
            {
                buildController.SelectTile(this);
            }
            else if (gameObject.name == "TileShipYardPlayer2(Clone)" && activePlayer == 2)
            {
                buildController.SelectTile(this);
            }
            else if (gameObject.name == "TileConstructionPlayer1(Clone)" && activePlayer == 1)
            {
                buildController.SelectTile(this);
            }
            else if (gameObject.name == "TileConstructionPlayer2(Clone)" && activePlayer == 2)
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
        GameObject fighterBuild = BuildController.instance.GetFighterBuild();
        BuildSpaceShip(rotationPlayer, fighterBuild);
    }

    // Bomber
    public void BuildBomberOnTile(int rotationPlayer = 0)
    {
        GameObject bomberBuild = BuildController.instance.GetBomberBuild();
        BuildSpaceShip(rotationPlayer, bomberBuild);
    }

    // Corvette
    public void BuildCorvetteOnTile(int rotationPlayer = 0)
    {
        GameObject corvetteBuild = BuildController.instance.GetCorvetteBuild();
        BuildSpaceShip(rotationPlayer, corvetteBuild);
    }


    private void BuildSpaceShip(int rotationPlayer, GameObject spaceShip)
    {
        GameObject fleetBuild = BuildController.instance.GetFleetBuild();
        fleet = (GameObject)Instantiate(fleetBuild, transform.position, transform.rotation);

        GameObject shipHolder = new GameObject();
        shipHolder.name = "Ships";
        shipHolder.transform.parent = fleet.transform;
        Vector3 shipHolderPos = new Vector3(fleet.transform.position.x, fleet.transform.position.y + 1f, fleet.transform.position.z);
        shipHolder.transform.position = shipHolderPos;
        
        fighter = (GameObject)Instantiate(spaceShip, shipHolderPos, transform.rotation);
        fighter.transform.parent = shipHolder.transform;

        shipHolder.transform.rotation = Quaternion.Euler(0, rotationPlayer, 0);
    }

    public void BuildFactoryOnTile()
    {
        GameObject factoryBuild = BuildController.instance.GetFactoryBuild();
        fleet = (GameObject)Instantiate(factoryBuild, transform.position, transform.rotation);
        buildingOnTileFlag = true;
    }

    public void BuildTurretOnTile()
    {
        GameObject turretBuild = BuildController.instance.GetTurretBuild();
        fleet = (GameObject)Instantiate(turretBuild, transform.position, transform.rotation);
        buildingOnTileFlag = true;
    }
}
