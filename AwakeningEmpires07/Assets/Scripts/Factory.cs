using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : PlayerBuilding {
    
    private int mineralOutput = 50;
    private bool factoryFlag = true;
	// Use this for initialization
	void Start () {
        eneryConsumption = 50;
        BuildingStartInit();
        DrainEnergy(eneryConsumption);
    }
	
	// Update is called once per frame
	void Update ()
    {
        BuildingUpdateInit();
        EarnMinearlsCheck();
    }

    private void EarnMinearlsCheck()
    {
        if (ofPlayer == activePlayer && factoryFlag && currentGameRound > gameRound + 1)
        {
            EarnMinerals();
            factoryFlag = false;
        }
        else if (ofPlayer != activePlayer && !factoryFlag)
        {
            factoryFlag = true;
        }
    }

    private void EarnMinerals()
    {
        playerController.mineralIncome(mineralOutput);
    }
}
