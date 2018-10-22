using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuilding : MonoBehaviour
{
    protected GameObject playerControllerObj;
    protected PlayerController playerController;
    protected GameObject NextTurnBTN2Obj;
    protected NextTurnBTN2 nextTurnBTN2;
    protected int ofPlayer;
    protected int activePlayer;
    protected int gameRound;
    protected int currentGameRound;
    protected int eneryConsumption;

    protected void BuildingStartInit()
    {
        playerControllerObj = GameObject.Find("PlayerController");
        playerController = playerControllerObj.GetComponent<PlayerController>();
        NextTurnBTN2Obj = GameObject.Find("NextTurnBTN2");
        nextTurnBTN2 = NextTurnBTN2Obj.GetComponent<NextTurnBTN2>();
        ofPlayer = nextTurnBTN2.activePlayer;
        gameRound = nextTurnBTN2.gameRound;
    }

    protected void BuildingUpdateInit()
    {
        activePlayer = nextTurnBTN2.activePlayer;
        currentGameRound = nextTurnBTN2.gameRound;
    }

    protected void DrainEnergy(int n)
    {
        playerController.energyExpense(n);
    }
}
