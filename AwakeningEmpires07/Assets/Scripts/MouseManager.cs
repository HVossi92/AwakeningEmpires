﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour {

    public GameObject hoverObject;
    public GameObject selectedObject;
    private Camera _Camera;
    private Color prevClrHover;
    private Color prevClrSelect;
    public GameObject hitObject;
    public GameObject nextTurnBTN2;
    private int activePlayer;
    public BuildingTileUI buildingTileUI;
    public ShipyardTileUI shipyardTileUI;
    private ClickTile clickTile;

    public int test = 42;
    // Use this for initialization
    void Start () {
        _Camera = FindObjectOfType<Camera>();
	}

    // Update is called once per frame
    void Update()
    {        
        ClearHoverSelection();
        Ray ray = _Camera.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo))
        {
            hitObject = hitInfo.collider.transform.gameObject; // Works for tiles inside parent Folder
            
            if (hitObject.name == "FleetMesh")
            {
                hitObject = hitObject.transform.parent.gameObject;
            }           
                HoverObject(hitObject);
        }
        else
        {
           ClearHoverSelection();
        }        

        // Select objects by cklicking (on a hovered over Object)
        if (Input.GetMouseButton(0))
        {      
            // Hide Building Ui Panel
            if (hitObject.name != "TileConstructionPlayer1(Clone)" && hitObject.name != "TileConstructionPlayer2(Clone)")
            {
               buildingTileUI.Hide();
            }
            // Hide Shipyard Ui Panel
            if (hitObject.name != "TileShipYardPlayer1(Clone)" && hitObject.name != "TileShipYardPlayer2(Clone)")
            {                
                shipyardTileUI.Hide();
            }

            if (hitObject.name == "NextTurnBTN2")
            {
                ClearSelectedSelection();
            }
            else { 

                GetPlayer();
                int playerNumber = (int) char.GetNumericValue(hoverObject.name[hoverObject.name.Length - 8]);
          
                int playerFleet = 0;

                if (hoverObject.name.Contains("_P1_") && hoverObject.name.Contains("Fleet"))
                {
                    playerFleet = 1;
                }
                else if (hoverObject.name.Contains("_P2") && hoverObject.name.Contains("Fleet"))
                {
                    playerFleet = 2;
                }

                if (playerFleet != 0 || playerNumber == activePlayer)
                {
                    ClearSelectedSelection();
                    selectedObject = hoverObject;
                    SelectedObjects();
                }
            }

            if (hitObject.name == "BTN_Factory")
            {
                clickTile = selectedObject.GetComponent<ClickTile>();
                clickTile.BuildFactoryOnTile();
            }
            else if (hitObject.name == "BTN_Turret")
            {
                clickTile = selectedObject.GetComponent<ClickTile>();
                clickTile.BuildTurretOnTile();
            }
            else if (hitObject.name == "BTN_Solar")
            {
                clickTile = selectedObject.GetComponent<ClickTile>();
                clickTile.BuildSolarOnTile();
            }
            else if (hitObject.name == "BTN_Fighter")
            {
                clickTile = selectedObject.GetComponent<ClickTile>();
                if(activePlayer == 1)
                {
                    clickTile.BuildFighterOnTile(90);
                }
                else
                {
                    clickTile.BuildFighterOnTile(-90);
                }
                
            }
            else if (hitObject.name == "BTN_Bomber")
            {
                clickTile = selectedObject.GetComponent<ClickTile>();
                if (activePlayer == 1)
                {
                    clickTile.BuildBomberOnTile(90);
                }
                else
                {
                    clickTile.BuildBomberOnTile(-90);
                }
            }
            else if (hitObject.name == "BTN_Corvette")
            {
                clickTile = selectedObject.GetComponent<ClickTile>();
                if (activePlayer == 1)
                {
                    clickTile.BuildCorvetteOnTile();
                }
                else
                {
                    clickTile.BuildCorvetteOnTile(-180);
                }
            }
        }
    }

        // Hover over objects to get a 'light up', but no actual selection
        void HoverObject(GameObject obj)
    {  
        hoverObject = obj; 
        if(hoverObject == selectedObject)
        {
            return;
        }
        
        Renderer[] fleet = hoverObject.GetComponentsInChildren<Renderer>();
        Renderer fleetMesh = fleet[0];

        Material mat = fleetMesh.material;
        prevClrHover = mat.color;
        mat.color = Color.red;
        fleetMesh.material = mat;
    }

    // Actually make a 'permament' selection out of the hover over
    void SelectedObjects()
    {
        if (selectedObject == null)
        {
            return;
        }
        Renderer[] selectAry = selectedObject.GetComponentsInChildren<Renderer>();
        Renderer fleetMesh = selectAry[0];

        Material mat = fleetMesh.material;
        prevClrSelect = prevClrHover;
        mat.color = Color.white;
        fleetMesh.material = mat;
    }

    // Clear Selection for Hover
    void ClearHoverSelection()
    {
        if(hoverObject == null || hoverObject == selectedObject)
        {
            return;
        }

        Renderer[] fleet = hoverObject.GetComponentsInChildren<Renderer>();
        Renderer fleetMesh = fleet[0];

        Material mat = fleetMesh.material;
        mat.color = prevClrHover;
        fleetMesh.material = mat;
        hoverObject = null;
    }

    // Clear Selection for the 'permament' selection
    void ClearSelectedSelection()
    {
        if (selectedObject == null)
        {
            return;
        }

        Renderer[] selectAry = selectedObject.GetComponentsInChildren<Renderer>();
        Renderer fleetMesh = selectAry[0];

        Material mat = fleetMesh.material;
        mat.color = prevClrSelect;
        fleetMesh.material = mat;
        selectedObject = null;
    }
    
    private void GetPlayer()
    {
        GameObject thePlayer = GameObject.Find("NextTurnBTN2");
        NextTurnBTN2 nextTurnBtn2 = thePlayer.GetComponent<NextTurnBTN2>();
        activePlayer = nextTurnBtn2.activePlayer;
    }
}
