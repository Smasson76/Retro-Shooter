using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChooseYourShip : MonoBehaviour
{
    public Selection[] Ships;
    public GameObject PlayerObject;
    public GameObject PlayerInstance;
    public Animator animator;
    public Animator instancedAnimator;
    public RuntimeAnimatorController RAC;
    private AnimatorOverrideController ORAC;
    public AnimationClip clip;
    public bool click_Next = false;
    //public bool click_Prev = false;
    public bool click_Play = false;
    private string current_state;
    private string prev_state;

    protected int index = 0;
    /**************************************************************************
    UI ELEMENTS
    **************************************************************************/
    private UnityEvent myEvent1 = new UnityEvent();
    private UnityEvent myEvent2 = new UnityEvent();
    public UnityEngine.UI.Button _playButton;
    public UnityEngine.UI.Button _nextButton;
    void Start(){
        if(GameManager.instance.PlayerInstance == null){
            GameManager.instance.SpawnPlayer();
            PlayerInstance = GameObject.FindWithTag("Player");
        }
        else{
            PlayerInstance = GameManager.instance.PlayerInstance;
        }
        animator = PlayerInstance.GetComponentInChildren<Animator>();
        PlayerInstance.transform.position = new Vector2(0f,0f);
        animator.ResetTrigger(Ships[index].Name);
        current_state = "stateBlue";
        /*************************************************************
        Button stuff
        *************************************************************/
        _playButton.onClick.AddListener(Play);
        _nextButton.onClick.AddListener(Next);
    }

    void Update(){
        prev_state = Ships[index].Name;
        int temp=0;
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
         animator.ResetTrigger(prev_state);
         animator.SetTrigger(Ships[index].Name);
         if(click_Play){
            Debug.Log("Play called!");
            GameManager.instance.SelectionMade();
            click_Play = false;
         }
        Debug.Log("Index : " + index + "\nName : " + Ships[index].Name + "\nClip : " + Ships[index].shipClip);
        Debug.Log("Ship Color : " + PlayerInstance.GetComponentInChildren<SpriteRenderer>().sharedMaterial.GetColor("_Color"));
    }
    public void Next(){
        Debug.Log("called Next()");
        click_Next = true;
        Debug.Log("Bool field : click_Next = " + click_Next);

    }
    public void Play(){
        click_Play = true;
    }
}