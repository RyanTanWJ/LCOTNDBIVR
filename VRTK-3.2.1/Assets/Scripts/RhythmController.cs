using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmController : MonoBehaviour {

    public delegate void BeatTriggered();
    public static event BeatTriggered BeatTriggeredEvent;

    [SerializeField]
    private List<int> beatsPerMinute;

    [SerializeField]
    private List<float> beatOffsets;

    private float secondsPerBeat;
    private float dpsStartTime;
    private float dpsCurrentTime;
    private float beatPosition;

    private int integerBeatPosition = 0;

    [SerializeField]
    private List<AudioSource> musicTracks;

	public void StartGame (bool normal) {
        int accessor = normal ? 0 : 1;
        AudioSource musicTrack = musicTracks[accessor];
        int bpm = beatsPerMinute[accessor];
        float beatOffset = beatOffsets[accessor];
        secondsPerBeat = 60.0f / (float)bpm;
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
