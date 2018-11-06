using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

	public delegate void PlayerHurt(int damage);
	public static event PlayerHurt PlayerHurtEvent;

    [SerializeField]
    private int columnCount, rowCount, sectors;

    [SerializeField]
    private float arenaIncline, tileSpacing;
    private float inclineAngleRad, tileAngleRad;

    [SerializeField]
    private float arenaRadius, arenaHeight;

    private bool[,] enemyGrid;
    private Transform enemyHolder;

    [SerializeField]
    private List<int> startingSectors;
	private List<int> validColumns;

    private int enemyLimit = 14;

	private EnemyWaveManager enemyWaveManager;
    private EnemyWave enemyWave = null;
    private MovementPattern patterns;

    private int spawnAttempts = 0;
    private int maxSpawnAttempts = 3;

    private int currBeat = 0;

    //My modifications 
    private int waveCount = 0;
    private int endPhase1 = 6;
    private int endPhase2 = 12;
    private int endPhase3 = 30;
    private int beatCount = 0;
    private float saturation = 0.0f;
    private float averageRowSaturation = 0.0f;
    private int sectorSize;
    //-------------------


    [SerializeField]
    AudioSource[] deathAudio;

    [SerializeField]
    AudioSource spawnAudio, hitPlayerAudio;

    void Start() {
        enemyGrid = new bool[columnCount, rowCount];
		enemyHolder = new GameObject("Enemies").transform;

		enemyWaveManager = this.GetComponent<EnemyWaveManager>();
        patterns = this.GetComponent<MovementPattern>();

		validColumns = new List<int> (); //List of index of columns on which enemies can spawn and move
        //The grid is divided in sectors next to each other
        //The activeSectors (here startingSectors) are the index of the current active sectors (ex: sector 1, 2 and 4)
        UpdateValidColumns(sectors, startingSectors); 

        inclineAngleRad = arenaIncline * Mathf.Deg2Rad;
        tileAngleRad = 2 * Mathf.PI / columnCount;
        sectorSize = columnCount / sectors;
    }


    private void OnSpawnCommand2()
    {
        Debug.Log(enemyWave);
        //(we don't have a wave OR we spawned all enemies in wave) AND the arena is clear of enemies
        if ((enemyWave == null || enemyWave.enemyRows.Count == 0) && enemyHolder.childCount <= 0)
        {
            Debug.Log("If statement entered");
            resetGrid();
            enemyWave = enemyWaveManager.GenerateNewWave(sectorSize);
            waveCount += 1;
        }
        else
        {
            Debug.Log("Else statement entered");
            //check if spawning line if free
            bool isSpawningLineOccupied = false;
            for (int j = 0; j < sectorSize; j++)
            {
                isSpawningLineOccupied = isSpawningLineOccupied || enemyGrid[validColumns[j], rowCount - 1];
            }
            //If enough "room" for spawning next row then spawn
            if((saturation + countSaturation(enemyWave.enemyRows[0]) <= maxSaturation(waveCount) || saturation <= 0.0f) && !isSpawningLineOccupied)
            {
                //here we retrieve a row to spawn
                GameObject[] enemyRow = enemyWave.enemyRows[0];
                enemyWave.enemyRows.RemoveAt(0);

                int nbOfEnemyInRow = 0;
                for (int i = 0; i<validColumns.Count; i++)
                {
                    int spawnRowPosition = rowCount - 1;
                    if (isTutorial())
                    {
                        spawnRowPosition -= 2;
                    }
                    int spawnColumnPosition = validColumns[i];
                    //Calculate spawn position and rotation
                    Vector3 spawnPosition;
                    Quaternion spawnRotation;
                    CalculatePositionAndRotation(spawnColumnPosition, spawnRowPosition, out spawnPosition, out spawnRotation);

                    //Create enemy and assign attributes
                    Debug.Log(enemyRow.Length);
                    Debug.Log(i);
                    GameObject enemyType = enemyRow[i];
                    Debug.Log("I made it through!");


                    if (enemyType != null)
                    {
                        nbOfEnemyInRow += 1;
                        GameObject newEnemy = Instantiate(enemyType, spawnPosition, spawnRotation, enemyHolder);

                        spawnAudio.Play();

                        EnterGrid(spawnColumnPosition, spawnRowPosition);

                        Enemy newEnemyController = newEnemy.GetComponent<Enemy>();
                        newEnemyController.SetGridLimits(columnCount, rowCount);
                        newEnemyController.SetStartingPosition(spawnColumnPosition, spawnRowPosition);

                        bool randomPattern = false;
                        List<Vector2Int> pattern = patterns.RetrievePattern(newEnemyController.movementPattern, out randomPattern);
                        newEnemyController.UpdateNextPosition(pattern, currBeat, randomPattern);
                    }
                }
                saturation = saturation + countSaturation(enemyRow);
                averageRowSaturation = countSaturation(enemyRow) / nbOfEnemyInRow;
            }
            else
            {
                //wait a bit longer
                saturation -= averageRowSaturation;
            }

            
        }
    }

    /*


        // Called every beat by the game manager
        private void OnSpawnCommand()
    {
        //TODO get a new policy for spawning that enable high density of enemies

        //If no enemy wave, then try to get from EWM
        if (enemyWave == null)
        {
            if (enemyWaveManager.ReadyForNewWave())
            {
                enemyWave = enemyWaveManager.GetEnemyWave();
            }
            return;
        }

        //No more enemies to spawn
        if (enemyWave.EnemiesInOrder.Count == 0)
        {
            //if there are still enemies, don't let EMW start counting yet
            if (enemyHolder.childCount > 0)
            {
                return;
            }
            enemyWaveManager.CurrentWaveEnded();
            resetGrid();
            enemyWave = null;
            return;
        }

        if (!SpawnEnemyRNG())
        {
            return;
        }

        //Spawn on the outermost row
        int spawnRowPosition = rowCount - 1;
        
        if (isTutorial())
        {
            spawnRowPosition -= 2;
        }

        //Spawn only in valid columns
        int spawnColumnPosition = validColumns[Random.Range (0, validColumns.Count-1)];

        //Verify valid spawn position
        while (enemyGrid[spawnColumnPosition, spawnRowPosition]) {      //trying to spawn on a random column
            spawnColumnPosition = validColumns[Random.Range(0, validColumns.Count - 1)];

            if (spawnAttempts >= maxSpawnAttempts) {
                //breaking causes enemies to spawn on top another enemy
                return;
            } else {
                spawnAttempts++;
            }
        }

        //Calculate spawn position and rotation
        Vector3 spawnPosition;
        Quaternion spawnRotation;
        CalculatePositionAndRotation(spawnColumnPosition, spawnRowPosition, out spawnPosition, out spawnRotation);

        //Create enemy and assign attributes
		GameObject enemyType = enemyWave.EnemiesInOrder.Dequeue();

		if (enemyType != null) {
			GameObject newEnemy = Instantiate(enemyType, spawnPosition, spawnRotation, enemyHolder);

            spawnAudio.Play();

			EnterGrid(spawnColumnPosition, spawnRowPosition);

			Enemy newEnemyController = newEnemy.GetComponent<Enemy>();
			newEnemyController.SetGridLimits(columnCount, rowCount);
			newEnemyController.SetStartingPosition(spawnColumnPosition, spawnRowPosition);

            bool randomPattern = false;
            List<Vector2Int> pattern = patterns.RetrievePattern(newEnemyController.movementPattern, out randomPattern);
			newEnemyController.UpdateNextPosition(pattern, currBeat, randomPattern);
		}

    }


        */

    // Called by the game manager on every beat
    private void OnMoveCommand() {                          
        // Interate through all children
        for (int i = 0; i < enemyHolder.childCount; i++) {
            // Get Enemy Component
            Enemy enemy = enemyHolder.GetChild(i).GetComponent<Enemy>();

            if (enemy != null) {
                Vector2 enemyCurrentPosition = enemy.GetCurrentPosition();  // Get the current position of the enemy on the grid
                Vector2 enemyNextPosition = enemy.GetNextPosition();        // Get the next enemy position on the grid

				if (enemyNextPosition[1] <= 0) {
                    //Enemy has reached the player
                    LeaveGrid((int)enemyCurrentPosition[0], (int)enemyCurrentPosition[1]);

                    //TODO: Hurt the player
					PlayerHurtEvent(1); //Replace with Enemy's damage
                    hitPlayerAudio.Play();
                    
                    //TODO could turn barrier red on hurt , to make it more meaningful

                    Destroy(enemy.gameObject);

                } else if (!enemyGrid[(int)enemyNextPosition[0], (int)enemyNextPosition[1]]) { //next position is empty
                    //Valid movement tile
                    LeaveGrid((int)enemyCurrentPosition[0], (int)enemyCurrentPosition[1]);

                    //Calculate new positions
                    Vector3 newPosition;
                    Quaternion newRotation;
                    CalculatePositionAndRotation((int)enemyNextPosition[0], (int)enemyNextPosition[1], out newPosition, out newRotation);

                    //Update values
                    bool randomPattern = false;
                    List<Vector2Int> pattern = patterns.RetrievePattern(enemy.movementPattern, out randomPattern);
                    enemy.MoveAndUpdateNextPosition(newPosition, newRotation, pattern, currBeat, randomPattern);
                    EnterGrid((int)enemyNextPosition[0], (int)enemyNextPosition[1]);

                } else {
                    //Invalid movement tile
                    Debug.Log(enemy.name + " cannot move.");
                    Debug.Log(enemyCurrentPosition);
                    Debug.Log(enemyNextPosition);
                    bool randomPattern = false;
                    List<Vector2Int> pattern = patterns.RetrievePattern(enemy.movementPattern, out randomPattern);
                    enemy.UpdateNextPosition(pattern, currBeat, randomPattern);
                }
            }
        }
    }

    private void OnDestroyCommand(GameObject enemyObject) {
        Enemy enemy = enemyObject.GetComponent<Enemy>();

        if (enemy != null) {
            Vector2 enemyCurrentPosition = enemy.GetCurrentPosition();
            LeaveGrid((int)enemyCurrentPosition[0], (int)enemyCurrentPosition[1]);

            deathAudio[Random.Range(0, deathAudio.Length - 1)].Play();

            Destroy(enemyObject);
        }
    }

    public void DestroyAllEnemies() {
        foreach(Transform enemy in enemyHolder) {
            Destroy(enemy.gameObject);
        }
    }

    /**
     * Private API
     **/

    private void LeaveGrid(int x, int y) {
        enemyGrid[x, y] = false;
    }

    private void EnterGrid(int x, int y) {
        enemyGrid[x, y] = true;
    }

    private void CalculatePositionAndRotation(int column, int row, out Vector3 position, out Quaternion rotation) {
        float theta = (2 * Mathf.PI / (float)columnCount) * ((float)column + 0.5f);
        float t = (Mathf.PI / 2) * ((float)(rowCount - row) / (float)rowCount);

        float posX = arenaRadius * Mathf.Cos(t) * Mathf.Cos(theta);
        float posZ = arenaRadius * Mathf.Cos(t) * Mathf.Sin(theta);
        float posY = arenaHeight - ((arenaHeight-0.25f) * Mathf.Sin(t));

        position = new Vector3(posX, posY, posZ);
        rotation = Quaternion.LookRotation(-position);
    }

    /// <summary>
    /// Updates the valid columns where enemies will spawn from.
    /// </summary>
    /// <param name="sectors">Number of Sectors the grid is divided into. Sector count starts from 0.</param>
    /// <param name="activeSectors">A list of all Sectors that are valid on the grid.</param>
    private void UpdateValidColumns(int sectors, List<int> activeSectors)
    {
        validColumns.Clear();

        //Find the number of columns in each sector
        int colPerSect = columnCount / sectors;

        foreach (int sectorNum in activeSectors)
        {
            //Find the column on which the sector begins
            int sectStartCol = colPerSect * sectorNum;

            //Find the column before which the sector ends
            int sectEndCol = (int)Mathf.Min((sectStartCol + colPerSect), columnCount); //Min in case not exactly divisible by 4

            for (int j = sectStartCol; j < sectEndCol; j++)
            {
                validColumns.Add(j);    //here we do a lot of useless stuff as there is 20 sectors for 20 columns ... we are just adding columns 2 to 8 as valid columns ...
            }
        }
    }


    //Here is the function to decide is a new enemy is going to spawn 
    // has chance of spawning of (max(nb of enemies, 6) + 6 ) / 14

    /// <summary>
    /// RNG to check whether to spawn an enemy.
    /// </summary>
    /// <returns>Returns true if an enemy should be spawned, false otherwise.</returns>
    private bool SpawnEnemyRNG()
    {
        float numerator = (float)enemyHolder.childCount;
        if (numerator < 6)
        {
            numerator += 6;
        }
        float spawnChance =  numerator / (float)enemyLimit;
        return Random.Range(0, 1.0f) > spawnChance;
    }

    private void resetGrid()
    {
        for (int i=0; i< rowCount; i++)
        {
            for (int j=0; j< columnCount; j++)
            {
                enemyGrid[j, i] = false;
            }
        }
    }


    /*My new functions*/
    // Give the current staturation max possible in game
    // Max saturation of 5 reached at end of phase 2
    // Saturation max increase is linear
    private float maxSaturation(int n)
    {
        return Mathf.Min((float)n * 3.0f / (float)endPhase2 + 2 , 5.0f); 
    }

    private float countSaturation(GameObject[] currentEnemyRow)
    {
        float saturationCounter = 0.0f;
        for (int i = 0; i< currentEnemyRow.Length; i++)
        {
            if(currentEnemyRow[i] != null)
            {
                saturationCounter += currentEnemyRow[i].GetComponent<Enemy>().speed;
            }
        }
        return saturationCounter;
    }

    /*-----------------------------------------------------------------------------*/


    /**
     * Public API
     **/

    public void UpdateValidColumns(List<int> activeSectors){
		//UpdateValidColumns (sectors, activeSectors);
	}


    
    public void SpawnEnemy() {
        OnSpawnCommand2();
	}
    

    public void MoveEnemy(int beat){
        currBeat = beat;
        OnMoveCommand();
    }

    public void DestroyEnemy(GameObject enemy)
    {
        FXPlayer deathFX = Instantiate(enemy.GetComponent<Enemy>().GetDeathFX(), enemy.transform.position, enemy.transform.rotation, transform).GetComponent<FXPlayer>();

        deathFX.PlayFXes();

        Destroy(deathFX.gameObject, 3.0f);

        OnDestroyCommand(enemy);
    }
    
    public bool isTutorial()
    {
        return enemyWaveManager.isTutorial();
    }

    public int CurrentWave()
    {
        return enemyWaveManager.CurrentWave();
    }
}
