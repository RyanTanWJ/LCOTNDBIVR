using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaveManager : MonoBehaviour {

    [SerializeField]
    private int beatsBetweenWaves;

    [SerializeField]
    private List<GameObject> EnemyTypes;

    private int currentWave = 1;
    private int noWaveCounter = 0;

	private EnemyWave wave;

    private void Start() {
        UpdateWave();
    }

    public EnemyWave GetEnemyWave()
    {
        return wave;
    }

    public bool ReadyForNewWave()
    {
        if (noWaveCounter >= beatsBetweenWaves)
        {
            noWaveCounter = 0;
            return true;
        }
        OnNoWave();
        return false;
    }

    public void CurrentWaveEnded()
    {
        currentWave++;
        Debug.Log("New Wave: " + currentWave);
        UpdateWave();
        OnNoWave();
    }

    private void OnNoWave() {
        noWaveCounter++;
    }

    private void UpdateWave() {
        wave = new EnemyWave();
        PopulateEnemyTypes();
        wave.SetEnemiesInWave(NewNumberEnemies(wave.EnemyTypes.Count));
        wave.GenerateNewWave();
    }

    //Binary Wave Creator
    // Split wave number into binary, and based on these select which enemy types will spawn during the wave
    private void PopulateEnemyTypes()
    {
        int i = currentWave;
        int j = 0;
        while (i > 0)
        {
            if (i % 2 == 1)
            {
                if (j < EnemyTypes.Count)
                {
                    wave.EnemyTypes.Add(EnemyTypes[j]);
                }
                else
                {
                    break;
                }
            }

            i /= 2;
            j++;
        }
    }

    private int NewNumberEnemies(int numEnemyTypes)
    {
        return numEnemyTypes * 3 - numEnemyTypes;
    }
}
