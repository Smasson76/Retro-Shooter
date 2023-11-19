using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChooseYourShip : MonoBehaviour
{
    public static ChooseYourShip instance;
    public Selection[] Ships;
    public GameObject PlayerObject;
    public GameObject PlayerInstance;
    public Animator source_animator;
    public Animator inst_animator;
    public bool click_Next = false;

    public bool click_Play = false;
    private string current_state;
    private string prev_state;

    public string stateatDeath;
    protected int index = 0;
    /**************************************************************************
    UI ELEMENTS
    **************************************************************************/
    private UnityEvent myEvent1 = new UnityEvent();
    private UnityEvent myEvent2 = new UnityEvent();
    public UnityEngine.UI.Button _playButton;
    public UnityEngine.UI.Button _nextButton;

    void Awake(){
        if(instance == null){
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else{
            Destroy(gameObject);
        }
    }
    void Start(){
        if(GameManager.instance.PlayerInstance == null){
            GameManager.instance.SpawnPlayer();
            PlayerInstance = GameObject.FindWithTag("Player");
        }
        else{
            PlayerInstance = GameManager.instance.PlayerInstance;
        }
        inst_animator = PlayerInstance.GetComponentInChildren<Animator>();
        source_animator = Instantiate(PlayerObject.GetComponentInChildren<Animator>());
        source_animator.transform.position = new Vector2(-1000f,-1000f);
        PlayerInstance.transform.position = new Vector2(0f,0f);
        current_state = "stateBlue";
        /*************************************************************
        Button stuff
        *************************************************************/
        _playButton.onClick.AddListener(Play);
        _nextButton.onClick.AddListener(Next);
    }

    void Update(){
        prev_state = Ships[index].Name;
        if(click_Next){
            Debug.Log("Next ship has been called");
            if(index < Ships.Length-1 && click_Next){
                index++;
                click_Next = false;
            }
            else{
                index = 0;
                click_Next = false;
            }
        }
         //inst_animator.ResetTrigger(prev_state);
         inst_animator.SetTrigger(Ships[index].Name);
         if(click_Play){
            Debug.Log("Play called!");
            GameManager.instance.SelectionMade();
            musicManager.Instance.playSound("selected");
            GameManager.instance.setAnim_string(Ships[index].Name);
            source_animator.SetTrigger(Ships[index].Name);
            click_Play = false;
         }
    }
    public void Next(){
        Debug.Log("called Next()");
        musicManager.Instance.playSound("nextSkin");
        click_Next = true;
        Debug.Log("Bool field : click_Next = " + click_Next);

    }
    public void Die(){
        GameManager.instance.animation_string = current_state;
        Destroy(GameManager.instance.PlayerInstance);
    }
    public void Play(){
        click_Play = true;
    }
    public GameObject Respawn()
    {
        PlayerObject.GetComponentInChildren<Animator>().SetTrigger(Ships[index].Name);
        return Instantiate(PlayerObject,new Vector2(0f,-5f),Quaternion.identity);
    }
    public string getState()
    {
        return Ships[index].Name;
    }
    public void TransferState(GameObject someShip){
        Debug.Log("Trying to set skin to " + Ships[index].Name + " which correspondes to index " + index);
        someShip.GetComponentInChildren<Animator>().SetTrigger(Ships[index].Name);
    }
    public void cloneanim(GameObject source){
        Animator anim = source.GetComponentInChildren<Animator>();
        anim.runtimeAnimatorController = source_animator.runtimeAnimatorController;
    }
}