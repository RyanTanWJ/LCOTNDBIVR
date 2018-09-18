using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	[SerializeField]
	int maxHealth;

    [SerializeField]
    int maxFlow, startingFlow;

    int score = 0;

	HealthController health;
    FlowController flow;

	void Awake () {
        //health = new HealthController (maxHealth);
        //flow = gameObject.AddComponent<FlowController>();
        flow = new FlowController(maxFlow, startingFlow);
	}

	public int Health
	{
		//get { return health.Health; }
        get { return flow.Flow; }
	}

	public int Score
	{
		get { return score; }
	}

    public void TakeDamage(int dmg)
    {
        //health.TakeDamage (dmg);
		flow.TakeDamage(FlowMultiplier(dmg));
    }

    public void Heal(int health)
    {
		flow.Heal(FlowMultiplier(health));
    }

    public bool IsDead() {
        return flow.IsDead;
    }

	//TODO: Implement Score Multiplier based on flow
	private int FlowMultiplier(int value){
		return value;
	}


	//TODO: Implement Score Multiplier based on flow
	private int ScoreMultiplier(int value){
		return flow.Flow / 100 * 5 * value;
	}

	/// <summary>
	/// Adds the score with player's multiplier to the current score
	/// </summary>
	/// <param name="value">The destroyed enemy's base score.</param>
	public void AddScore(int value){
		score += ScoreMultiplier (value);
	}
}
