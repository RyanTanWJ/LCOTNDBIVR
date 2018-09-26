using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour {

	public delegate void ShotFired(GameObject enemy, Vector3 hitPoint);
	public static event ShotFired ShotFiredEvent;

    public delegate void GameStart();
    public static event GameStart GameStartEvent;

    public delegate void GameRestart();
    public static event GameRestart GameRestartEvent;

    public VRTK.VRTK_ControllerEvents controllerEvents;
	public VRTK.AdditionalControllerInput extraInput;

	private bool triggerReleased = true;
	private WaitForSeconds shotDuration = new WaitForSeconds (0.07f);

	private LineRenderer laserline;
    private AudioSource buttonSource;

    [SerializeField]
    private GameObject dot;
    [SerializeField]
    private GunPulse gunPulse;

    public Gun gun;
	float nextFire = 0;
	float fireDelay = 0.5f;

	void Start(){
		laserline = GetComponent<LineRenderer> ();
        buttonSource = GetComponent<AudioSource>();
	}

	// Update is called once per frame
	void Update () {
        RaycastHit dotHit;
        if (Physics.Raycast(gun.transform.position, gun.fireDirection, out dotHit, gun.range))
        {
            dot.SetActive(true);
            dot.transform.position = dotHit.point;
        }
        else
        {
            dot.SetActive(false);
        }

        laserline.SetPosition (0, gun.transform.position);
        laserline.SetPosition(1, gun.transform.position + 0.5f * gun.fireDirection);

        if (controllerEvents.triggerClicked && Time.time > nextFire && triggerReleased) {
			nextFire = Time.time + fireDelay;

            gunPulse.PlayPulse();

			//StartCoroutine (ShotEffect ());

			Vector3 rayOrigin = gun.transform.position;
			RaycastHit hit;

			if (Physics.Raycast (rayOrigin, gun.fireDirection, out hit, gun.range)) {
				GameObject hitObject = hit.collider.gameObject;
				if (hitObject.CompareTag ("Enemy")) {
					ShotFiredEvent (hitObject, hit.point);
				}
                else if (hitObject.CompareTag("Start"))
                {
                    GameStartEvent();
                    buttonSource.Play();
                    //hitObject.gameObject.transform.parent.gameObject.SetActive(false);
                }
                else if (hitObject.CompareTag("Retry"))
                {
                    buttonSource.Play();
                    GameRestartEvent();

                }
            }

			triggerReleased = false;
		}
		if (!extraInput.TriggerState && !triggerReleased) {
			triggerReleased = true;
        }
        
    }

	private IEnumerator ShotEffect(){
		laserline.enabled = true;
		yield return shotDuration;
		laserline.enabled = false;
	}
}
