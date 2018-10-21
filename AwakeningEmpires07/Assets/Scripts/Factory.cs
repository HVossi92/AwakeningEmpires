using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : PlayerBuilding {
    
    private int mineralOutput = 50;
    private bool factoryFlag = true;

	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        print(activePlayer);
        print(ofPlayer);
        if (ofPlayer == activePlayer && factoryFlag && currentGameRound > gameRound + 1)
        {
            EarnMinerals();
            factoryFlag = false;
        }
        else if(ofPlayer != activePlayer && !factoryFlag)
        {
            factoryFlag = true;
        }
	}

    private void EarnMinerals()
    {
        playerController.mineralIncome(mineralOutput);
    }
}
