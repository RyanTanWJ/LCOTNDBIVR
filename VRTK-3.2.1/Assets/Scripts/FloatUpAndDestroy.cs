using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatUpAndDestroy : MonoBehaviour {

    float lifespan = 1;
    float speed = 0.01f;

	// Use this for initialization
	void Start () {
        Destroy(this.gameObject, lifespan);
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = transform.position + (transform.up * speed);
	}
}
