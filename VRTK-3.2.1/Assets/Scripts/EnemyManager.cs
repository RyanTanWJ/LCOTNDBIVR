using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

    [SerializeField]
    private GameObject enemyPrefab;

    [SerializeField]
    private float columnCount, rowCount, arenaIncline, tileSpacing;

    private bool[,] enemyGrid;
    private Transform enemyHolder;

    void Start() {
        enemyGrid = new bool[(int)columnCount, (int)rowCount];
        enemyHolder = new GameObject("Enemies").transform;
    }

    private void OnSpawnCommand() {
        //Spawn on the outermost row
        int spawnColumnPosition = (int)columnCount - 1;
        int spawnRowPosition = Random.Range((int)0, (int)rowCount);

        //TODO: Verify valid spawn position

        //Calculate spawn position and rotation
        float angle = 2 * Mathf.PI / columnCount;
        float radius = spawnColumnPosition * (spawnColumnPosition + 1);
        float inclineRad = arenaIncline * Mathf.Deg2Rad;

        float spawnX = radius * Mathf.Cos(angle);
        float spawnZ = radius * Mathf.Sin(angle);
        float spawnY = radius * Mathf.Tan(inclineRad);

        Vector3 spawnPosition = new Vector3(spawnX, spawnY, spawnZ);
        Quaternion spawnRotation = Quaternion.LookRotation(-spawnPosition);

        //Create enemy and assign attributes
        Instantiate(enemyPrefab, spawnPosition, spawnRotation, enemyHolder);
        EnterGrid(spawnColumnPosition, spawnRowPosition);

        //TODO: Abstract position & rotation calculations (Above)

        //TODO: Assign Health <- Might use a common health/damage system for player & enemy
        //TODO: Assign Movement Cycles - List of Vector2 of relative movements
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

                    Destroy(enemy.gameObject);

                } else if (enemyGrid[(int)enemyNextPosition[0], (int)enemyNextPosition[1]] == false) {
                    //Valid movement tile
                    LeaveGrid((int)enemyCurrentPosition[0], (int)enemyCurrentPosition[1]);
                    enemy.MoveAndUpdateNextPosition();
                    EnterGrid((int)enemyNextPosition[0], (int)enemyNextPosition[1]);

                } else {
                    //Invalid movement tile
                    enemy.UpdateNextPosition();
                }
            }
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

    /**
     * Public API
     **/

    public void SpawnEnemy() {
        OnSpawnCommand();
    }

    public void MoveEnemy(){
        OnMoveCommand();
    }

}
