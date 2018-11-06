using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWave {
    
	public List<GameObject> EnemyTypes;
	private int EnemiesInWave;
    public Queue<GameObject> EnemiesInOrder;
    
    public float avgSaturation;
    private GameObject[] firstPattern;
    public List<GameObject[]> enemyRows;
    //public float[] enemySpeed;    //stores the value of speed for each enemy

    public EnemyWave(int nbOfEnemiesInWave){
		EnemyTypes = new List<GameObject>();
		EnemiesInWave = nbOfEnemiesInWave;
        EnemiesInOrder = new Queue<GameObject>();
        avgSaturation = 0.0f;
        enemyRows = new List<GameObject[]>();
    }

    public void GenerateNewWave(int windowLength)
    {
        int nbOfEnemies = 0;                        //Count the number of enemies that we are spawning
        //MOMENT OF RANDOMNESS
        //int patternType = Random.Range(0, 6);       //Pick which type of pattern it will be
        int patternType = Random.Range(3, 6);       //Debug Code
        if (patternType < 3)
        {
            Debug.Log("I am here");
            firstPattern = createSymetricPattern(windowLength);
        }
        else
        {
            Debug.Log("Am I here");
            firstPattern = createOrdinaryPattern(windowLength);
        }

        enemyRows.Add(firstPattern);
        nbOfEnemies = CountNbOfEnemies(firstPattern);
        while (nbOfEnemies < EnemiesInWave)
        {
            GameObject[] nextPattern = new GameObject[windowLength];
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
    }

    public bool WaveEmpty()
    {
        return EnemiesInOrder.Count==0;
    }

    //############################# All the utilities to be used in the creation of patterns #########################

    // Simple utility function to count the number of enemy in a row pattern
    private int CountNbOfEnemies(GameObject[] enemyRowPattern)
    {
        int count = 0;
        for(int i = 0; i< enemyRowPattern.Length; i++)
        {
            if(enemyRowPattern[i] != null)
            {
                count += 1;
            }
        }
        return count;
    }

    // Create a random pattern of enemies
    private GameObject[] createOrdinaryPattern(int windowLength)
    {
        firstPattern = new GameObject[windowLength];
        for (int i = 0; i < windowLength; i++)
        {
            if (Random.Range(0, 2) == 1) //0.5 probability to put an enemy there
            {
                if (i != 0)
                {
                    if (firstPattern[i - 1] != null) //cluster enemies of the same type together
                    {
                        firstPattern[i] = firstPattern[i - 1];
                    }
                    else
                    {
                        firstPattern[i] = EnemyTypes[Random.Range(0, EnemyTypes.Count)];
                    }
                }
                else
                {   //here i = 0 first enemy of the row
                    firstPattern[i] = EnemyTypes[Random.Range(0, EnemyTypes.Count)];
                }
            }
            else
            {
                firstPattern[i] = null;
            }
        }
        return firstPattern;
    }

        // Create a pattern of spawn symetric around the central position of window
        private GameObject[] createSymetricPattern(int windowLength)
    {
        firstPattern = new GameObject[windowLength];
        for (int i = 0; i < windowLength / 2; i++)
        {
            if(Random.Range(0, 2) == 1) //0.5 probability to put an enemy there
            {
                if (i != 0)
                {
                    if(firstPattern[i-1] != null) //cluster enemies of the same type together
                    {
                        firstPattern[i] = firstPattern[i - 1];
                        firstPattern[windowLength - i - 1] = firstPattern[i];
                    }
                    else
                    {
                        firstPattern[i] = EnemyTypes[Random.Range(0, EnemyTypes.Count)];
                        firstPattern[windowLength - i - 1] = firstPattern[i];
                    }
                }
                else
                {   //here i = 0 first enemy of the row
                    firstPattern[i] = EnemyTypes[Random.Range(0, EnemyTypes.Count)];
                    firstPattern[windowLength - i - 1] = firstPattern[i];
                }
            }
            else
            {
                firstPattern[i] = null;
                firstPattern[windowLength - i - 1] = firstPattern[i];
            }
        }
        if(windowLength % 2 == 1)
        {
            if (Random.Range(0, 2) == 1){
                if (windowLength != 1)
                {
                    if (firstPattern[windowLength / 2] != null)
                    {       //enemy on the left position compared to center position is not null so get the same 
                        firstPattern[windowLength / 2 + 1] = firstPattern[windowLength / 2];
                    }
                    else
                    {       //enemy on the left of the center position is null so center not part of a vluster just spawn a new random enemy
                        firstPattern[windowLength / 2 + 1] = EnemyTypes[Random.Range(0, EnemyTypes.Count)];
                    }
                }
                else
                {   //Windows size is 1 so just spawn one random enemy
                    Debug.Log("winLen = " + windowLength);
                    Debug.Log("calc = " + (windowLength / 2 + 1));
                    Debug.Log("1st Pattern Len = " + firstPattern.Length);
                    Debug.Log("EnemyTypes.Count = " + EnemyTypes.Count);
                    firstPattern[windowLength / 2 + 1] = EnemyTypes[Random.Range(0, EnemyTypes.Count)];
                }
            }
            else
            {
                Debug.Log("winLen = " + windowLength);
                Debug.Log("calc = " + (windowLength / 2 + 1));
                Debug.Log("1st Pattern Len = " + firstPattern.Length);
                firstPattern[windowLength / 2 + 1] = null;
            }
        }
        return firstPattern;
    }

    //Gives the complement of the pattern 
    private GameObject[] complementPattern(GameObject[] pattern)
    {
        GameObject[] complement = new GameObject[pattern.Length];
        for (int i = 0; i < pattern.Length; i++)
        {
            if (pattern[i] == null)
            {                       //equivalent to say that we need to spawn an enemy here
                if (i != 0)
                {
                    if (complement[i - 1] != null) //cluster enemies of the same type together
                    {
                        complement[i] = complement[i - 1];
                    }
                    else
                    {
                        complement[i] = EnemyTypes[Random.Range(0, EnemyTypes.Count)];
                    }
                }
                else
                {   //here i = 0 first enemy of the row
                    complement[i] = EnemyTypes[Random.Range(0, EnemyTypes.Count)];
                }
            }
            else
            {
                complement[i] = null;
            }
        }
        return complement;
    }

    //Shift pattern to left
    private GameObject[] shiftPattern(GameObject[] pattern)
    {
        GameObject[] complement = new GameObject[pattern.Length];
        for (int i = 0; i < pattern.Length; i++)
        {
            complement[i] = pattern[(i + 1)% pattern.Length];
        }
        return complement;
    }

}
