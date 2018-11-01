using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FleetCombatInfo : MonoBehaviour
{

    public int fightersP1 = 0;
    public int bombersP1 = 0;
    public int corvettesP1 = 0;

    public int fightersP2 = 0;
    public int bombersP2 = 0;
    public int corvettesP2 = 0;

    public bool postCombatFlag = false;

    private GameObject sluObj;
    private SaveLoadUtility slu;

    public static FleetCombatInfo Instance { get; private set; }

    private void Awake()
    {
        SingeltonFCI();

        LoadSaveLoadUT();
    }

    private void Update()
    {
        SingeltonFCI();
        ReloadGameStatus();
    }

    public void ReloadGameStatus()
    {
        int activeScene = SceneManager.GetActiveScene().buildIndex;
        if (postCombatFlag && activeScene == 3)
        {
            postCombatFlag = false;
            slu.LoadGame(slu.quickSaveName);
        }
    }

    private void LoadSaveLoadUT()
    {
        // Load in SaveLoad Utility
        if (slu == null)
        {
            sluObj = GameObject.Find("Persistent_SaveLoad_Obj_Menu");
            slu = sluObj.GetComponent<SaveLoadUtility>();

            if (slu == null)
            {
                Debug.Log("[SaveLoadMenu] Start(): Warning! SaveLoadUtility not assigned!");
            }
        }
    }

    private void SingeltonFCI()
    {
        if (Instance != null && Instance != this)
        {
            // If that is the case, we destroy other instances
            Destroy(gameObject);
        }

        // Here we save our singleton instance
        Instance = this;

        // Furthermore we make sure that we don't destroy between scenes (this is optional)
        DontDestroyOnLoad(gameObject);
    }

}
