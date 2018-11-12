using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowTextureController : MonoBehaviour {

    [SerializeField]
    private int onThreshold;

    public Material materialOn;
    public Material materialOff;

    private Player player;
    private Renderer renderMaterial;

	private RhythmController rhythmController;
	private float rhythmState = 0;

    private void OnEnable() {
        RhythmController.BeatTriggeredEvent += UpdatePattern;
        GameStart();
    }

    private void OnDisable() {
        RhythmController.BeatTriggeredEvent -= UpdatePattern;
    }

    private void GameStart()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        renderMaterial = GetComponent<Renderer>();
        rhythmController = GameObject.FindGameObjectWithTag("RhythmController").GetComponent<RhythmController>();
    }

    private void Update()
    {
        if (rhythmController != null)
        {
            rhythmState = rhythmController.GetCurrentBeat();
            renderMaterial.material.color = beatToColor(rhythmState);
            return;
        }
	}

	private void UpdatePattern() {
        if (player.Health < onThreshold) {
            renderMaterial.material = materialOff;
        } else {
			renderMaterial.material = materialOn;
        }
    }

	private Color beatToColor(float beat) {
		//Base Color is (0.5, 0.5, 0.5)
		//Scales to/from (1,1,1) between 0.9-1 / 0-0.1
		float temp = 0.5f;
		if (beat <= 0.9 || beat >= 0.1) {
			return new Color(0.5f, 0.5f, 0.5f);
		} else if (beat > 0.9f) {
			temp += ((beat - 0.9f) * 5f);
		} else {
			temp += (0.5f - (beat * 5f));
		}

		return new Color(temp, temp, temp);
	}

}
