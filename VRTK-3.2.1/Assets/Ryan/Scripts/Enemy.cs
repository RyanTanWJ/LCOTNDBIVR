using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	int id;
	int health;

	private float count = 0f;

	void OnEnable() {
		RhythmController.BeatTriggeredEvent += Move;
	}

	void OnDisable() {
		RhythmController.BeatTriggeredEvent -= Move;
	}

	private void Move() {
		count++;
		//Debug.Log ("hi");
		transform.Translate(new Vector3 (0,0, 2 * Mathf.Pow (-1.0f, count)));
	}
}
