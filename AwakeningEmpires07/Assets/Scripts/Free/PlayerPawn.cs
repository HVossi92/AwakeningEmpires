using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPawn : MonoBehaviour {

    protected GameObject playerControllerObj;
    protected PlayerController playerController;
    protected FleetCollision fleetCollision;

    protected void PlayerPawnStartInit()
    {
        playerControllerObj = GameObject.Find("PlayerController");
        playerController = playerControllerObj.GetComponent<PlayerController>();
        fleetCollision = playerControllerObj.GetComponent<FleetCollision>();
    }
}
