using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour {

    [SerializeField]
    GameObject StartTarget;
    [SerializeField]
    GameObject RetryTarget;
    [SerializeField]
    TMPro.TextMeshPro Score;

    public void GameOverMenu(int score)
    {
        StartTarget.SetActive(false);
        RetryTarget.SetActive(true);
        Score.text = "" + score;
    }
}
