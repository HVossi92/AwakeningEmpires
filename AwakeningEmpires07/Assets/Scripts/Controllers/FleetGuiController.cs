using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FleetGuiController : MonoBehaviour
{
    public GameObject fleetInventoryPrefab;
    public GameObject canvas;

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

    public GameObject selcFighterObj;
    public GameObject selcBomberObj;
    public GameObject selcCorvetteObj;
    private Text selcFighterTxt;
    private Text selcBomberTxt;
    private Text selcCorvetteTxt;

    private int fighterCount;
    private int bomberCount;
    private int corvetteCount;

    private int selcFighterCount;
    private int selcBomberCount;
    private int selcCorvetteCount;

    private bool activeGui = false;

    private void Awake()
    {
      /*  GameObject fleetInventory = Instantiate(fleetInventoryPrefab);
        fleetInventory.activeSelf = false;
        fleetInventory.transform.parent = canvas.transform;*/

        shipChildrenCollect = gameObject.GetComponent<ShipChildrenCollect>();
        mouseManager = mouseManagerObj.GetComponent<MouseManager>();

        fighterTxt = fighterObj.GetComponent<Text>();
        bomberTxt = bomberObj.GetComponent<Text>();
        corvetteTxt = corvetteObj.GetComponent<Text>();

        selcFighterTxt = selcFighterObj.GetComponent<Text>();
        selcBomberTxt = selcBomberObj.GetComponent<Text>();
        selcCorvetteTxt = selcCorvetteObj.GetComponent<Text>();

        ResetCounters();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ActivateFleetGui();
    }

    public int FighterCount
    {
        get { return fighterCount; }
    }

    public int BomberCount
    {
        get { return bomberCount; }
    }
    public int CorvetteCount
    {
        get { return corvetteCount; }
    }

    public int SelcFighterCount
    {
        get { return selcFighterCount; }
    }

    public int SelcBomberCount
    {
        get { return selcBomberCount; }
    }

    public int SelcCorvetteCount
    {
        get { return selcCorvetteCount; }
    }

    private void ActivateFleetGui()
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

                CountCurShips(shipChildren, loopIndex, ref fighterCount, ref bomberCount, ref corvetteCount);
            }
            else
            {
                fleetGui.SetActive(false);
                activeGui = false;
                ResetCounters();
            }
        }
    }

    private void CountCurShips(List<GameObject> shipChildren, int loopIndex, ref int fighterCount, ref int bomberCount, ref int corvetteCount)
    {
        if (!activeGui)
        {
            for (int i = 0; i <= loopIndex - 1; i++)
            {
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

            activeGui = true;
        }
    }

    public void SelcFighter()
    {
        if (selcFighterCount < fighterCount && fighterCount > 0)
        {
            selcFighterCount++;
            selcFighterTxt.text = selcFighterCount.ToString();
        }
    }

    public void SelcBomber()
    {
        if (selcBomberCount < bomberCount && bomberCount > 0)
        {
            selcBomberCount++;
            selcBomberTxt.text = selcBomberCount.ToString();
        }
    }

    public void SelcCorvette()
    {
        if (selcCorvetteCount < corvetteCount && corvetteCount > 0)
        {
            selcCorvetteCount++;
            selcCorvetteTxt.text = selcCorvetteCount.ToString();
        }
    }

    private void ResetCounters()
    {
        fighterCount = 0;
        bomberCount = 0;
        corvetteCount = 0;
        fighterTxt.text = "0";
        bomberTxt.text = "0";
        corvetteTxt.text = "0";

        selcFighterCount = 0;
        selcBomberCount = 0;
        selcCorvetteCount = 0;
        selcFighterTxt.text = "0";
        selcBomberTxt.text = "0";
        selcCorvetteTxt.text = "0";
    }
}
