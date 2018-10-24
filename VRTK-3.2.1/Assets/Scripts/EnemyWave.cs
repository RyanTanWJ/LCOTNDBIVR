using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWave {
    
	public List<GameObject> EnemyTypes;
	private int EnemiesInWave;
    public Queue<GameObject> EnemiesInOrder;
    
    public float avgSaturation;
    private int[] firstPattern;
    public Queue<GameObject[]> enemyRows;

    public EnemyWave(){
		EnemyTypes = new List<GameObject>();
		EnemiesInWave = 0;
        EnemiesInOrder = new Queue<GameObject>();
        avgSaturation = 0.0f;
	}

    public void GenerateNewWave(int windowLength)
    {
        firstPattern = new int[windowLength];
        int patternType = Random.Range(0, 6);
        if(patternType < 3)
        {
            for(int i = 0; i < windowLength / 2; i++)
            {
                firstPattern[i] = Random.Range(0, 2);
                firstPattern[windowLength - i - 1] = firstPattern[i];
            }
            firstPattern[windowLength/2 + 1] = Random.Range(0, 2);
        }
        else
        {
            for (int i = 0; i < windowLength; i++)
            {
                firstPattern[i] = Random.Range(0, 2);
            }
        }



        for (int i=0; i<EnemiesInWave; i++)
        {
            //EnemiesInOrder.Enqueue(EnemyTypes[Random.Range(0, EnemyTypes.Count)]);

        }
    }

    public void SetEnemiesInWave(int num)
    {
        EnemiesInWave = num;
    }

    public bool WaveEmpty()
    {
        return EnemiesInOrder.Count==0;
    }
}
