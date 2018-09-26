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
    [SerializeField]
    AudioSource countSource;

    public void GameOverMenu(int score)
    {
        StartTarget.SetActive(false);
        RetryTarget.SetActive(true);
        //Score.text = "" + score;
        StartCoroutine(IncreaseScore(score));
    }

    IEnumerator IncreaseScore(int score)
    {
        int displayScore = 0;
        while (displayScore < score)
        {
            displayScore = Mathf.Min(displayScore + 200, score);
            Score.text = "" + displayScore;
            countSource.Play();
            yield return null;
        }
        yield return null;
    }
}
