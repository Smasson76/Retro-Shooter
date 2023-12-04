using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameCanvas : MonoBehaviour
{
    public static GameCanvas instance;
    void Awake(){
        if(instance == null){
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else{
            Destroy(gameObject);
        }
    }
}