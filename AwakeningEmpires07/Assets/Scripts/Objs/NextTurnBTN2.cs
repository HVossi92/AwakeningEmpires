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
        gameRound = 1;
        
    }
    void Start () {  

        SetToBlack();
    }

    // Update is called once per frame
    void Update () {
         if (playerController == null)
        {
            reassignGameObjs();
        }
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

    public void reassignGameObjs()
    {
        GameObject playerControllerObj = GameObject.Find("PlayerController");
        playerController = playerControllerObj.GetComponent<PlayerController>();
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
