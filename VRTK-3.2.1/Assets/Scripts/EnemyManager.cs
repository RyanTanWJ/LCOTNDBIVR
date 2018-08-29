using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

    [SerializeField]
    private GameObject enemyPrefab;

    [SerializeField]
    private float minX, maxX, spawnY, minZ, maxZ;

    private void OnSpawnCommand() {
        float spawnX = Random.Range(minX, maxX);
        float spawnZ = Random.Range(minZ, maxZ);

        Vector3 spawnPosition = new Vector3(spawnX, spawnY, spawnZ);

        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }

    public void SpawnEnemy() {
        OnSpawnCommand();
    }
}
