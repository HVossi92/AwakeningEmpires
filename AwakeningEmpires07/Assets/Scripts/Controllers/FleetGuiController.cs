using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FleetGuiController : MonoBehaviour
{
    private ShipChildrenCollect shipChildrenCollect;
    public GameObject mouseManagerObj;
    private MouseManager mouseManager;
    private GameObject selectedFleet;
    private GameObject mouseSelect;
    public GameObject fleetGui;

    public GameObject fighterObj;
    public GameObject bomberObj;
    public GameObject corvetteObj;
    private Text fighterTxt;
    private Text bomberTxt;
    private Text corvetteTxt;
    
    // Use this for initialization
    void Start()
    {
        shipChildrenCollect = gameObject.GetComponent<ShipChildrenCollect>();
        mouseManager = mouseManagerObj.GetComponent<MouseManager>();
        fighterTxt = fighterObj.GetComponent<Text>();
        bomberTxt = bomberObj.GetComponent<Text>();
        corvetteTxt = corvetteObj.GetComponent<Text>();
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

                int fighterCount = 0;
                int bomberCount = 0;
                int corvetteCount = 0;
                for (int i = 0; i <= loopIndex - 1; i++)
                {
                    print(i);
                    string curShip = shipChildren[i].name;
                    Transform parentFolder = fleetGui.transform.GetChild(0).transform;     
                    
                    switch (curShip)
                    {
                        case "Fighter(Clone)":
                            fighterCount++;
                            break;
                        case "Bomber(Clone)":
                            bomberCount++;
                            break;
                        case "Corvette(Clone)":
                            corvetteCount++;
                            break;
                        default:
                            print("Problem");                            
                            break;
                    }
                }

                fighterTxt.text = fighterCount.ToString();
                bomberTxt.text = bomberCount.ToString();
                corvetteTxt.text = corvetteCount.ToString();
            }
            else
            {
                fleetGui.SetActive(false);
            }
        }
    }
}
