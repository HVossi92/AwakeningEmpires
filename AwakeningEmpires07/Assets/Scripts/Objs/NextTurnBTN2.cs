using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextTurnBTN2 : MonoBehaviour {

    public int gameRound;
    public int activePlayer;
    public PlayerController playerController;
    Renderer nextTurnBtnMat;


    // Use this for initialization
    private void Awake()
    {
        activePlayer = 1;
    }
    void Start () {
        gameRound = 1;     

        SetToBlack();
    }

    // Update is called once per frame
    void Update () {
         // print("AP" + activePlayer);
	}

    private void OnMouseDown()
    {
        SetToWhite();
        gameRound++;        
    }

    private void OnMouseUp()
    {
        if (activePlayer < 2)
        {
            activePlayer++;
        }
        else
        {
            activePlayer = 1;
        }
        playerController.CallUpdates();
        SetToBlack();
    }

    private void SetToWhite()
    {
        //Fetch the GameObject's Renderer component
        nextTurnBtnMat = GetComponent<Renderer>();
        //Change the GameObject's Material Color to red
        nextTurnBtnMat.material.color = Color.white;
    }

    private void SetToBlack()
    {
        //Fetch the GameObject's Renderer component
        nextTurnBtnMat = GetComponent<Renderer>();
        //Change the GameObject's Material Color to red
        nextTurnBtnMat.material.color = Color.black;
    }
}
