using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDisplay : MonoBehaviour {

	[SerializeField]
	Player player;
	[SerializeField]
	TMPro.TextMeshPro health;
	[SerializeField]
	TMPro.TextMeshPro score;
	
	// Update is called once per frame
	void Update () {
		health.text = "Flow: " + player.Health;
		score.text = "Score: " + player.Score;
	}
}
