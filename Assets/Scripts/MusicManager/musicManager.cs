using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class musicManager : MonoBehaviour{
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
   	    } else {
   	        _musicSrc.clip = s.clip;
   	        _musicSrc.Play();
   	    }
    }

	public string getCurrentTrack(){
		string current_track_name = _musicSrc.clip.name;
		for (int i = 0; i < musicSounds.Length; i++){
			Sound current_sound = musicSounds[i];	
		    if (current_sound.clip.name == current_track_name){
		    	Debug.Log("Track found! " + current_track_name);
				return current_sound.name;
		    }
		}	
		Debug.Log("No hit found for " + current_track_name);
	    return _musicSrc.name;
    }

    public void playSound(string name){
        Sound s = Array.Find(sfxSounds,x=>x.name == name);
        if(s==null){
            Debug.Log("Sound not found!");
        }
        else{
            _FXsrc.clip = s.clip;
            _FXsrc.Play();
        }
    }

    public void ChangeMasterVolume(float value){
        AudioListener.volume = value;
    }

    public void ToggleMusic(){
        _musicSrc.mute = ! _musicSrc.mute;
    }

    public void ToggleFX(){
        _FXsrc.mute = ! _FXsrc.mute;
    }
}
