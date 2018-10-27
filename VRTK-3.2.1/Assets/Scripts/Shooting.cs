using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Shooting : MonoBehaviour {

	public delegate void ShotFired(GameObject enemy, Vector3 hitPoint);
	public static event ShotFired ShotFiredEvent;

    public delegate void GameStart(VRTK.VRTK_ControllerReference CR);
    public static event GameStart GameStartEvent;

    public delegate void GameRestart();
    public static event GameRestart GameRestartEvent;

    public delegate void Credits();
    public static event Credits CreditsEvent;

    public delegate void Back();
    public static event Back BackEvent;

    public VRTK.VRTK_ControllerEvents controllerEvents;
	public VRTK.AdditionalControllerInput extraInput;

	private bool triggerReleased = true;
	private WaitForSeconds shotDuration = new WaitForSeconds (0.07f);

	private LineRenderer laserline;

    [SerializeField]
    private AudioSource buttonSource, missSource;
    
    [SerializeField]
    private FXPlayer missGunPulse;

    public Gun gun;
	float nextFire = 0;
	float fireDelay = 0.5f;

	void Start(){
		laserline = GetComponent<LineRenderer> ();
	}

	// Update is called once per frame
	void Update () {

        laserline.SetPosition (0, gun.transform.position);

        if (controllerEvents.triggerClicked && Time.time > nextFire && triggerReleased) {
			nextFire = Time.time + fireDelay;

			//StartCoroutine (ShotEffect ());

			Vector3 rayOrigin = gun.transform.position;
			RaycastHit hit;

			if (Physics.Raycast (rayOrigin, gun.fireDirection, out hit, gun.range)) {

				GameObject hitObject = hit.collider.gameObject;

                laserline.SetPosition(1, hit.point);

                if (hitObject.CompareTag ("Enemy")) {
					ShotFiredEvent (hitObject, hit.point);

                    // Short light on hit and any normal action
                    HapticPulse(extraInput.TheController, 0.2f);
                }
                else if (hitObject.CompareTag("Start"))
                {
                    GameStartEvent(extraInput.TheController);
                    buttonSource.Play();
                    //hitObject.gameObject.transform.parent.gameObject.SetActive(false);
                    
                    HapticPulse(extraInput.TheController, 0.5f);
                }
                else if (hitObject.CompareTag("Retry"))
                {
                    buttonSource.Play();
                    GameRestartEvent();
                    
                    HapticPulse(extraInput.TheController, 0.5f);
                }
                else if (hitObject.CompareTag("Credits"))
                {
                    buttonSource.Play();
                    CreditsEvent();
                    
                    HapticPulse(extraInput.TheController, 0.5f);
                }
                else if (hitObject.CompareTag("Back"))
                {
                    buttonSource.Play();
                    BackEvent();
                    
                    HapticPulse(extraInput.TheController, 0.5f);
                }
                else if (hitObject.CompareTag("Quit"))
                {
                    buttonSource.Play();
                    
                    HapticPulse(extraInput.TheController, 0.5f);

                    Application.Quit();
                }
                else
                {
                    missGunPulse.PlayFXes();
                    // Short strong on a miss beat
                    HapticPulse(extraInput.TheController, 1.0f);
                    missSource.Play();
                }
            }
            else
            {
                laserline.SetPosition(1, gun.transform.position + gun.range * gun.fireDirection);
                missGunPulse.PlayFXes();
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

    /// <summary>
    /// Send a Single haptic pulse of a given strength to the controller referenced. CopyPasta/Move this method to whichever script you want to call haptic feedback then send the controllerReference from Shooting.cs to it.
    /// </summary>
    /// <param name="controllerReference">Get this from AdditionalControllerInput game object or script.</param>
    /// <param name="strength">A float between 0.0 to 1.0f, higher value = greater strength</param>
    public void HapticPulse(VRTK.VRTK_ControllerReference controllerReference, float strength)
    {
        VRTK.VRTK_ControllerHaptics.TriggerHapticPulse(controllerReference, Mathf.Clamp(strength, 0, 1.0f));
    }
}
