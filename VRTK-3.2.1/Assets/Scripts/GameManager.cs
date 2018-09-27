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

    [SerializeField]
    private ParticleSystem OnHitFX;

    private RhythmController rhythmController;
    private EnemyManager enemyManager;

    [SerializeField]
    private AudioSource beatSource, hitSource, missSource;

    private bool deactivatedModel = false;

    [SerializeField]
    private GameObject ArenaScoreDisplay;

    private VRTK.VRTK_ControllerReference controller;

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
        //beatSource = this.GetComponent<AudioSource>();
        ArenaScoreDisplay.SetActive(false);
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
        //Expensive but calling on start doesn't work
        if (!deactivatedModel)
        {
            GameObject controllerModel = GameObject.Find("Model");
            if (controllerModel != null)
            {
                controllerModel.SetActive(false);
                deactivatedModel = true;
            }
        }
    }

    private void OnBeatTrigger() {
        beatSource.Play();

        enemyManager.MoveEnemy(rhythmController.Beat);

        enemyManager.SpawnEnemy();
    }

	private void OnShotFired(GameObject enemy, Vector3 hitPoint){
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
            missSource.Play();
            return;
		}

        hitSource.Play();
		Enemy enemyHit = enemy.GetComponent<Enemy> ();

		enemyHit.TakeDamage (1);
		player.Heal((int)flowMultiplier);

		if (enemyHit.IsDead) {
			player.AddScore (enemyHit.score);
			enemyManager.DestroyEnemy (enemy);


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

    private void spawnText(string text, Transform transform) {
		GameObject newText = Instantiate (accuracyText);
		newText.GetComponent<RectTransform> ().SetPositionAndRotation (transform.position, transform.localRotation);
		newText.GetComponent<TMPro.TextMeshPro>().text = text;
	}

	private void HurtPlayer(int damage)
    {
        //Long strong on player gets hit
        HapticPulse(controller, 1.0f, 0.5f, 0.05f);
		player.TakeDamage (damage);
    }

    private void OnPlayerDead()
    {
        GameOver();
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

    public void HapticPulse(VRTK.VRTK_ControllerReference controllerReference, float strength, float duration, float interval)
    {
        VRTK.VRTK_ControllerHaptics.TriggerHapticPulse(controllerReference, Mathf.Clamp(strength, 0, 1.0f), duration, interval);
    }
}
