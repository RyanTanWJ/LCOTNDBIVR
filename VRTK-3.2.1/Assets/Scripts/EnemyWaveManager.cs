using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaveManager : MonoBehaviour {

    public GameObject[] Enemies;

    [SerializeField]
    private float enemySpawnChance;

    [SerializeField]
    private int beatsBetweenWaves;

    private int currentWave = 0;
    private int noWaveCounter = 0;

	private EnemyManager enemyManager;

	private EnemyWave wave = new EnemyWave();

    private void Start() {
        UpdateWave();
		enemyManager = this.GetComponent<EnemyManager> ();
    }
		
    public GameObject GetEnemy() {
		if (wave.EnemiesInWave > 0) {
			if (Random.value <= enemySpawnChance) {
				return SelectEnemy ();
			}
		} else {
			OnNoWave ();
		}

		return null;
    }

	//TODO: Customisbable number of enemies in wave
    private void OnNoWave() {
        noWaveCounter++;
		if (noWaveCounter % beatsBetweenWaves == 0) {
            UpdateWave();

			wave.EnemiesInWave = 10;

			UpdateWaveActiveSectors (Random.Range (0, 3));
			// Call Enemy Manager to Update the Valid Columns
			enemyManager.UpdateValidColumns(wave.ActiveSectors);
        }
    }

    private void UpdateWave() {
        currentWave++;
		wave.EnemyTypes.Clear();

        //Binary Wave Creator
        // Split wave number into binary, and based on these select which enemy types will spawn during the wave
        int i = currentWave;
        int j = 0;
        while (i > 0) {
            if (i % 2 == 1) {
                if (j < Enemies.Length) {
					wave.EnemyTypes.Add(j);
                } else {
                    break;
                }
            }

            i /= 2;
            j++;
        }
    }

	/*
	private void UpdateFixedWave(string jsonFilename) {
		currentWave++;
		wave.EnemyTypes.Clear();

		List<int> fixedWave = new List<int> ();
		//Read JSON file, parse and populate fixedWave

		wave.EnemyTypes = fixedWave;
	}
	*/

    private GameObject SelectEnemy() {
		List<int> validEnemies = wave.EnemyTypes;
		wave.EnemiesInWave--;
        return Enemies[validEnemies[Random.Range(0, validEnemies.Count - 1)]];
	}

	private void UpdateWaveActiveSectors(params int[] actSects){
		wave.ActiveSectors.Clear ();
		if (actSects.Length == 0) {
			wave.ActiveSectors.Add (0);
		} else {
			foreach (int sect in actSects) {
				wave.ActiveSectors.Add (sect);
			}
		}
	}
}
