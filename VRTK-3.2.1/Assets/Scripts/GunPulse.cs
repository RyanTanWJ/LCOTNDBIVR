using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPulse : MonoBehaviour {

    [SerializeField]
    private List<ParticleSystem> Pulses;

    public void PlayPulse()
    {
        foreach(ParticleSystem pulse in Pulses)
        {
            pulse.Play();
        }
    }
}
