using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaScreenMovement : MonoBehaviour {

    [SerializeField]
    GameObject cameraVR;
    float distance = 15;
    float height = 10;
	
	// Update is called once per frame
	void LateUpdate () {
        Vector3 newPosition = cameraVR.transform.forward.normalized * distance + new Vector3(0, height, 0);
        Quaternion newRotation = Quaternion.LookRotation(-newPosition);

        transform.SetPositionAndRotation(newPosition, newRotation);
    }
}
