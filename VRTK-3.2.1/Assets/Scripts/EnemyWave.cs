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

    public EnemyWave(int nbOfEnemiesInWave){
		EnemyTypes = new List<GameObject>();
		EnemiesInWave = nbOfEnemiesInWave;
        EnemiesInOrder = new Queue<GameObject>();
        avgSaturation = 0.0f;
	}

    public void GenerateNewWave(int windowLength)
    {
        int nbOfEnemies = 0;
        List<int[]> enemyRows = new List<int[]>();

        int patternType = Random.Range(0, 6);
        if(patternType < 3)
        {
            firstPattern = createSymetricPattern(windowLength);
        }
        else
        {
            firstPattern = new int[windowLength];
            for (int i = 0; i < windowLength; i++)
            {
                firstPattern[i] = Random.Range(0, 2);
            }
        }

        enemyRows.Add(firstPattern);
        nbOfEnemies = CountNbOfEnemies(firstPattern);
        while (nbOfEnemies < EnemiesInWave)
        {
            int[] nextPattern = new int[windowLength];
            if(patternType == 0)                        //New symetric pattern
            {
                nextPattern = createSymetricPattern(windowLength);
            }
            if(patternType == 1 || patternType == 3)    //Repeat pattern
            {
                System.Array.Copy(enemyRows[enemyRows.Count - 1], nextPattern, windowLength);
            }
            if(patternType == 2 || patternType == 4)    //Complement pattern
            {
                nextPattern = complementPattern(enemyRows[enemyRows.Count - 1]);
            }
            if (patternType == 5)
            {
                nextPattern = shiftPattern(enemyRows[enemyRows.Count - 1]);
            }
            nbOfEnemies += CountNbOfEnemies(nextPattern);
            enemyRows.Add(nextPattern);
        }
        // Now we have a set of patterns with at least the minimum amount of enemies required
        // TODO populate those pattern with enemies trying to balance slow and fast enemies

    }

    public bool WaveEmpty()
    {
        return EnemiesInOrder.Count==0;
    }

    //############################# All the utilities to be used in the creation of patterns #########################

    // Simple utility function to count the number of enemy in a row pattern
    private int CountNbOfEnemies(int[] enemyRowPattern)
    {
        int count = 0;
        for(int i = 0; i< enemyRowPattern.Length; i++)
        {
            count += enemyRowPattern[i];
        }
        return count;
    }

    // Create a pattern of spawn symetric around the central position of window
    private int[] createSymetricPattern(int windowLength)
    {
        firstPattern = new int[windowLength];
        for (int i = 0; i < windowLength / 2; i++)
        {
            firstPattern[i] = Random.Range(0, 2);
            firstPattern[windowLength - i - 1] = firstPattern[i];
        }
        if(windowLength % 2 == 1)
        {
            firstPattern[windowLength / 2 + 1] = Random.Range(0, 2);
        }
        return firstPattern;
    }

    //Gives the complement of the pattern 
    private int[] complementPattern(int[] pattern)
    {
        int[] complement = new int[pattern.Length];
        for(int i=0; i < pattern.Length; i++)
        {
            complement[i] = (pattern[i] + 1) % 2;
        }
        return complement;
    }

    //Shift pattern to left
    private int[] shiftPattern(int[] pattern)
    {
        int[] complement = new int[pattern.Length];
        for (int i = 0; i < pattern.Length; i++)
        {
            complement[i] = pattern[(i + 1)% pattern.Length];
        }
        return complement;
    }

}
