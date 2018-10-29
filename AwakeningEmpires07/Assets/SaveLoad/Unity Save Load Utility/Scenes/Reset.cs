using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Reset : MonoBehaviour {

    public List<SaveGame> saveGames;
    public SaveLoadUtility slu;

    private void ReloadSene()
    {
        slu.SaveGame(slu.quickSaveName);
        Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
    }
}
