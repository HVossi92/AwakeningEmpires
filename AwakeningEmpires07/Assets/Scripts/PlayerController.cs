using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    public Text playerTurnText;
    public Text energyTxtP1;
    public Text energyTxtP2;
    public GameObject nextTurnBtn2;     
    private int activePlayer;
    private int gameRound;
    private Button nextTurn;
    private int mineralsP1 = 100;
    private int mineralsP2 = 100;

    // Use this for initialization
    void Start () {
        GetPlayer();
    }
	
	// Update is called once per frame
	void Update ()
    {
        SetText();
    }

    public void CallUpdates()
    {
        GetPlayer();
        PlayerMineralsRoundTick();        
    }

    private void GetPlayer()
    {
        GameObject thePlayer = GameObject.Find("NextTurnBTN2");
        NextTurnBTN2 nextTurnBtn2 = thePlayer.GetComponent<NextTurnBTN2>();
        activePlayer = nextTurnBtn2.activePlayer;
        gameRound = nextTurnBtn2.gameRound;
    }

    private void SetText()
    {
        playerTurnText.text = "Player: " + activePlayer + "\nGameround " + gameRound;
        energyTxtP1.text = "Minerals P1: " + mineralsP1.ToString();
        energyTxtP2.text = "Mienrals P2: " + mineralsP2.ToString();
    }

    // Minerals
    private void PlayerMineralsRoundTick()
    {
        if(activePlayer == 1)
        {
            mineralsP1 += 100;
        }else if(activePlayer == 2)
        {
            mineralsP2 += 100;
        }
    }

    public int getMineralBalance()
    {
        if (activePlayer == 1)
        {
            return mineralsP1;
        }
        else if (activePlayer == 2)
        {
            return mineralsP1;
        }
        else
        {
            print("Mineral Balance weirdness?");
            return 0;
        }
    }

    public void mineralIncome(int n)
    {
        if (activePlayer == 1)
        {
            mineralsP1 += n;
        }
        else if (activePlayer == 2)
        {
            mineralsP2 += n;
        }
    }

    public void mineralExpense(int n)
    {
        if (activePlayer == 1 && mineralsP1 >= n)
        {
            mineralsP1 -= n;
        }
        else if (activePlayer == 2 && mineralsP2 >= n)
        {
            mineralsP2 -= n;
        }
        else
        {
            print("Insufficient Minerals!");
        }
    }

}
