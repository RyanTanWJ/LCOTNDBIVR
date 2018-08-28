using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {

	public Vector3 fireDirection;
	public float range;

	void Update () {
		fireDirection = transform.right;
	}
}
