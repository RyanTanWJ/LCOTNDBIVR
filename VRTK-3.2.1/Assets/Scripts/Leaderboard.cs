using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaderboard : MonoBehaviour {

    [SerializeField]
    private GameObject ScoresHolder;

    [SerializeField]
    private LeaderboardScore LeaderboardScorePrefab;

    private List<PlayerScoreData> Highscores = new List<PlayerScoreData>();

    //Only keeps track of this number of player scores
    const int TopX = 5;

    void Start()
    {
        PopulateLeaderboard();
    }

    private void PopulateLeaderboard()
    {
        //Header
        Instantiate(LeaderboardScorePrefab, ScoresHolder.transform);
        Highscores.Sort();
        //Populate the Leaderboard
        LeaderboardScore leaderboardScore;
        for (int i = 0; i < TopX; i++)
        {
            leaderboardScore = Instantiate(LeaderboardScorePrefab, ScoresHolder.transform);
            if (i < Highscores.Count)
            {
                leaderboardScore.FillScore(i + 1, Highscores[i].Name, Highscores[i].Score);
                continue;
            }
            leaderboardScore.FillScore(i + 1, "", 0);
        }
    }
}
