using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visualiser : MonoBehaviour {

    private int visualiserSamples = 1024;
    private int samplesPerInstance;

    public GameObject visualiser;
    public int visualiserInstances;

    Transform visualiserHolder;

    float[] visualiserValues;

	void Start () {
        visualiserHolder = new GameObject("Visualisation").transform;
        visualiserValues = new float[visualiserSamples];

        Vector3 tempTransform = new Vector3(0, 0, 0);

        for (int i = 0; i < visualiserInstances; i++)
        {
            tempTransform.x = i;
            Instantiate(visualiser, tempTransform, Quaternion.identity, visualiserHolder);
        }

        samplesPerInstance = visualiserSamples / visualiserInstances;
	}
	
	// Update is called once per frame
	void Update () {
        AudioListener.GetOutputData(visualiserValues, 0);

        float yScale;
        Vector3 tempScale = new Vector3(0, 0, 0);

        for (int i = 0; i < visualiserHolder.childCount; i++)
        {
            yScale = 0;

            for (int j = 0; j < samplesPerInstance; j++)
            {
                yScale += visualiserValues[i * samplesPerInstance + j];
            }

            Transform visualiserTransform = visualiserHolder.GetChild(i);

            tempScale.x = visualiserTransform.localScale.x;
            tempScale.y = yScale;
            tempScale.z = visualiserTransform.localScale.z;

            visualiserTransform.localScale = tempScale;
        }
	}
}
