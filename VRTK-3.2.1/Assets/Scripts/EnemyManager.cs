using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

    [SerializeField]
    private GameObject enemyPrefab;

    [SerializeField]
    private int columnCount, rowCount;

    [SerializeField]
    private float arenaIncline, tileSpacing;
    private float inclineAngleRad, tileAngleRad;

    private bool[,] enemyGrid;
    private Transform enemyHolder;

    private int spawnAttempts = 0;
    private int maxSpawnAttempts = 3;

    void Start() {
        enemyGrid = new bool[columnCount, rowCount];
        enemyHolder = new GameObject("Enemies").transform;

        inclineAngleRad = arenaIncline * Mathf.Deg2Rad;
        tileAngleRad = 2 * Mathf.PI / columnCount;
    }

    private void OnSpawnCommand() {
        //Spawn on the outermost row
        int spawnRowPosition = rowCount - 1;
        int spawnColumnPosition = Random.Range(0, columnCount);

        //Verify valid spawn position
        while (enemyGrid[spawnColumnPosition, spawnRowPosition]) {
            spawnColumnPosition = Random.Range(0, columnCount);

            if (spawnAttempts >= maxSpawnAttempts) {
                break;
            } else {
                spawnAttempts++;
            }
        }

        //Calculate spawn position and rotation
        Vector3 spawnPosition;
        Quaternion spawnRotation;
        CalculatePositionAndRotation(spawnColumnPosition, spawnRowPosition, out spawnPosition, out spawnRotation);

        //Create enemy and assign attributes
        GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, spawnRotation, enemyHolder);
        EnterGrid(spawnColumnPosition, spawnRowPosition);

        //TODO: Assign Health <- Might use a common health/damage system for player & enemy
        newEnemy.GetComponent<Enemy>().SetGridLimits(columnCount, rowCount);

        //TODO: Assign Movement Cycles - List of Vector2 of relative movements
        // Currently hardcoded in Enemy.cs
    }

    private void OnMoveCommand() {
        // Interate through all children
        for (int i = 0; i < enemyHolder.childCount; i++) {
            // Get Enemy Component
            Enemy enemy = enemyHolder.GetChild(i).GetComponent<Enemy>();

            if (enemy != null) {
                Vector2 enemyCurrentPosition = enemy.GetCurrentPosition();
                Vector2 enemyNextPosition = enemy.GetNextPosition();

                if (enemyNextPosition[1] <= 0) {
                    //Enemy has reached the player
                    LeaveGrid((int)enemyCurrentPosition[0], (int)enemyCurrentPosition[1]);

                    //TODO: Hurt the player

                    //Destroy(enemy.gameObject);

                } else if (enemyGrid[(int)enemyNextPosition[0], (int)enemyNextPosition[1]] == false) {
                    //Valid movement tile
                    LeaveGrid((int)enemyCurrentPosition[0], (int)enemyCurrentPosition[1]);

                    //Calculate new positions
                    Vector3 newPosition;
                    Quaternion newRotation;
                    CalculatePositionAndRotation((int)enemyNextPosition[0], (int)enemyNextPosition[1], out newPosition, out newRotation);
                    
                    //Update values
                    enemy.MoveAndUpdateNextPosition(newPosition, newRotation);
                    EnterGrid((int)enemyNextPosition[0], (int)enemyNextPosition[1]);

                } else {
                    //Invalid movement tile
                    enemy.UpdateNextPosition();
                }
            }
        }
    }

    private void OnDestroyCommand(GameObject enemyObject) {
        Enemy enemy = enemyObject.GetComponent<Enemy>();

        if (enemy != null) {
            Vector2 enemyCurrentPosition = enemy.GetCurrentPosition();
            LeaveGrid((int)enemyCurrentPosition[0], (int)enemyCurrentPosition[1]);

            Destroy(enemyObject);
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
        float angle = tileAngleRad * (float)column;
        float radius = tileSpacing * (float)(row + 1);

        float posX = radius * Mathf.Cos(angle);
        float posZ = radius * Mathf.Sin(angle);
        float posY = radius * Mathf.Tan(inclineAngleRad);

        position = new Vector3(posX, posY, posZ);
        rotation = Quaternion.LookRotation(-position);
    }

    /**
     * Public API
     **/

    public void SpawnEnemy() {
        OnSpawnCommand();
    }

    public void MoveEnemy(){
        OnMoveCommand();
    }

    public void DestroyEnemy(GameObject enemy)
    {
        OnDestroyCommand(enemy);
    }

}
