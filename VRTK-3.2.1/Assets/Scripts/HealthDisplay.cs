using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDisplay : MonoBehaviour {

	[SerializeField]
	Player player;
	[SerializeField]
	TMPro.TextMeshPro text;

	// Use this for initialization
	void Start () {
		text.text = "Player Health = " + player.Health;
	}
	
	// Update is called once per frame
	void Update () {
		text.text = "Player Health = " + player.Health;
	}
}
