using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class HighscoresList : MonoBehaviour
{
    private List<PlayerScoreData> Highscores = new List<PlayerScoreData>();

    void Awake()
    {
        Load();
    }

    public List<PlayerScoreData> HIGHSCORES
    {
        get { return Highscores; }
    }

    public void AddToHighscoreList(PlayerScoreData playerScoreData)
    {
        Highscores.Add(playerScoreData);
        Highscores.Sort();
        Save();
    }

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        //Application.persistentDataPath is a string, so if you wanted you can put that into debug.log if you want to know where save games are located
        Debug.Log("__________Save Game is here: " + Application.persistentDataPath);
        FileStream file = File.Create(Application.persistentDataPath + "/highscores.save"); //you can call it anything you want
        bf.Serialize(file, Highscores);
        file.Close();
    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/highscores.save"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/highscores.save", FileMode.Open);
            Highscores = (List<PlayerScoreData>)bf.Deserialize(file);
            file.Close();
            Highscores.Sort();
        }
    }
}
