using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaveManager : MonoBehaviour
{
    public delegate void TutorialEnd(bool hardMode);
    public static event TutorialEnd TutorialEndEvent;

    [SerializeField]
    private int beatsBetweenWaves;

    [SerializeField]
    private List<GameObject> EnemyTypes;

    private int currentWave = 0;

    private bool hardMode = false;
    private int hardDifficultyMaskMin = 0;
    private int hardDifficultyMaskMax = 0;

    private bool tutorialEnded = false;

    private EnemyWave wave;

    /* ------------- My new stuff --------------------*/
    private List<GameObject> typesAvailable = new List<GameObject>();
    [SerializeField]
    private int nbOfWavesBeforeNewEnemy;
    private int nextNewEnemy = 1;
    [SerializeField]
    private int minNbOfEnemy;       //The minimum nb of enemies per wave
    [SerializeField]
    private float alphaNbEnemy;     //The coefficient of progress of number of enemies with regards to the number of waves
    //[SerializeField]
    //private float[] enemySpeed;    //stores the value of speed for each enemy
    /*------------------------------------------------*/

    private void Start()
    {
        hardDifficultyMaskMin = 3;
        hardDifficultyMaskMax = EnemyTypes.Count / 2 + 1;
    }

    public EnemyWave GetEnemyWave()
    {
        return wave;
    }

    //TODO modify the creation of a enemy wave and the object enemy wave itself
    public EnemyWave GenerateNewWave(int windowSize)
    {     //Here a new wave is created then will be passed to enemy manager by enemy wave manager
        currentWave++;
        Debug.Log("New Wave: " + currentWave);
        if (!tutorialEnded && currentWave == 3)
        {
            currentWave = 1;
            tutorialEnded = true;
            TutorialEndEvent(false);
        }
        wave = new EnemyWave(NewNumberEnemies());
        //wave.enemySpeed = enemySpeed;
        PopulateEnemyTypes();

        wave.GenerateNewWave(windowSize);
        return wave;
    }

    // Add the types of enemies that are available to use in the new wave
    private void PopulateEnemyTypes()
    {
        //For HardMode
        if (hardMode)
        {

            for (int i = hardDifficultyMaskMin; i < hardDifficultyMaskMax; i++)
            {
                wave.EnemyTypes.Add(EnemyTypes[i]);
            }

            if (hardDifficultyMaskMax < EnemyTypes.Count)
            {
                hardDifficultyMaskMax++;
            }
            else if (hardDifficultyMaskMin < (EnemyTypes.Count / 2) - 1)
            {
                hardDifficultyMaskMin++;
            }
            return;
        }

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

    private int NewNumberEnemies()
    {
        return (int)((float)currentWave * alphaNbEnemy) + minNbOfEnemy;
    }

    public bool isTutorial()
    {
        return !tutorialEnded;
    }

    public int CurrentWave()
    {
        return currentWave;
    }

    public void HardMode()
    {
        hardMode = true;
        tutorialEnded = true;
        TutorialEndEvent(true);
    }
}
