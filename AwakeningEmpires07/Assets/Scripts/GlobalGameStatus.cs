using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalGameStatus : MonoBehaviour
{
    private GameObject mapObj;
    private TileMap map;
    bool loadWorld = false;

    private void Start()
    {
        mapObj = GameObject.Find("Map");
        map = mapObj.GetComponent<TileMap>();
    }


    public void ResetMap()
    {
        print("reset map");
        SceneManager.LoadScene(3);
    }

    public void SaveMap()
    {
        print("save map");
    }

    public void LoadMap()
    {
        print("load map from save game");
        // SceneManager.LoadScene(3);
        map.GenerateMapSetUp();
    }
}