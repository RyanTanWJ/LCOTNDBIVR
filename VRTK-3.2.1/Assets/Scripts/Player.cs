using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	[SerializeField]
	int maxHealth;
	HealthController health;

	void Awake () {
		health = new HealthController (maxHealth);
	}
	
	// Update is called once per frame
	void Update () {
	}

	public int Health
	{
		get { return health.Health; }
	}

	public void TakeDamage(int dmg)
	{
		health.TakeDamage (dmg);
	}
}
