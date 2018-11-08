using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuilding : PlayerPawn
{
    protected GameObject NextTurnBTN2Obj;
    protected NextTurnBTN2 nextTurnBTN2;
    protected int ofPlayer;
    protected int activePlayer;
    protected int gameRound;
    protected int curGameRound;
    protected int eneryConsumption;

    protected void BuildingStartInit()
    {
        NextTurnBTN2Obj = GameObject.Find("NextTurnBTN2");
        nextTurnBTN2 = NextTurnBTN2Obj.GetComponent<NextTurnBTN2>();
        ofPlayer = nextTurnBTN2.activePlayer;
        gameRound = nextTurnBTN2.gameRound;
        PlayerPawnStartInit();
    }

    protected void BuildingUpdateInit()
    {
        activePlayer = nextTurnBTN2.activePlayer;
        curGameRound = nextTurnBTN2.gameRound;
    }

    protected void DrainEnergy(int n)
    {
        playerController.energyExpense(n);
    }
}
