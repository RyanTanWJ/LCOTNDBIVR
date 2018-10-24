using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmController : MonoBehaviour {

    public delegate void BeatTriggered();
    public static event BeatTriggered BeatTriggeredEvent;

    [SerializeField]
    private int beatsPerMinute;

    [SerializeField]
    private float beatOffset;

    private float secondsPerBeat;
    private float dpsStartTime;
    private float dpsCurrentTime;
    private float beatPosition;

    private int integerBeatPosition = 0;

    private AudioSource musicTrack;

	void Start () {
        musicTrack = GetComponent<AudioSource>();

        secondsPerBeat = 60.0f / (float) beatsPerMinute;
        dpsStartTime = (float)AudioSettings.dspTime - beatOffset;

        Debug.Log(secondsPerBeat + "s per Beat");

        if (musicTrack == null) {
            Debug.LogError("No AudioSource Detected");
        } else {
            musicTrack.Play();
        }
	}

    void Update() {
        dpsCurrentTime = (float) AudioSettings.dspTime - dpsStartTime;
        beatPosition = dpsCurrentTime / secondsPerBeat;

        if ((int) beatPosition > integerBeatPosition) {
            integerBeatPosition = (int)beatPosition;
            TriggerBeat();
        }
    }

    private void TriggerBeat() {
        BeatTriggeredEvent();       //Seems to be the starting event for all the beat based behavior
    }

    public float GetCurrentBeat() {
        return beatPosition;
    }

	public float GetSecondsPerBeat(){
		return secondsPerBeat;
	}

    public int Beat
    {
        get { return integerBeatPosition; }
    }
}
