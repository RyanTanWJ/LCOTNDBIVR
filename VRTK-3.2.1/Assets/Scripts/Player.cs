﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	[SerializeField]
	int maxHealth;

    [SerializeField]
    int maxFlow, startingFlow;

    int score = 0;

    FlowController flow;

    bool isTutorial = true;

    bool hardMode = false;

	void Awake () {
        //health = new HealthController (maxHealth);
        //flow = gameObject.AddComponent<FlowController>();
        flow = new FlowController(maxFlow, startingFlow);
	}

	public int Health
    {
        get { return flow.Flow; }
	}

	public int Score
	{
		get { return score; }
	}

    public void TakeDamage(int dmg)
    {
        if (isTutorial)
        {
            return;
        }
		flow.TakeDamage(FlowMultiplier(dmg));
    }

    public void Heal(int health)
    {
        if (isTutorial)
        {
            return;
        }
        flow.Heal(FlowMultiplier(health));
    }

    public bool IsDead() {
        return flow.IsDead;
    }

	private int FlowMultiplier(int value){
		return value;
	}
		
	private int ScoreMultiplier(int value){
        int scoreToAdd = flow.Flow * value;
        if (hardMode)
        {
            scoreToAdd = Mathf.CeilToInt(scoreToAdd * 1.2f);
        }
		return scoreToAdd;
	}

    public void ResetPlayer(bool hard)
    {
        hardMode = hard;
        score = 0;
        flow.Reset(startingFlow);
        OffTutorial();
    }

	/// <summary>
	/// Adds the score with player's multiplier to the current score
	/// </summary>
	/// <param name="value">The destroyed enemy's base score.</param>
	public void AddScore(int value){
		score += ScoreMultiplier (value);
	}

    private void OffTutorial()
    {
        isTutorial = false;
    }
}
