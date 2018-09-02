using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

    [SerializeField]
    private GameObject enemyPrefab;

    [SerializeField]
    private float columnCount, rowCount, arenaIncline, tileSpacing;

    private int[,] enemyGrid;
    private Transform enemyHolder;

    void Start() {
        enemyGrid = new int[(int)columnCount, (int)rowCount];
        enemyHolder = new GameObject("Enemies").transform;
    }

    private void OnSpawnCommand() {
        //Spawn on the outermost row
        int spawnColumnPosition = (int)columnCount - 1;
        int spawnRowPosition = Random.Range((int)0, (int)rowCount);

        float angle = 2 * Mathf.PI / columnCount;
        float radius = spawnColumnPosition * (spawnColumnPosition + 1);
        float inclineRad = arenaIncline * Mathf.Deg2Rad;

        float spawnX = radius * Mathf.Cos(angle);
        float spawnZ = radius * Mathf.Sin(angle);
        float spawnY = radius * Mathf.Tan(inclineRad);

        Vector3 spawnPosition = new Vector3(spawnX, spawnY, spawnZ);
        Quaternion spawnRotation = Quaternion.LookRotation(-spawnPosition);

        Instantiate(enemyPrefab, spawnPosition, spawnRotation, enemyHolder);
    }

    public void SpawnEnemy() {
        OnSpawnCommand();
    }

}
