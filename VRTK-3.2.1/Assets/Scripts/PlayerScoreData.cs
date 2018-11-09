using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerScoreData : System.IComparable<PlayerScoreData>
{
    private int playerScore;
    private string playerName;
    private System.DateTime dateTimeScoreAchieved;

    public PlayerScoreData(int score, string name, System.DateTime dateTime)
    {
        playerScore = score;
        playerName = name;
        dateTimeScoreAchieved = dateTime;
    }

    public int CompareTo(PlayerScoreData other)
    {
        if (playerScore == other.playerScore)
        {
            //Newer Score first
            return -dateTimeScoreAchieved.CompareTo(other.dateTimeScoreAchieved);
        }
        //Better score first
        return -playerScore.CompareTo(other.playerScore);
    }

    public int Score
    {
        get { return playerScore; }
    }

    public string Name
    {
        get { return playerName; }
    }
}
