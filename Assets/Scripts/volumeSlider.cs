using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class volumeSlider : MonoBehaviour
{
    [SerializeField] public Slider slider;
    void Start()
    {
        musicManager.Instance.ChangeMasterVolume(slider.value);
        slider.onValueChanged.AddListener(val => musicManager.Instance.ChangeMasterVolume(val));
    }

}