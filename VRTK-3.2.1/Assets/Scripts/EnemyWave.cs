using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWave {

	public List<int> ActiveSectors;
	public List<int> EnemyTypes;
	public int EnemiesInWave;

	public EnemyWave(){
		ActiveSectors = new List<int> ();
		EnemyTypes = new List<int> ();
		EnemiesInWave = 10;
	}
}
