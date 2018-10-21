using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuilding : MonoBehaviour
{
    public GameObject playerControllerObj;
    public PlayerController playerController;
    public GameObject NextTurnBTN2Obj;
    public NextTurnBTN2 nextTurnBTN2;
    public int ofPlayer;
    public int activePlayer;
    public int gameRound;
    public int currentGameRound;

    // Use this for initialization
    void Start()
    {
        playerControllerObj = GameObject.Find("PlayerController");
        playerController = playerControllerObj.GetComponent<PlayerController>();
        NextTurnBTN2Obj = GameObject.Find("NextTurnBTN2");
        nextTurnBTN2 = NextTurnBTN2Obj.GetComponent<NextTurnBTN2>();
        ofPlayer = nextTurnBTN2.activePlayer;
        gameRound = nextTurnBTN2.gameRound;
    }

    // Update is called once per frame
    void Update()
    {
        print(activePlayer);
        activePlayer = nextTurnBTN2.activePlayer;
        currentGameRound = nextTurnBTN2.gameRound;
    }
}
