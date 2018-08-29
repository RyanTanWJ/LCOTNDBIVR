﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    [SerializeField]
    private KeyCode triggerKey;

    [SerializeField]
    private float enemySpawnChance;

    [SerializeField]
    private float offsetPerfect, offsetGreat, offsetOkay, offsetPoor;

	[SerializeField]
	private GameObject accuracyText;

    private RhythmController rhythmController;
    private EnemyManager enemyManager;
    private AudioSource audioSource;

    void OnEnable() {
        RhythmController.BeatTriggeredEvent += OnBeatTrigger;
		Shooting.ShotFiredEvent += OnShotFired;
    }

    void OnDisable() {
		RhythmController.BeatTriggeredEvent -= OnBeatTrigger;
		Shooting.ShotFiredEvent -= OnShotFired;
    }

    void Start() {
        rhythmController = this.GetComponentInChildren<RhythmController>();
        enemyManager = this.GetComponent<EnemyManager>();
        audioSource = this.GetComponent<AudioSource>();

        if (rhythmController == null) {
            Debug.LogError("No RhythmController attached to this object");
        }

        if (audioSource == null)
        {
            Debug.LogError("No AudioSource attached to this object");
        }
    }

    private void OnBeatTrigger() {
        audioSource.Play();
        if (Random.value <= enemySpawnChance) {
            enemyManager.SpawnEnemy();
        }
    }

	private void OnShotFired(GameObject enemy){
		//Do something when shot fired
		float rhythmState = rhythmController.GetCurrentBeat() % 1;

		if (rhythmState < offsetPerfect || 1 - offsetPerfect < rhythmState) {
			Debug.Log("Tapped at " + rhythmState + "s. Pefect");
			spawnText ("Perfect", enemy.transform);
		} else if (rhythmState < offsetGreat || 1 - offsetGreat < rhythmState) {
			Debug.Log("Tapped at " + rhythmState + "s. Great");
			spawnText ("Great", enemy.transform);
		} else if (rhythmState < offsetOkay || 1 - offsetOkay < rhythmState) {
			Debug.Log("Tapped at " + rhythmState + "s. Okay");
			enemy.GetComponent<MeshRenderer> ().enabled = false;
			spawnText ("Okay", enemy.transform);
		} else if (rhythmState < offsetPoor || 1 - offsetPoor < rhythmState) {
			Debug.Log("Tapped at " + rhythmState + "s. Poor");
			spawnText ("Poor", enemy.transform);
		} else {
			Debug.Log("Tapped at " + rhythmState + "s.Missed");
		}

		Destroy (enemy);


	}

	private void spawnText(string text, Transform transform) {
		Debug.Log ("hi");
		GameObject newText = Instantiate (accuracyText);
		newText.GetComponent<RectTransform> ().SetPositionAndRotation (transform.position, transform.localRotation);
		newText.GetComponent<TMPro.TextMeshPro>().text = text;
		Destroy (newText, 0.5f);
	}
}
