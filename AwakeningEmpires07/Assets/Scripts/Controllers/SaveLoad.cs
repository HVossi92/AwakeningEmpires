using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveLoad {

    public static void Save(Game saveGame)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create("C:/Users/HVossi92/Documents/AwakeningEmpires/" + saveGame.savegameName + ".sav");
        bf.Serialize(file, saveGame);
        file.Close();
        Debug.Log("Game saved " + saveGame.savegameName);
    }

    public static Game Load(string gameToLoad)
    {
        if(File.Exists("C:/Users/HVossi92/Documents/AwakeningEmpires/" + gameToLoad + ".sav"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open("C:/Users/HVossi92/Documents/AwakeningEmpires/" + gameToLoad + ".gd", FileMode.Open);
            Game loadedGame = (Game)bf.Deserialize(file);
            file.Close();
            Debug.Log("Game loaded: " + loadedGame.savegameName);
        }
        else
        {
            Debug.Log("File doesn't exist");            
        }
        return null;
    }
}
