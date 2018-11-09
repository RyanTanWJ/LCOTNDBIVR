using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public delegate void Pulse(bool hit, bool isLeft);
    public static event Pulse PulseEvent;

    [SerializeField]
    private KeyCode triggerKey;

    [SerializeField]
    private float offsetPerfect, offsetGreat, offsetOkay, offsetPoor;

    [SerializeField]
    private float flowPerfect, flowGreat, flowOkay, flowPoor;

    [SerializeField]
    private GameObject accuracyText;

    [SerializeField]
    private Player player;

    [SerializeField]
    private GameObject rhythmControllerPrefab;

    [SerializeField]
    private GameObject menu;
    [SerializeField]
    private Leaderboard leaderboard;
    [SerializeField]
    private GameObject credits;

    [SerializeField]
    private ParticleSystem OnHitFX;

    private RhythmController rhythmController;
    private EnemyManager enemyManager;

    [SerializeField]
    private AudioSource beatSource, hitSource, missSource;

    [SerializeField]
    HealthDisplay healthDisplay;

    [SerializeField]
    private GameObject ArenaScoreDisplay;

    [SerializeField]
    private GameObject HighscoresListObj;

    [SerializeField]
    private HighscoresList highscoresList;

    private VRTK.VRTK_ControllerReference controller;

    void OnEnable()
    {
        RhythmController.BeatTriggeredEvent += OnBeatTrigger;
        Shooting.ShotFiredEvent += OnShotFired;
        Shooting.GameStartEvent += OnGameStart;
        Shooting.GameRestartEvent += OnGameRestart;
        Shooting.CreditsEvent += OnCredits;
        Shooting.BackEvent += OnBack;
        Shooting.SubmitScoreMenuEvent += OpenSubmitScoreMenu;
        EnemyManager.PlayerHurtEvent += HurtPlayer;
        EnemyManager.WinGameEvent += WinGame;
        EnemyWaveManager.TutorialEndEvent += ResetPlayer;
        FlowController.PlayerDeadEvent += OnPlayerDead;
        SubmitScoreMenu.SubmitScoreToLeaderboardEvent += AddScore;
    }

    void OnDisable()
    {
        RhythmController.BeatTriggeredEvent -= OnBeatTrigger;
        Shooting.ShotFiredEvent -= OnShotFired;
        Shooting.GameStartEvent -= OnGameStart;
        Shooting.GameRestartEvent -= OnGameRestart;
        Shooting.CreditsEvent -= OnCredits;
        Shooting.BackEvent -= OnBack;
        Shooting.SubmitScoreMenuEvent -= OpenSubmitScoreMenu;
        EnemyManager.PlayerHurtEvent -= HurtPlayer;
        EnemyManager.WinGameEvent -= WinGame;
        EnemyWaveManager.TutorialEndEvent -= ResetPlayer;
        FlowController.PlayerDeadEvent -= OnPlayerDead;
        SubmitScoreMenu.SubmitScoreToLeaderboardEvent -= AddScore;
    }

    void Start()
    {
        //rhythmController = this.GetComponentInChildren<RhythmController>();
        enemyManager = this.GetComponent<EnemyManager>();
        //beatSource = this.GetComponent<AudioSource>();
        ArenaScoreDisplay.SetActive(false);
        leaderboard.PopulateLeaderboard(highscoresList.HIGHSCORES);
        credits.SetActive(false);
        /*
        if (rhythmController == null) {
            Debug.LogError("No RhythmController attached to this object");
        }
        */

        if (beatSource == null)
        {
            Debug.LogError("No AudioSource attached to this object");
        }
    }

    void Update()
    {
        if (enemyManager.isTutorial())
        {
            healthDisplay.setHealth("");
            if (enemyManager.CurrentWave() == 1)
            {
                healthDisplay.setScore("Shoot enemies\non the beat");
            }
            if (enemyManager.CurrentWave() == 2)
            {
                healthDisplay.setScore("Some enemies\nmove on the beat");
            }
            if (enemyManager.CurrentWave() == 3)
            {
                healthDisplay.setScore("Enemies can\nappear together");
            }
        }
        else
        {
            healthDisplay.setScore("Score: " + player.Score);
            healthDisplay.setHealth("Multiplier: " + (float)player.Health / 100f + "x");
        }
    }

    private void OnBeatTrigger()
    {
        beatSource.Play();

        enemyManager.MoveEnemy(rhythmController.Beat);

        enemyManager.SpawnEnemy();
    }

    private void OnShotFired(GameObject enemy, Vector3 hitPoint, bool isLeft)
    {
        //Do something when shot fired
        float rhythmState = rhythmController.GetCurrentBeat() % 1;
        float flowMultiplier = 0;

        if (rhythmState < offsetPerfect || 1 - offsetPerfect < rhythmState)
        {
            spawnText("Perfect", enemy.transform);
            flowMultiplier = flowPerfect;
            PulseEvent(true, isLeft);
        }
        else if (rhythmState < offsetGreat || 1 - offsetGreat < rhythmState)
        {
            spawnText("Great", enemy.transform);
            flowMultiplier = flowPerfect;
            PulseEvent(true, isLeft);
        }
        else if (rhythmState < offsetOkay || 1 - offsetOkay < rhythmState)
        {
            spawnText("Okay", enemy.transform);
            flowMultiplier = flowOkay;
            PulseEvent(true, isLeft);
        }
        else if (rhythmState < offsetPoor || 1 - offsetPoor < rhythmState)
        {
            spawnText("Poor", enemy.transform);
            flowMultiplier = flowPoor;
            PulseEvent(true, isLeft);
        }
        else
        {
            spawnText("Beat Missed", enemy.transform);
            player.TakeDamage(1);
            missSource.Play();
            PulseEvent(false, isLeft);
            return;
        }

        hitSource.Play();
        Enemy enemyHit = enemy.GetComponent<Enemy>();

        enemyHit.TakeDamage(1);
        player.Heal((int)flowMultiplier);

        if (enemyHit.IsDead)
        {
            player.AddScore(enemyHit.score);
            enemyManager.DestroyEnemy(enemy);


            GameObject HitFX = Instantiate(OnHitFX.gameObject);
            HitFX.transform.position = hitPoint;
            ParticleSystem HitFXPS = HitFX.GetComponent<ParticleSystem>();
            HitFXPS.Play();
            Destroy(HitFX.gameObject, HitFXPS.main.startLifetime.constantMax + HitFXPS.main.duration);
        }
    }

    private void OnGameStart(VRTK.VRTK_ControllerReference CR)
    {
        controller = CR;
        ArenaScoreDisplay.SetActive(true);
        menu.SetActive(false);
        rhythmController = Instantiate(rhythmControllerPrefab, this.transform).GetComponent<RhythmController>();
    }

    private void OnGameRestart()
    {
        SceneManager.LoadScene("VRTest1");
    }

    private void spawnText(string text, Transform transform)
    {
        GameObject newText = Instantiate(accuracyText);
        newText.GetComponent<RectTransform>().SetPositionAndRotation(transform.position, transform.localRotation);
        newText.GetComponent<TMPro.TextMeshPro>().text = text;
    }

    private void HurtPlayer(int damage)
    {
        //Long strong on player gets hit
        HapticPulse(controller, 1.0f, 0.5f, 0.05f);
        player.TakeDamage(damage);
    }

    private void ResetPlayer()
    {
        player.ResetPlayer();
    }

    private void OnPlayerDead()
    {
        GameOver();
    }

    private void WinGame()
    {
        if (player.Health <= 0)
        {
            return;
        }
        ArenaScoreDisplay.SetActive(false);
        //Destroy all Enemies
        enemyManager.DestroyAllEnemies();
        //Destroy RhythmController
        Destroy(rhythmController);
        //Reactivate Start/GameOver Screen
        menu.SetActive(true);
        //Switch to Game Win menu
        menu.GetComponent<MenuController>().GameWinMenu(player.Score);
    }

    private void GameOver()
    {
        ArenaScoreDisplay.SetActive(false);
        //Destroy all Enemies
        enemyManager.DestroyAllEnemies();
        //Destroy RhythmController
        Destroy(rhythmController);
        //Reactivate Game Over Screen
        menu.SetActive(true);
        //Switch to Game Over menu
        menu.GetComponent<MenuController>().GameOverMenu(player.Score);
    }

    private void OpenSubmitScoreMenu()
    {
        menu.GetComponent<MenuController>().SubmitScoreMenu(player.Score);
    }

    private void OnBack()
    {
        menu.SetActive(true);
        credits.SetActive(false);
    }

    private void OnCredits()
    {
        menu.SetActive(false);
        credits.SetActive(true);
    }

    public void HapticPulse(VRTK.VRTK_ControllerReference controllerReference, float strength, float duration, float interval)
    {
        VRTK.VRTK_ControllerHaptics.TriggerHapticPulse(controllerReference, Mathf.Clamp(strength, 0, 1.0f), duration, interval);
    }

    public void AddScore(PlayerScoreData playerScoreData)
    {
        highscoresList.AddToHighscoreList(playerScoreData);
        //leaderboard.PopulateLeaderboard(highscoresList.HIGHSCORES);
        OnGameRestart();
    }
}
