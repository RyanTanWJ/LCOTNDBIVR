using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour {

	public delegate void ShotFired(GameObject enemy);
	public static event ShotFired ShotFiredEvent;
	public VRTK.VRTK_ControllerEvents controllerEvents;
	public VRTK.AdditionalControllerInput extraInput;

	private bool triggerReleased = true;
	private WaitForSeconds shotDuration = new WaitForSeconds (0.07f);

	private LineRenderer laserline;

	public Gun gun;
	float nextFire = 0;
	float fireDelay = 1.5f;

	void Start(){
		laserline = GetComponent<LineRenderer> ();
	}

	// Update is called once per frame
	void Update () {
		laserline.SetPosition (0, gun.transform.position);
		if (controllerEvents.triggerPressed && Time.time > nextFire && triggerReleased) {

			nextFire = Time.time + fireDelay;

			//StartCoroutine (ShotEffect ());

			Vector3 rayOrigin = gun.transform.position;
			RaycastHit hit;

			bool test = Physics.Raycast (rayOrigin, gun.fireDirection, out hit, gun.range);

			if (test) {
				laserline.SetPosition (1, hit.point);
				GameObject hitObject = hit.collider.gameObject;
				if (hitObject.CompareTag ("Enemy")) {
					ShotFiredEvent (hitObject);
				}
			}

			triggerReleased = false;
		}
		if (!extraInput.TriggerState && !triggerReleased) {
			triggerReleased = true;
		}
		laserline.SetPosition (1, gun.transform.position + gun.range * gun.fireDirection);
	}

	private IEnumerator ShotEffect(){
		laserline.enabled = true;
		yield return shotDuration;
		laserline.enabled = false;
	}
}
