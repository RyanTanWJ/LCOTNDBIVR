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
    AudioSource countSource, gameOverSource;

    public void GameOverMenu(int score)
    {
        StartTarget.SetActive(false);
        StartCoroutine(IncreaseScore(score));
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

    IEnumerator IncreaseScore(int score)
    {
        yield return StartCoroutine(GameOverTriggered());

        RetryTarget.SetActive(true);

        int displayScore = 0;
        int scoreIncrement = (int) ((float)score / (4f * 60f));
        countSource.Play();
        while (displayScore < score)
        {
            displayScore = Mathf.Min(displayScore + scoreIncrement, score);
            Score.text = "" + displayScore;
            
            yield return null;
        }
        countSource.Stop();
        yield return null;
    }
}
