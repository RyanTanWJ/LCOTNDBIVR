using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScoreData : IComparer<PlayerScoreData>
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

    public int Compare(PlayerScoreData x, PlayerScoreData y)
    {
        if (x.playerScore == y.playerScore)
        {
            return x.dateTimeScoreAchieved.CompareTo(y.dateTimeScoreAchieved);
        }
        return x.playerScore.CompareTo(y.playerScore);
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
