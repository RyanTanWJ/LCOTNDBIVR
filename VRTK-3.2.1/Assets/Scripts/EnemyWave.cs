using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWave {

	public int EnemiesLeft;
	public List<int> EnemyTypes;

	public EnemyWave(int enemiesInWave, List<int> enemyTypes){
		EnemiesLeft = enemiesInWave;
		EnemyTypes = enemyTypes;
	}
}
