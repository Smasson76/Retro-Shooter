using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class musicManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static musicManager Instance;

    public Sound[] musicSounds , sfxSounds;
    [SerializeField] public AudioSource _musicSrc,_FXsrc;
   void Awake(){
        if(Instance == null){
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else{
            Destroy(gameObject);
        }
   }
   private void Start(){
    playMusic("StartMenu");
   }
   public void playMusic(string name){
    Sound s = Array.Find(musicSounds , x => x.name == name);
    if(s==null){
        Debug.Log("Sound not found!");
    }
    else
    {
        _musicSrc.clip = s.clip;
        _musicSrc.Play();
    }
   }
   public void playSound(string name)
   {
        Sound s = Array.Find(sfxSounds,x=>x.name == name);
        if(s==null){
            Debug.Log("Sound not found!");
        }
        else{
            _FXsrc.clip = s.clip;
            _FXsrc.Play();
        }
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
