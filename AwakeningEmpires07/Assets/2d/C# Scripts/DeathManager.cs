using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathManager : MonoBehaviour {

    GameObject player;
    bool gameOver = false;
    private GameObject fleetCombatInfoObj;
    private FleetCombatInfo fleetCombatInfo;

    private void Awake()
    {
        fleetCombatInfoObj = GameObject.Find("FleetCombatInfo");
        fleetCombatInfo = fleetCombatInfoObj.GetComponent<FleetCombatInfo>();
    }

    void Start() {
        player = GameObject.FindGameObjectWithTag("player");

    }


    void Update() {

        if (player == null&&!gameOver)
            GameOver();

    }

    void GameOver() {
        gameOver = true;
        if (PlayerPrefs.GetInt("Score") > PlayerPrefs.GetInt("HighScore"))
            PlayerPrefs.SetInt("HighScore", PlayerPrefs.GetInt("Score"));
        StartCoroutine(LoadGameOver());



   }
    IEnumerator LoadGameOver() { 
        yield return new WaitForSeconds(1f);
        fleetCombatInfo.postCombatFlag = true;
        SceneManager.LoadScene(3);
    

    }
}
