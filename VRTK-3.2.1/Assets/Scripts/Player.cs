using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	[SerializeField]
	int maxHealth;
	int score = 0;
	HealthController health;

	void Awake () {
		health = new HealthController (maxHealth);
	}

	public int Health
	{
		get { return health.Health; }
	}

	public int Score
	{
		get { return score; }
	}

	public void TakeDamage(int dmg)
	{
		health.TakeDamage (dmg);
	}

	//TODO: Implement Score Multiplier based on flow
	private int Multiplier(int value){
		return value;
	}

	/// <summary>
	/// Adds the score with player's multiplier to the current score
	/// </summary>
	/// <param name="value">The destroyed enemy's base score.</param>
	public void AddScore(int value){
		score += Multiplier (value);
	}
}
