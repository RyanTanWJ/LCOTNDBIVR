using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWave {
    
	public List<GameObject> EnemyTypes;
	private int EnemiesInWave;
    public Queue<GameObject> EnemiesInOrder;

	public EnemyWave(){
		EnemyTypes = new List<GameObject> ();
		EnemiesInWave = 0;
        EnemiesInOrder = new Queue<GameObject>();
	}

    public void GenerateNewWave()
    {
        for (int i=0; i<EnemiesInWave; i++)
        {
            EnemiesInOrder.Enqueue(EnemyTypes[Random.Range(0, EnemyTypes.Count)]);
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
