using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureController : MonoBehaviour {

    [SerializeField]
    private int lowThreshold, highThreshold;
 
    public Material[] materialPatternLow;
    public Material[] materialPatternNormal;
    public Material[] materialPatternHigh;

    private int beat = 0;

    private Player player;
    private Renderer renderMaterial;

    private void OnEnable() {
        RhythmController.BeatTriggeredEvent += UpdatePattern;
    }

    private void OnDisable() {
        RhythmController.BeatTriggeredEvent -= UpdatePattern;
    }

    void Start () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        renderMaterial = GetComponent<Renderer>();
	}

    private void UpdatePattern() {
        if (player.Health < lowThreshold) {

            renderMaterial.material = materialPatternLow[beat % materialPatternLow.Length];

        } else if (player.Health >= highThreshold) {

            renderMaterial.material = materialPatternHigh[beat % materialPatternHigh.Length];

        } else {

            renderMaterial.material = materialPatternNormal[beat % materialPatternNormal.Length];

        }

        beat++;
    }
}
