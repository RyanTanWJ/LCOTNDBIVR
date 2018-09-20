using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

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

    private RhythmController rhythmController;
    private EnemyManager enemyManager;
    private AudioSource audioSource;

    void OnEnable() {
        RhythmController.BeatTriggeredEvent += OnBeatTrigger;
		Shooting.ShotFiredEvent += OnShotFired;
        Shooting.GameStartEvent += OnGameStart;
        Shooting.GameRestartEvent += OnGameRestart;
        EnemyManager.PlayerHurtEvent += HurtPlayer;
        FlowController.PlayerDeadEvent += OnPlayerDead;
    }

    void OnDisable() {
		RhythmController.BeatTriggeredEvent -= OnBeatTrigger;
		Shooting.ShotFiredEvent -= OnShotFired;
        Shooting.GameStartEvent -= OnGameStart;
        Shooting.GameRestartEvent -= OnGameRestart;
        EnemyManager.PlayerHurtEvent -= HurtPlayer;
        FlowController.PlayerDeadEvent -= OnPlayerDead;
    }

    void Start() {
        //rhythmController = this.GetComponentInChildren<RhythmController>();
        enemyManager = this.GetComponent<EnemyManager>();
        audioSource = this.GetComponent<AudioSource>();

        /*
        if (rhythmController == null) {
            Debug.LogError("No RhythmController attached to this object");
        }
        */

        if (audioSource == null)
        {
            Debug.LogError("No AudioSource attached to this object");
        }
    }

    private void OnBeatTrigger() {
        audioSource.Play();

        enemyManager.MoveEnemy();

        enemyManager.SpawnEnemy();
    }

	private void OnShotFired(GameObject enemy){
		//Do something when shot fired
		float rhythmState = rhythmController.GetCurrentBeat() % 1;
		float flowMultiplier = 0;

		if (rhythmState < offsetPerfect || 1 - offsetPerfect < rhythmState) {
			spawnText ("Perfect", enemy.transform);
			flowMultiplier = flowPerfect;
		} else if (rhythmState < offsetGreat || 1 - offsetGreat < rhythmState) {
			spawnText ("Great", enemy.transform);
			flowMultiplier = flowPerfect;
		} else if (rhythmState < offsetOkay || 1 - offsetOkay < rhythmState) {
			spawnText ("Okay", enemy.transform);
			flowMultiplier = flowOkay;
		} else if (rhythmState < offsetPoor || 1 - offsetPoor < rhythmState) {
			spawnText ("Poor", enemy.transform);
			flowMultiplier = flowPoor;
		} else {
			Debug.Log("Tapped at " + rhythmState + "s.Missed");
		}

		Enemy enemyHit = enemy.GetComponent<Enemy> ();
		enemyHit.TakeDamage (1);
		player.Heal((int)flowMultiplier);

		if (enemyHit.IsDead) {
			player.AddScore (enemyHit.score);
			enemyManager.DestroyEnemy (enemy);
		}
	}

    private void OnGameStart()
    {
        menu.SetActive(false);
        rhythmController = Instantiate(rhythmControllerPrefab, this.transform).GetComponent<RhythmController>();
    }

    private void OnGameRestart()
    {
        SceneManager.LoadScene("VRTest1");
    }

    private void spawnText(string text, Transform transform) {
		GameObject newText = Instantiate (accuracyText);
		newText.GetComponent<RectTransform> ().SetPositionAndRotation (transform.position, transform.localRotation);
		newText.GetComponent<TMPro.TextMeshPro>().text = text;
		Destroy (newText, 0.5f);
	}

	private void HurtPlayer(int damage){
		player.TakeDamage (damage);
    }

    private void OnPlayerDead()
    {
        GameOver();
    }

    private void GameOver()
    {
        //Destroy all Enemies
        enemyManager.DestroyAllEnemies();
        //Destroy RhythmController
        Destroy(rhythmController);
        //Reactivate Game Over Screen
        menu.SetActive(true);
        //Switch to Game Over menu
        menu.GetComponent<MenuController>().GameOverMenu(player.Score);
    }
}
