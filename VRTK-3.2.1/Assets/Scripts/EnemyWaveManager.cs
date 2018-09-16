using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaveManager : MonoBehaviour {

	private int currWave, totalWaves, enemiesLeft;

	[SerializeField]
	List<int> wave0Types;
	[SerializeField]
	List<int> wave1Types;
	[SerializeField]
	List<int> wave2Types;
	[SerializeField]
	List<int> wave3Types;
	[SerializeField]
	List<int> wave4Types;

	[SerializeField]
	private List<EnemyWave> enemyWaves;

	void Start(){
		int currWave = 0;
		int totalWaves = 5;

		int wave0Enemies = 5;
		EnemyWave wave0 = new EnemyWave (wave0Enemies, wave0Types);

		int wave1Enemies = 5;
		EnemyWave wave1 = new EnemyWave (wave1Enemies, wave1Types);

		int wave2Enemies = 10;
		EnemyWave wave2 = new EnemyWave (wave2Enemies, wave2Types);

		int wave3Enemies = 5;
		EnemyWave wave3 = new EnemyWave (wave3Enemies, wave3Types);

		int wave4Enemies = 20;
		EnemyWave wave4 = new EnemyWave (wave4Enemies, wave4Types);

		enemyWaves.Add (wave0);
		enemyWaves.Add (wave1);
		enemyWaves.Add (wave2);
		enemyWaves.Add (wave3);
		enemyWaves.Add (wave4);
	}

	public bool NextWave(){
		if (currWave < totalWaves) {
			currWave += 1;
			return true;
		}
		return false;
	}

	public List<int> GetCurrentWaveEnemyTypes(){
		return enemyWaves [currWave].EnemyTypes;
	}

	public int GetCurrentWaveEnemiesLeft(){
		return enemyWaves [currWave].EnemiesLeft;
	}
}
