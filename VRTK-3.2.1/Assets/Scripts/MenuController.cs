using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour {

    [SerializeField]
    GameObject StartTarget;
    [SerializeField]
    GameObject RetryTarget;
    [SerializeField]
    TMPro.TextMeshPro Title;

    public void GameOverMenu(int score)
    {
        StartTarget.SetActive(false);
        RetryTarget.SetActive(true);
        Title.text = "Game Over!\nScore: " + score;
        Title.alignment = TMPro.TextAlignmentOptions.Center;
        Title.fontSize = 20f;
    }
}
