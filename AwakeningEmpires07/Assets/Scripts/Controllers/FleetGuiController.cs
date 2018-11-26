using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleetGuiController : MonoBehaviour
{

    private ShipChildrenCollect shipChildrenCollect;
    public GameObject mouseManagerObj;
    private MouseManager mouseManager;
    private GameObject selectedFleet;
    private GameObject mouseSelect;
    public GameObject fleetGui;
    public GameObject guiFighterBtn;
    public GameObject guiBomberBtn;
    public GameObject guiCorvetteBtn;

    // Use this for initialization
    void Start()
    {
        shipChildrenCollect = gameObject.GetComponent<ShipChildrenCollect>();
        mouseManager = mouseManagerObj.GetComponent<MouseManager>();
    }

    private void OnMouseDown()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonUp(0))
        {
            mouseSelect = mouseManager.selectedObject;
            if (mouseSelect != null && mouseSelect.name.StartsWith("Fleet"))
            {
                fleetGui.SetActive(true);
                selectedFleet = mouseSelect;
                GameObject shipHolder = selectedFleet.gameObject.transform.GetChild(1).gameObject;
                List<GameObject> shipChildren = shipChildrenCollect.ShipChildrenList(shipHolder);
                int loopIndex = shipChildren.Count;

                for (int i = 0; i <= loopIndex - 1; i++)
                {
                    print(i);
                    string curShip = shipChildren[i].name;
                    GameObject btn;
                    Transform parentFolder = fleetGui.transform.GetChild(0).transform;
                    switch (curShip)
                    {
                        case "Fighter(Clone)":
                            btn = Instantiate(guiFighterBtn, parentFolder, false);
                            break;
                        case "Bomber(Clone)":
                            btn = Instantiate(guiBomberBtn, parentFolder, false);
                            break;
                        case "Corvette(Clone)":
                            btn = Instantiate(guiCorvetteBtn, parentFolder, false);
                            break;
                        default:
                            print("Error instantiating Fleet Gui");
                            break;
                    }
                }
            }
            else
            {
                fleetGui.SetActive(false);
                if (fleetGui.transform.childCount > 0)
                {
                    for (int i = 0; i < fleetGui.transform.childCount; i++)
                    {
                        Destroy(fleetGui.transform.GetChild(i));
                    }
                }
            }
        }
    }
}
