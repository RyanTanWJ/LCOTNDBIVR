using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController {

	int maxHealth;
	int currHealth;

	public HealthController(int max){
		maxHealth = max;
		currHealth = max;
	}

	public int Health
	{
		get { return currHealth; }
	}

	public int MaxHealth
	{
		get { return maxHealth; }
	}

	public void TakeDamage(int damage)
	{
		currHealth -= damage;
	}

	public void Heal(int heal)
	{
		currHealth += heal;
		if (currHealth > maxHealth)
		{
			currHealth = maxHealth;
		}
	}

	public bool IsDead
	{
		get { return currHealth<=0; }
	}
}
