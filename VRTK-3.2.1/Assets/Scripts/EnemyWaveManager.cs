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

    private bool isWave = false;

    private List<int> validEnemies;

    //Possible to get GameManager to manage the listening of the beats instead,
    //so we can start/stop spawning at will.
    private void OnEnable() {
        RhythmController.BeatTriggeredEvent += OnBeat;
    }

    private void OnDisable() {
        RhythmController.BeatTriggeredEvent -= OnBeat;
    }

    private void Start() {
        UpdateWave();
    }

    private void OnBeat() {
        if (isWave) {
            if(Random.value <= enemySpawnChance) {
                // Call Enemy Manager To Spawn Selected Enemy
                //EnemyManager.SpawnEnemy(SelectEnemy());
            }
        } else {
            OnNoWave();
        }
    }

    private void OnNoWave() {
        noWaveCounter++;

		if (noWaveCounter % beatsBetweenWaves == 0) {
            UpdateWave();
            isWave = true;
        }
    }

    private void UpdateWave() {
        currentWave++;
        validEnemies.Clear();

        //Binary Wave Creator
        // Split wave number into binary, and based on these select which enemy types will spawn during the wave
        int i = currentWave;
        int j = 0;
        while (i > 0) {
            if (i % 2 == 1) {
                if (j < Enemies.Length) {
                    int k = j;
                    validEnemies.Add(k);
                } else {
                    break;
                }
            }

            i /= 2;
            j++;
        }
    }

    private GameObject SelectEnemy() {
        return Enemies[validEnemies[Random.Range(0, validEnemies.Count - 1)]];
    }
}
