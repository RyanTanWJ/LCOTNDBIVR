using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour {

    [SerializeField]
    GameObject StartTarget;
    [SerializeField]
    GameObject RetryTarget;
    [SerializeField]
    GameObject WinTarget;
    [SerializeField]
    TMPro.TextMeshPro RetryScore, WinScore;
    [SerializeField]
    AudioSource countSource, gameOverSource;

    public void GameOverMenu(int score)
    {
        StartTarget.SetActive(false);
        StartCoroutine(IncreaseScore(score, RetryTarget, RetryScore));
    }


    public void GameWinMenu(int score) {
        StartTarget.SetActive(false);
        StartCoroutine(IncreaseScore(score, WinTarget, WinScore));
    }


    IEnumerator GameOverTriggered()
    {
        gameOverSource.Play();

        while(gameOverSource.isPlaying)
        {
            yield return null;
        }
        yield return null;
    }

    IEnumerator IncreaseScore(int score, GameObject targetCanvas, TMPro.TextMeshPro scoreText)
    {
        yield return StartCoroutine(GameOverTriggered());

        targetCanvas.SetActive(true);

        int displayScore = 0;
        int scoreIncrement = (int) ((float)score / (4f * 60f));
        countSource.Play();

        while (displayScore < score)
        {
            displayScore = Mathf.Min(displayScore + scoreIncrement, score);

            if (scoreIncrement == 0)
            {
                displayScore = score;
            }

            scoreText.text = "" + displayScore;

            yield return null;
        }
        countSource.Stop();
        yield return null;
    }
}
