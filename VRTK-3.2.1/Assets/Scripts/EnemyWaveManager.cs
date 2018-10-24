﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaveManager : MonoBehaviour
{
    public delegate void TutorialEnd();
    public static event TutorialEnd TutorialEndEvent;

    [SerializeField]
    private int beatsBetweenWaves;

    [SerializeField]
    private List<GameObject> EnemyTypes;

    private int currentWave = 0;

    private bool tutorialEnded = false;

	private EnemyWave wave;

    /* ------------- My new stuff --------------------*/
    private List<GameObject> typesAvailable;
    [SerializeField]
    private int nbOfWavesBeforeNewEnemy;
    private int nextNewEnemy = 1;

    /*------------------------------------------------*/

    private void Start() {}

    public EnemyWave GetEnemyWave()
    {
        return wave;
    }

    //TODO modify the creation of a enemy wave and the object enemy wave itself
    private void GenerateNewWave() {     //Here a new wave is created then will be passed to enemy manager by enemy wave manager
        currentWave++;
        Debug.Log("New Wave: " + currentWave);
        if (!tutorialEnded && currentWave == 3)
        {
            currentWave = 1;
            tutorialEnded = true;
            TutorialEndEvent();
        }
        wave = new EnemyWave();
        PopulateEnemyTypes();

        wave.SetEnemiesInWave(NewNumberEnemies(wave.EnemyTypes.Count));
        wave.GenerateNewWave();
    }

    // Add the types of enemies that are available to use in the new wave
    private void PopulateEnemyTypes()
    {
        if (!tutorialEnded)
        {
            //first wave = static enemies only
            //second wave = strafing enemies only
            wave.EnemyTypes.Add(EnemyTypes[currentWave - 1]);         
        }
        // If we reach next threashold to add a new enemy in term of nb of wave and there are still enemies to add
        // then add enemy to the enemy pool
        else
        {
            if (currentWave == nextNewEnemy && typesAvailable.Count < EnemyTypes.Count - 2)
            {
                wave.EnemyTypes.Add(EnemyTypes[typesAvailable.Count + 2]);
                typesAvailable.Add(EnemyTypes[typesAvailable.Count + 2]);
                nextNewEnemy += nbOfWavesBeforeNewEnemy;
            }
            else // just give the whole pool of enemies available to the wave 
            {
                for (int i = 0; i < typesAvailable.Count; i++)
                {
                    wave.EnemyTypes.Add(typesAvailable[i]);
                }
            }
        }
    }

    private int NewNumberEnemies(int numEnemyTypes)
    {
        return numEnemyTypes * 3 + currentWave;
    }

    public bool isTutorial()
    {
        return !tutorialEnded;
    }

    public int CurrentWave()
    {
        return currentWave;
    }
}
