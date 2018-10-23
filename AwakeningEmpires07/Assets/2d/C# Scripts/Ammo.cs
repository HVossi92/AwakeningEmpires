using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{


    void OnTriggerEnter2D(Collider2D col){
        if (col.gameObject.tag=="player"){
            col.gameObject.GetComponent<player_controls>().AddAmmo();
            Destroy(gameObject);
        }
    }
}