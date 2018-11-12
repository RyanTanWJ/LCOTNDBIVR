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
    GameObject SubmitScoreTarget;
    [SerializeField]
    List<GameObject> MainMenuOptions;
    [SerializeField]
    List<GameObject> DifficultyOptions;
    [SerializeField]
    TMPro.TextMeshPro RetryScore, WinScore;
    [SerializeField]
    AudioSource countSource, gameOverSource;
    
    void OnEnable()
    {
        Shooting.BackAltEvent += BackAlt;
        Shooting.DifficultySelectEvent += DifficultySelectMenu;
    }

    void OnDisable()
    {
        Shooting.BackAltEvent -= BackAlt;
        Shooting.DifficultySelectEvent -= DifficultySelectMenu;
    }

    private void BackAlt()
    {
        foreach(GameObject option in MainMenuOptions)
        {
            option.SetActive(true);
        }
        foreach (GameObject option in DifficultyOptions)
        {
            option.SetActive(false);
        }
    }

    public void DifficultySelectMenu()
    {
        foreach (GameObject option in DifficultyOptions)
        {
            option.SetActive(true);
        }
        foreach (GameObject option in MainMenuOptions)
        {
            option.SetActive(false);
        }
    }

    public void GameOverMenu(int score)
    {
        StartTarget.SetActive(false);
        StartCoroutine(IncreaseScore(score, RetryTarget, RetryScore));
    }


    public void GameWinMenu(int score) {
        StartTarget.SetActive(false);
        StartCoroutine(IncreaseScore(score, WinTarget, WinScore));
    }
    
    public void SubmitScoreMenu(int score)
    {
        RetryTarget.SetActive(false);
        WinTarget.SetActive(false);
        SubmitScoreTarget.SetActive(true);
        SubmitScoreTarget.GetComponent<SubmitScoreMenu>().OpenMenu(score);
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
