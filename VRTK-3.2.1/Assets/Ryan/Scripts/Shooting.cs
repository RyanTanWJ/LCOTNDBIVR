using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour {

	public delegate void ShotFired(GameObject enemy);
	public static event ShotFired ShotFiredEvent;
	public VRTK.VRTK_ControllerEvents controllerEvents;

	public Gun gun;
	float nextFire = 0;
	float fireDelay = 0.5f;

	// Update is called once per frame
	void Update () {
		if (controllerEvents.triggerPressed && Time.time > nextFire) {
			Debug.Log ("FIRE!");

			nextFire = Time.time + fireDelay;

			Vector3 rayOrigin = gun.transform.position;
			RaycastHit hit;

			bool test = Physics.Raycast (rayOrigin, gun.fireDirection, out hit, gun.range);
			Debug.Log (test);
			if (test) {
				GameObject hitObject = hit.collider.gameObject;
				Debug.Log (hitObject.name + " has been hit.");
				if (hitObject.CompareTag("Enemy")) {
					ShotFiredEvent (hitObject);
				}

			}
		}
	}
}
