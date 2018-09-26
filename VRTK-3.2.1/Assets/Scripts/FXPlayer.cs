using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXPlayer : MonoBehaviour {

    [SerializeField]
    private List<ParticleSystem> FXes;

    public void PlayFXes()
    {
        foreach(ParticleSystem fx in FXes)
        {
            fx.Play();
        }
    }
}
