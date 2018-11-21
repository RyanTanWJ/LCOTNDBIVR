using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmitScoreMenu : MonoBehaviour
{

    public delegate void SubmitScoreToLeaderboard(PlayerScoreData playerScoreData);
    public static event SubmitScoreToLeaderboard SubmitScoreToLeaderboardEvent;

    [SerializeField]
    TMPro.TextMeshProUGUI nameField;

    [SerializeField]
    TMPro.TextMeshPro scoreField;

    private const int charLimit = 10;

    const string emptyName = "";
    int playerScore = 0;
    string playerName = emptyName;

    void OnEnable()
    {
        Shooting.KeyboardEvent += PressButton;
    }

    void OnDisable()
    {
        Shooting.KeyboardEvent -= PressButton;
    }

    public void OpenMenu(int score)
    {
        playerName = emptyName;
        playerScore = score;
        nameField.text = playerName;
        scoreField.text = playerScore.ToString();
    }

    public void PressButton(string buttonPressed)
    {
        switch (buttonPressed)
        {
            case "DEL":
                if (playerName.Length > 0)
                {
                    playerName.Remove(playerName.Length - 1);
                }
                nameField.text = playerName;
                break;
            case "Submit":
                if (playerName == emptyName)
                {
                    playerName = "Anon" + Random.Range(1, 9000);
                }
                SubmitScoreToLeaderboardEvent(new PlayerScoreData(playerScore, playerName, System.DateTime.UtcNow));
                break;
            default:
                if (playerName.Length < charLimit)
                {
                    playerName += buttonPressed;
                    nameField.text = playerName;
                }
                break;
        }
    }
}
