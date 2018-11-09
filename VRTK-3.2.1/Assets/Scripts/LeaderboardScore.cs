using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardScore : MonoBehaviour {

    [SerializeField]
    TMPro.TextMeshProUGUI playerRank;
    [SerializeField]
    TMPro.TextMeshProUGUI playerName;
    [SerializeField]
    TMPro.TextMeshProUGUI playerScore;

    public void FillScore(int rank, string name, int score)
    {
        playerRank.text = rank.ToString() + ".";
        playerName.text = name;
        playerScore.text = score.ToString();
    }
}
