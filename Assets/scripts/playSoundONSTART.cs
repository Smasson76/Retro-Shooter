using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playSoundONSTART : MonoBehaviour
{
    [SerializeField] private AudioClip clip;
    void Start()
    {
        musicManager.Instance.playSound(clip);
    }

}