using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class musicManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static musicManager Instance;

    [SerializeField] public AudioSource _musicSrc,_FXsrc;
   void Awake(){
        if(Instance == null){
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else{
            Destroy(gameObject);
        }
   }
   public void playSound(AudioClip clip)
   {
        _FXsrc.PlayOneShot(clip);
   }

    public void ChangeMasterVolume(float value)
    {
        AudioListener.volume = value;
    }
    public void ToggleMusic(){
        _musicSrc.mute = ! _musicSrc.mute;
    }
    public void ToggleFX(){
        _FXsrc.mute = ! _FXsrc.mute;
    }
}