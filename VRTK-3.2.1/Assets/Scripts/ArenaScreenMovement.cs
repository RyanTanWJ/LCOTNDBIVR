using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaScreenMovement : MonoBehaviour {

    [SerializeField]
    GameObject cameraVR;
    float distance = 12;
    float height = 7;

    void Start()
    {
        cameraVR = GameObject.FindWithTag("MainCamera");
    }
	
	// Update is called once per frame
	void LateUpdate () {
        Vector3 cameraFoward = cameraVR.transform.forward.normalized;

        Vector3 newPosition = new Vector3(cameraFoward.x * distance, height, cameraFoward.z * distance);
        Quaternion newRotation = Quaternion.LookRotation(newPosition);

        transform.SetPositionAndRotation(newPosition, newRotation);
    }
}
