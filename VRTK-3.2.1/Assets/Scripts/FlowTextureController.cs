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

    private void OnEnable()
    {
        RhythmController.BeatTriggeredEvent += UpdatePattern;
    }

    private void OnDisable()
    {
        RhythmController.BeatTriggeredEvent -= UpdatePattern;
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        renderMaterial = GetComponent<Renderer>();
    }

    private void UpdatePattern()
    {
        if (player.Health < onThreshold)
        {

            renderMaterial.material = materialOff;

        }
        else
        {

            renderMaterial.material = materialOn;

        }
    }
}
