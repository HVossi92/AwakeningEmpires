using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    public Text playerTurnText;
    public Text mineralsTxtP1;
    public Text minearlsTxtP2;
    public Text energyTxtP1;
    public Text energyTxtP2;
    public Text gameRoundTxt;
    public GameObject nextTurnBtn2;     
    private int activePlayer;
    private int gameRound;
    private Button nextTurn;
    private int mineralsP1 = 100;
    private int mineralsP2 = 25;
    private int energyP1 = 0;
    private int energyP2 = 0;
    private int mineralTick = 100;
    private int eneryPenalty = 2;

    // Use this for initialization
    void Start () {
        GetPlayer();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (playerTurnText == null)
        {
            reassignGameObjs();
        }
        SetText();        
    }

    // Updates only at new turn (gets called from NextTurnBtn2
    public void CallUpdates()
    {
        GetPlayer();
        PlayerMineralsRoundTick();        
    }

    public void reassignGameObjs()
    {
        playerTurnText = GameObject.Find("Turn_txt").GetComponent<Text>();
        mineralsTxtP1 = GameObject.Find("Minerals_txt_P1").GetComponent<Text>();
        minearlsTxtP2 = GameObject.Find("Minerals_txt_P2").GetComponent<Text>();
        energyTxtP1 = GameObject.Find("Energy_txt_P1").GetComponent<Text>();
        energyTxtP2 = GameObject.Find("Energy_txt_P2").GetComponent<Text>();
        gameRoundTxt = GameObject.Find("GameRound_txt").GetComponent<Text>();
        nextTurnBtn2 = GameObject.Find("NextTurnBTN2");
    }

    private void GetPlayer()
    {
        GameObject thePlayer = GameObject.Find("NextTurnBTN2");
        NextTurnBTN2 nextTurnBtn2 = thePlayer.GetComponent<NextTurnBTN2>();
        activePlayer = nextTurnBtn2.activePlayer;
        gameRound = nextTurnBtn2.gameRound;
    }

    // Sets UI Player relevant Text
    private void SetText()
    {
        playerTurnText.text = "Player: " + activePlayer + "\nGameround " + gameRound;
        energyTxtP1.text = "Energy P1: " + energyP1.ToString();
        energyTxtP2.text = "Energy P2: " + energyP2.ToString();
        mineralsTxtP1.text = "Minerals P1: " + mineralsP1.ToString();
        minearlsTxtP2.text = "Mienrals P2: " + mineralsP2.ToString();
        gameRoundTxt.text = gameRound.ToString();
    }

    // Resources
    private void PlayerMineralsRoundTick()
    {
        if(activePlayer == 1)
        {
            if (energyP1 < 0)
            {
                mineralTick /= eneryPenalty;
            }

            mineralsP1 += mineralTick;
        }else if(activePlayer == 2)
        {
            if (energyP2 < 0)
            {
                mineralTick /= eneryPenalty;
            }

            mineralsP2 += mineralTick;
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
            return mineralsP2;
        }
        else
        {
            print("Mineral Balance weirdness?");
            return 0;
        }
    }

    public int getEnergyBalance()
    {
        if (activePlayer == 1)
        {
            return energyP1;
        }
        else if (activePlayer == 2)
        {
            return energyP2;
        }
        else
        {
            print("Energy Balance weirdness?");
            return 0;
        }
    }

    public void mineralIncome(int n)
    {
        if (activePlayer == 1)
        {
            if (energyP1 < 0)
            {
                n /= eneryPenalty;
            }

            mineralsP1 += n;
        }
        else if (activePlayer == 2)
        {
            if (energyP2 < 0)
            {
                n /= eneryPenalty;
            }

            mineralsP2 += n;
        }
    }

    public void energyIncome(int n)
    {
        if (activePlayer == 1)
        {     
            energyP1 += n;
        }
        else if (activePlayer == 2)
        {    
            energyP2 += n;
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

    public void energyExpense(int n)
    {
        if (activePlayer == 1)
        {
            energyP1 -= n;
        }
        else if (activePlayer == 2)
        {
            energyP2 -= n;
        }
        else
        {
            print("Energy Weirdness!");
        }
    }

}
