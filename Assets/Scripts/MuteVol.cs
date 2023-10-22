using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class MuteVol : MonoBehaviour
{
    [SerializeField] public bool toggleMusic, toggleFX;

    public void Toggle()
    {
        if(toggleMusic){
            musicManager.Instance.ToggleMusic();
        }
        if(toggleFX){
            musicManager.Instance.ToggleFX();
        }
    }
}